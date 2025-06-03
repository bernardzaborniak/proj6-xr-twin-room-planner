using UnityEngine;

public class FurnitureVisualization : MonoBehaviour
{
    // used to isplay the furnitues, maybe also later have some collision etc to interact with furniture interactor to be able to be moved


    // for the visualization of furniture, only allow it to change position and rotation with ray interactor


    FurnitureData localDataCopy;
    GameObject visualizedFurniturePiece;
    LabelToModelConversionTable labelToMeshConversionTableRef;

    [SerializeField] BoxCollider boxCollider;

    // todo add mvoeable readonly bool
    public bool Moveable { get; private set; }

    public void VisualizeFromData(FurnitureData data, LabelToModelConversionTable labelToMeshConversionTable)
    {
        localDataCopy = data.DeepCopy();
        transform.localPosition = data.posInRoom;
        transform.localRotation = data.rotInRoom;

        this.labelToMeshConversionTableRef = labelToMeshConversionTable;

        SelectAndDisplayMesh();

        // Resize to unit size 1x1x1
        MeshRenderer meshRenderer = visualizedFurniturePiece.GetComponent<MeshRenderer>();

   
        Bounds meshBounds = meshRenderer.bounds;
        Vector3 normalizedScale = new Vector3(
            meshRenderer.bounds.size.x != 0 ? 1f / meshRenderer.bounds.size.x : 0f,
            meshRenderer.bounds.size.y != 0 ? 1f / meshRenderer.bounds.size.y : 0f,
            meshRenderer.bounds.size.z != 0 ? 1f / meshRenderer.bounds.size.z : 0f
        );

        // Now combine normalized 1x1x1 size with the size we extracted from the scan
        // scale is still fuced up a bit at the moment when loading form save data :/
        Vector3 newSize = Vector3.Scale(normalizedScale, data.volumeBounds.size);

        boxCollider.size = data.volumeBounds.size;
        boxCollider.center = new Vector3(0f, boxCollider.size.y/2, 0f);

        visualizedFurniturePiece.transform.localScale = newSize;

        // Floors and walls are not movable.
        Moveable = data.type == FurnitureType.Furniture;
       
    }

    void SelectAndDisplayMesh()
    {
        GameObject furnitureToSpawn = null;

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
