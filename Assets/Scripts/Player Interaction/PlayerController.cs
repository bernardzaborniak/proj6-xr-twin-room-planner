using UnityEngine;
using static Oculus.Interaction.Context;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] RoomsManager roomsManager;
    [SerializeField] OVRPassthroughLayer ovrPassthroughLayer;

    [Header("Player Controller Components")]
    [SerializeField] PlayerControllerReferences refs;
    [SerializeField] PlayerControllerConfig config;
    [Tooltip("Do not set any values here before start")]
    [SerializeField] PlayerControllerRuntimeData runtimeData;
    [SerializeField] Material skyboxMat;
    [SerializeField] GameObject floorObj;
    GameObject floorInstance;


    //[Space]
    //[Header("Furniture Mode Controllers")]
    //[SerializeField] FurnitureInteractionController layoutModeInteraction;
    //[SerializeField] SpawnObjectMenu spawnObjectMenu;
    //[SerializeField] FurnitureInteraction furnitureMoveInteraction;
    //[Header("Scan Edit Mode Controllers")]
    //[SerializeField] FurnitureInteractionController scanModeInteraction;

    [SerializeField]
    PlayerControllerInteractionStateMachine playerControllerStateMachine;


    enum CurrentRoomMode
    {
        ScanMode,
        LayoutMode
    }

    [SerializeField] CurrentRoomMode currentMode;

    bool controllerStarted = false;

    void Start()
    {
        playerControllerStateMachine = new PlayerControllerInteractionStateMachine(refs,config,runtimeData);

        refs.roomManager = RoomsManager.Instance;

        //roomsManager.ShowRoomScan();
        //SwitchMode();
    }

    void Update()
    {
        if (!controllerStarted)
            return;

        playerControllerStateMachine.Update();

        if (OVRInput.GetDown(config.switchRoomModeButton))
        {
            SwitchRoomMode();
        }

        /* if (OVRInput.GetDown(OVRInput.Button.Two))
         {
             SwitchRoomMode();
         }

         if (OVRInput.GetDown(OVRInput.Button.One))
         {
             roomsManager.CaptureCurrentMetaRoom();
         }*/
    }


    /// <summary>
    ///  needs to be called by application manager when the rooms are loaded
    /// </summary>
    public void StartWithScanMode()
    {
        controllerStarted = true;

        ChangeToScanMode();
    }

    void SwitchRoomMode()
    {
        // change from AR to VR (ADD SKYBOX!!) -> enter Layout Mode
        if (currentMode == CurrentRoomMode.ScanMode)
        {
            ChangeToLayoutMode();
        }
        // change from VR to AR -> enter Scan Mode
        else if (currentMode == CurrentRoomMode.LayoutMode)
        {
            ChangeToScanMode();
        }
    }

    void ChangeToScanMode()
    {
        currentMode = CurrentRoomMode.ScanMode;

        refs.roomManager.SaveRoomVariationFromVisualization(0);
        ovrPassthroughLayer.enabled = true;
        refs.roomManager.ShowRoomScan();

        // reset skybox for passthrough
        //RenderSettings.skybox = null;
        //Camera.main.clearFlags = CameraClearFlags.SolidColor;
        //Camera.main.backgroundColor = Color.black;

        // remove floor if exists
        if (floorInstance != null)
        {
            Destroy(floorInstance);
            floorInstance = null;
        }

        playerControllerStateMachine.SetState(playerControllerStateMachine.scanSelection);
    }

    void ChangeToLayoutMode()
    {
        currentMode = CurrentRoomMode.LayoutMode;

        refs.roomManager.SaveRoomScanFromVisualization();
        ovrPassthroughLayer.enabled = false;
        refs.roomManager.ShowRoomVariation(0);

        // set skybox
        //RenderSettings.skybox = skyboxMat;
        //Camera.main.clearFlags = CameraClearFlags.Skybox;

        // place floor
        floorInstance = Instantiate(floorObj);
        floorInstance.name = "FloorForRoom";
        floorInstance.transform.position = new Vector3(0, 0, 0);
        floorInstance.transform.localScale = new Vector3(50, 1, 50);

        playerControllerStateMachine.SetState(playerControllerStateMachine.layoutSelectionAndMove);
    }

   
}
