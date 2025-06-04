using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] RoomsManager roomsManager;
    [SerializeField] OVRPassthroughLayer ovrPassthroughLayer;

    void Start()
    {
        roomsManager.ShowRoomScan();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            switchScene();
        }

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            roomsManager.CaptureCurrentMetaRoom();
        }
    }

    void switchScene()
    {
        // change from AR to VR (ADD SKYBOX!!)
        if (ovrPassthroughLayer.enabled)
        {
            roomsManager.SaveRoomScanFromVisualization();
            ovrPassthroughLayer.enabled = !ovrPassthroughLayer.enabled;
            roomsManager.ShowRoomVariation(0);
        }
        // change from VR to AR
        else
        {
            roomsManager.SaveRoomVariationFromVisualization(0);
            ovrPassthroughLayer.enabled = !ovrPassthroughLayer.enabled;
            roomsManager.ShowRoomScan();
        }
    }
}
