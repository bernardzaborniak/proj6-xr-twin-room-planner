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
    [SerializeField] MeshFilter meshBoundsFilter;
    [SerializeField] MeshRenderer meshBoundsRenderer;
    [SerializeField] Transform scaleHelper;

    // todo add mvoeable readonly bool
    public bool Moveable { get; set; }
    /// <summary>
    /// We are using a custom direction decoupled from metas orientations //TODO
    /// </summary>
    public Vector3 CustomFurnitureDirection { get; private set; }


    public void VisualizeFromData(FurnitureData data, LabelToModelConversionTable labelToMeshConversionTable)
    {
        localDataCopy = data.DeepCopy();
        transform.localPosition = data.posInRoom;
        transform.localRotation = data.rotInRoom;

        // adjust rotation to change showing direction mostly 90/180 or 270 degrees
        Debug.Log($"rotate scaling helper {gameObject} by {data.rotationAdjuster}");
        scaleHelper.Rotate(Vector3.up, data.rotationAdjuster);

        this.labelToMeshConversionTableRef = labelToMeshConversionTable;

        bool isWall = FurnitureLabelUtilities.IsLabelFlatWall(data.label);

        
        if (isWall)
        {
            visualizedFurniturePiece = CreateWallMesh(data, labelToMeshConversionTable.defaultWallMaterial);
            meshBoundsFilter.sharedMesh = visualizedFurniturePiece.GetComponent<MeshFilter>().mesh;
           Moveable = false;
        }
        else
        {
            meshBoundsFilter.sharedMesh = CreateBoundsMesh(data);
            visualizedFurniturePiece = SelectAndDisplayFurnitureMesh();
            AdjustMeshScaling(data);

            Moveable = true;

        }

        SetBoxCollider();

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

        return Instantiate(furnitureToSpawn, scaleHelper);
    }

    GameObject CreateWallMesh(FurnitureData data, Material defaultWallMaterial)
    {
        GameObject furnitureToSpawn = new GameObject("Wall Mesh");
        furnitureToSpawn.transform.parent = scaleHelper;
        furnitureToSpawn.transform.localPosition = Vector3.zero;
        furnitureToSpawn.transform.localRotation = Quaternion.identity;

        MeshRenderer renderer = furnitureToSpawn.AddComponent<MeshRenderer>();
        renderer.material = defaultWallMaterial;
        MeshFilter filter = furnitureToSpawn.AddComponent<MeshFilter>();
        filter.sharedMesh = new Mesh();
        filter.sharedMesh.vertices = data.meshData.vertices;
        filter.sharedMesh.triangles = data.meshData.triangles;
        filter.sharedMesh.normals = data.meshData.normals;


        return furnitureToSpawn;
    }



    void AdjustMeshScaling(FurnitureData data)
    {
        // Resize to unit size 1x1x1   , thats all for now
        MeshRenderer furnitureMeshRenderer = visualizedFurniturePiece.GetComponent<MeshRenderer>();

        Bounds furnitureMeshBounds = furnitureMeshRenderer.localBounds;
        Vector3 size = furnitureMeshBounds.size;
        float maxDimension = Mathf.Max(size.x, size.y, size.z);

        Vector3 normalizedFurnitureScale = new Vector3(
           size.x > 0 ? 1f / size.x : 1f,
           size.y > 0 ? 1f / size.y : 1f,
           size.z > 0 ? 1f / size.z : 1f
       );

        visualizedFurniturePiece.transform.localScale = normalizedFurnitureScale;

        scaleHelper.transform.localScale = meshBoundsRenderer.localBounds.size;
    }

    void SetBoxCollider()
    {
        boxCollider.center = meshBoundsRenderer.localBounds.center;
        boxCollider.size = meshBoundsRenderer.localBounds.size;
    }


    public FurnitureData ConvertToFurnitureDataObject()
    {
        localDataCopy.posInRoom = transform.localPosition;
        localDataCopy.rotInRoom = transform.localRotation;

        return localDataCopy;
    }
}
