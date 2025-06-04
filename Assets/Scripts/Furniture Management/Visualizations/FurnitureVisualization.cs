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
    [SerializeField] MeshRenderer meshBoundsRenderer;
    [SerializeField] Transform scaleHelper;

    // todo add mvoeable readonly bool
    public bool Moveable { get; private set; }
    /// <summary>
    /// We are using a custom direction decoupled from metas orientations //TODO
    /// </summary>
    public Vector3 CustomFurnitureDirection { get; private set; }

    // Finn 04.06., used in SpawnMenuLogic for creating new furniture
    public void Set(BoxCollider boxC, MeshFilter meshF, MeshRenderer meshR, Transform scaleH)
    {
        boxCollider = boxC;
        meshBounds = meshF;
        meshBoundsRenderer = meshR;
        scaleHelper = scaleH;
    }

    public void VisualizeFromData(FurnitureData data, LabelToModelConversionTable labelToMeshConversionTable)
    {
        localDataCopy = data.DeepCopy();
        transform.localPosition = data.posInRoom;
        transform.localRotation = data.rotInRoom;

        this.labelToMeshConversionTableRef = labelToMeshConversionTable;

        bool isWall = FurnitureLabelUtilities.IsLabelFlatWall(data.label);

        

        if (isWall)
        {
            Debug.Log($"{data.label} is wall {gameObject.GetHashCode()}");
            visualizedFurniturePiece = CreateWallMesh(data, labelToMeshConversionTable.defaultWallMaterial);
            Moveable = false;
        }
        else
        {
            Debug.Log($"{data.label} is not wall  {gameObject.GetHashCode()}");


            meshBounds.sharedMesh = CreateBoundsMesh(data);
            visualizedFurniturePiece = SelectAndDisplayFurnitureMesh();
            AdjustMeshRotation();
            AdjustMeshPositionOffset(data);
            AdjustMeshScaling(data);
            SetBoxCollider();

            Moveable = true;

        }

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

    void AdjustMeshRotation()
    {
        // Metas scannedm eshes have weird rotations and centers, so we need to adjust them
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

        scaleHelper.transform.position += posOffset;
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
