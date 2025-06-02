using UnityEngine;

public class AnchorVisualization:MonoBehaviour
{
    // visualizes the bounds and tags, maybe also offer option to edit them?


    // have a gizmo here showing forward, thats important for setting up the furniture properly later

    FurnitureData localDataCopy;
    [SerializeField] GameObject visualizationBox;

    public void VisualizeFromData(FurnitureData data)
    {
        localDataCopy = data.DeepCopy();
        transform.localPosition = data.posInRoom;
        transform.localRotation = data.rotInRoom;

        if (data.type == FurnitureType.FloorAndWalls)
        {
            //temp for now, use actual scanned mesh later
            visualizationBox.transform.localScale = Vector3.one * 0.2f;
        }
        else if (data.type == FurnitureType.Furniture)
        {
            visualizationBox.transform.localScale = data.volumeBounds.size;
        }
    }


    public FurnitureData ConvertToFurnitureDataObject()
    {
        localDataCopy.posInRoom = transform.localPosition;
        localDataCopy.rotInRoom = transform.localRotation;

        return localDataCopy;
    }
}