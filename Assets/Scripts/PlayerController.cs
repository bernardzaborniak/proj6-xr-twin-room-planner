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
    }

    void switchScene()
    {
        // "Disabling destroys objects that are within the scope of the respective components, such as layers and projection meshes, which must be recreated when re-enabled."
        // -> roomscan within scope? save objects before and load after switch necessary?
        // are both directions covered?
        if (ovrPassthroughLayer.enabled)
        {
            roomsManager.SaveRoomScanFromVisualization();
            //Debug.Log("Saved");
            ovrPassthroughLayer.enabled = !ovrPassthroughLayer.enabled;
            //Debug.Log("Switched");
            roomsManager.ShowRoomVariation(0);
            //Debug.Log("Loaded");
        }
        else
        {
            roomsManager.SaveRoomVariationFromVisualization(0);
            //Debug.Log("Saved");
            ovrPassthroughLayer.enabled = !ovrPassthroughLayer.enabled;
            //Debug.Log("Switched");
            roomsManager.ShowRoomScan();
            //Debug.Log("Loaded");
        }
    }
}
