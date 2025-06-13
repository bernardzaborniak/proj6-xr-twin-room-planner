using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] RoomsManager roomsManager;
    [SerializeField] OVRPassthroughLayer ovrPassthroughLayer;
    [Space]
    [Header("Furniture Mode Controllers")]
    [SerializeField] FurnitureInteractionController layoutModeInteraction;
    [SerializeField] SpawnObjectMenu spawnObjectMenu;
    [SerializeField] FurnitureInteraction furnitureMoveInteraction;
    [Header("Scan Edit Mode Controllers")]
    [SerializeField] FurnitureInteractionController scanModeInteraction;

    [SerializeField] GameObject temp;


    enum CurrentMode
    {
        ScanMode,
        LayoutMode
    }

    [SerializeField] CurrentMode currentMode;

    void Start()
    {
        ChangeToScanMode();
        //roomsManager.ShowRoomScan();
        //SwitchMode();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            SwitchMode();
        }

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            roomsManager.CaptureCurrentMetaRoom();
        }
    }

    void SwitchMode()
    {
        // change from AR to VR (ADD SKYBOX!!) -> enter Layout Mode
        if (currentMode == CurrentMode.ScanMode)
        {
            ChangeToLayoutMode();
        }
        // change from VR to AR -> enter Scan Mode
        else if (currentMode == CurrentMode.LayoutMode)
        {
            ChangeToScanMode();
        }
    }

    void ChangeToScanMode()
    {
        currentMode = CurrentMode.ScanMode;

        roomsManager.SaveRoomVariationFromVisualization(0);
        ovrPassthroughLayer.enabled = true;
        roomsManager.ShowRoomScan();

        layoutModeInteraction.gameObject.SetActive(false);
        scanModeInteraction.gameObject.SetActive(true);
        spawnObjectMenu.gameObject.SetActive(false);
        furnitureMoveInteraction.gameObject.SetActive(false);
    }

    void ChangeToLayoutMode()
    {
        currentMode = CurrentMode.LayoutMode;

        roomsManager.SaveRoomScanFromVisualization();
        ovrPassthroughLayer.enabled = false;
        roomsManager.ShowRoomVariation(0);

        layoutModeInteraction.gameObject.SetActive(true);
        scanModeInteraction.gameObject.SetActive(false);
        spawnObjectMenu.gameObject.SetActive(true);
        furnitureMoveInteraction.gameObject.SetActive(true);
    }

   
}
