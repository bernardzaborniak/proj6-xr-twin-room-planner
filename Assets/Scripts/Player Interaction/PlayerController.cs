using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] RoomsManager roomsManager;
    [SerializeField] OVRPassthroughLayer ovrPassthroughLayer;

    [Header("Player Controller Components")]
    [SerializeField] PlayerControllerReferences refs;
    [SerializeField] PlayerControllerConfig config;
    [Tooltip("Do not set any values here before start")]
    [SerializeField] PlayerControllerRuntimeData runtimeData;


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

        roomsManager.SaveRoomVariationFromVisualization(0);
        ovrPassthroughLayer.enabled = true;
        roomsManager.ShowRoomScan();

        playerControllerStateMachine.SetState(playerControllerStateMachine.scanSelection);
    }

    void ChangeToLayoutMode()
    {
        currentMode = CurrentRoomMode.LayoutMode;

        roomsManager.SaveRoomScanFromVisualization();
        ovrPassthroughLayer.enabled = false;
        roomsManager.ShowRoomVariation(0);

        playerControllerStateMachine.SetState(playerControllerStateMachine.layoutSelectionAndMove);
    }

   
}
