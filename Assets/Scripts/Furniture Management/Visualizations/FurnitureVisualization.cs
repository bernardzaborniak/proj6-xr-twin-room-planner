using UnityEngine;
using UnityEngine.AI;

public class FurnitureVisualization : MonoBehaviour
{
    // used to isplay the furnitues, maybe also later have some collision etc to interact with furniture interactor to be able to be moved


    // for the visualization of furniture, only allow it to change position and rotation with ray interactor


    FurnitureData localDataCopy;
    GameObject visualizedFurniturePiece;
    LabelToModelConversionTable labelToMeshConversionTableRef;

    /// <summary>
    /// Will be used to interact with the pl,ayercontroller via raycasts.
    /// </summary>
    [SerializeField] BoxCollider boxCollider;
    /// <summary>
    /// Are used to recreate the scanned bounds for scaling and positoning.
    /// </summary>
    [SerializeField] MeshFilter meshBounds;

    // todo add mvoeable readonly bool
    public bool Moveable { get; private set; }
    /// <summary>
    /// We are using a custom direction decoupled from metas orientations //TODO
    /// </summary>
    public Vector3 CustomFurnitureDirection { get; private set; }

    public void VisualizeFromData(FurnitureData data, LabelToModelConversionTable labelToMeshConversionTable)
    {
        localDataCopy = data.DeepCopy();
        transform.localPosition = data.posInRoom;
        transform.localRotation = data.rotInRoom;

        this.labelToMeshConversionTableRef = labelToMeshConversionTable;

        meshBounds.sharedMesh = CreateBoundsMesh(data);
        visualizedFurniturePiece = SelectAndDisplayFurnitureMesh();
        AdjustMeshRotation();
        AdjustMeshPositionOffset(data);
        AdjustMeshScaling(data);



        // Floors and walls are not movable.
        Moveable = data.type == FurnitureType.Furniture;

    }


    Mesh CreateBoundsMesh(FurnitureData data)
    {
        Mesh newBoundsMesh = new Mesh();
        newBoundsMesh.vertices = data.meshData.vertices;
        newBoundsMesh.triangles = data.meshData.triangles;
        newBoundsMesh.normals = data.meshData.normals;

        return newBoundsMesh;
    }

    GameObject SelectAndDisplayFurnitureMesh()
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

        return Instantiate(furnitureToSpawn, transform);
    }

    void AdjustMeshRotation()
    {
        // Metas scannedm eshes have weird rotations and centers, so we need to adjust them

        // 1. Rotation
        //visualizedFurniturePiece.transform.localRotation *= Quaternion.Euler(-90f, 0f, 0f) * Quaternion.Euler(0f, -180f, 0f) * Quaternion.Euler(0f, 0f, 180f);

        //visualizedFurniturePiece.transform.localRotation *= Quaternion.Euler(-90f, 0f, 0f) * Quaternion.Euler(0f, -180f, 0f) * Quaternion.Euler(0f, 0f, 180f);

        visualizedFurniturePiece.transform.rotation = Quaternion.LookRotation(transform.up, Vector3.up);
    }

    void AdjustMeshPositionOffset(FurnitureData data)
    {
        Bounds bounds = meshBounds.sharedMesh.bounds;

        Vector3 boundsInWorldSpace = meshBounds.transform.TransformVector(bounds.size);

        Vector3 posOffset = Vector3.zero;

        //why defuq does COUCH have a different origin meta?
        if (data.label == FurnitureLabel.COUCH)
        {
            posOffset.y = -boundsInWorldSpace.y / 2;
        }
        else if (!FurnitureLabelUtilities.IsLabelFlatWall(data.label))
        {
            posOffset.y = -boundsInWorldSpace.y;
        }

        visualizedFurniturePiece.transform.position += posOffset;
    }


    void AdjustMeshScaling(FurnitureData data)
    {
        // Resize to unit size 1x1x1   , thats all for now
        MeshRenderer furnitureMeshRenderer = visualizedFurniturePiece.GetComponent<MeshRenderer>();

        Bounds furnitureMeshBounds = furnitureMeshRenderer.localBounds;
        Vector3 size = furnitureMeshBounds.size;
        float maxDimension = Mathf.Max(size.x, size.y, size.z);


        Debug.Log($"before bounds {furnitureMeshRenderer.bounds}");

        Vector3 scaleFactor = new Vector3(
           size.x > 0 ? 1f / size.x : 1f,
           size.y > 0 ? 1f / size.y : 1f,
           size.z > 0 ? 1f / size.z : 1f
       );

        /*Vector3 normalizedFurnitureScale = new Vector3(
            furnitureMeshRenderer.bounds.size.x != 0 ? 1f / furnitureMeshRenderer.bounds.size.x : 0f,
            furnitureMeshRenderer.bounds.size.y != 0 ? 1f / furnitureMeshRenderer.bounds.size.y : 0f,
            furnitureMeshRenderer.bounds.size.z != 0 ? 1f / furnitureMeshRenderer.bounds.size.z : 0f
        );*/

        Vector3 normalizedFurnitureScale = visualizedFurniturePiece.transform.localScale = scaleFactor;

        Debug.Log($"new local scale is: {normalizedFurnitureScale}");
        visualizedFurniturePiece.transform.localScale = normalizedFurnitureScale;
        Debug.Log($"new bounds {furnitureMeshRenderer.bounds}");


        // Vector3 newPos = visualizedFurniturePiece.transform.localPosition;
        //newPos.y = newPos.y - furnitureMeshRenderer.bounds.size.y / 2;
        //visualizedFurniturePiece.transform.localPosition = newPos;
    }


    public FurnitureData ConvertToFurnitureDataObject()
    {
        localDataCopy.posInRoom = transform.localPosition;
        localDataCopy.rotInRoom = transform.localRotation;

        return localDataCopy;
    }
}
