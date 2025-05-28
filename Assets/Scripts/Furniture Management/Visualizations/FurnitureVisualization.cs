using UnityEngine;

public class FurnitureVisualization : MonoBehaviour
{
    // used to isplay the furnitues, maybe also later have some collision etc to interact with furniture interactor to be able to be moved


    // for the visualization of furniture, only allow it to change position and rotation with ray interactor


    // also have some sort of classifier here, that maps the labels to a mesh
    
    FurnitureData localDataCopy;

    GameObject visualizedFurniturePiece;

    LabelToModelConversionTable labelToMeshConversionTableRef;

    public void VisualizeFromData(FurnitureData data, LabelToModelConversionTable labelToMeshConversionTable)
    {
        localDataCopy = data.DeepCopy();
        transform.localPosition = data.posInRoom;
        transform.localRotation = data.rotInRoom;

        this.labelToMeshConversionTableRef = labelToMeshConversionTable;

        SelectAndDisplayMesh();
    }

    void SelectAndDisplayMesh()
    {
        GameObject furnitureToSpawn = null;

        Debug.Log($"localDataCopy.label {localDataCopy.label}");
        Debug.Log($"labelToMeshConversionTableRef {labelToMeshConversionTableRef}");
        Debug.Log($"labelToMeshConversionTableRef.labelToPrefabDict {labelToMeshConversionTableRef.labelToPrefabDict}");

        if (labelToMeshConversionTableRef.labelToPrefabDict.ContainsKey(localDataCopy.label))
        {
            furnitureToSpawn = labelToMeshConversionTableRef.labelToPrefabDict[localDataCopy.label];
        }
        else
        {
            furnitureToSpawn = labelToMeshConversionTableRef.defaultPrefab;
        }

        visualizedFurniturePiece = Instantiate(furnitureToSpawn, transform);
    }


    public FurnitureData ConvertToFurnitureDataObject()
    {
        localDataCopy.posInRoom = transform.localPosition;
        localDataCopy.rotInRoom = transform.localRotation;

        return localDataCopy;
    }
}
