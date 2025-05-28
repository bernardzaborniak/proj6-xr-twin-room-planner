using UnityEngine;

public class AnchorVisualization:MonoBehaviour
{
    // visualizes the bounds and tags, maybe also offer option to edit them?


    // have a gizmo here showing forward, thats important for setting up the furniture properly later

    FurnitureData localDataCopy;

    public void VisualizeFromData(FurnitureData data)
    {
        localDataCopy = data.DeepCopy();
        transform.localPosition = data.posInRoom;
        transform.localRotation = data.rotInRoom;
    }


    public FurnitureData ConvertToFurnitureDataObject()
    {
        localDataCopy.posInRoom = transform.localPosition;
        localDataCopy.rotInRoom = transform.localRotation;

        return localDataCopy;
    }
}