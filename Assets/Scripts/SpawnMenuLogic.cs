using UnityEngine;

public class SpawnObjectMenu : MonoBehaviour
{

    public GameObject spawnMenu;
    public Mesh furnitureMeshFilter;
    public Material furnitureMaterial;

    bool isOpened = false;

    private void Start()
    {
        spawnMenu.SetActive(false);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            OpenMenu();
            Debug.Log("Spawn Menu used");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.Y) && isOpened)
        {
            OnClick();
        }
    }

    void OpenMenu()
    {
        spawnMenu.SetActive(!isOpened);
        isOpened = !isOpened;
    }

    public void OnClick()
    {
        Debug.Log("Object spawned");
        GameObject furniture = new GameObject("Furniture Visualization (Spawned)");
            
        BoxCollider collider = furniture.AddComponent<BoxCollider>();
        FurnitureVisualization furnitureVis = furniture.AddComponent<FurnitureVisualization>();

        // Create child "MeshBounds"
        GameObject childMeshBounds = new GameObject("MeshBounds");
        childMeshBounds.AddComponent<MeshFilter>();
        childMeshBounds.AddComponent<MeshRenderer>();
        childMeshBounds.transform.SetParent(furnitureVis.transform);

        // Create child "ScalingHelper"
        GameObject childScalingHelper = new GameObject("ScalingHelper");
        childScalingHelper.transform.SetParent(furnitureVis.transform);

        // Create child of ScalingHelper "Furniture Piece"
        GameObject subchildFurniturePiece = new GameObject("Furniture Piece");
        MeshFilter meshFilter = subchildFurniturePiece.AddComponent<MeshFilter>();
        meshFilter.mesh = furnitureMeshFilter;
        subchildFurniturePiece.transform.SetParent(childScalingHelper.transform);
        MeshRenderer renderer = subchildFurniturePiece.AddComponent<MeshRenderer>();
        renderer.material = furnitureMaterial;

        furnitureVis.Set(collider, childMeshBounds.GetComponent<MeshFilter>(), childMeshBounds.GetComponent<MeshRenderer>(), childScalingHelper.transform);
       

        // TODO: Scale furniture properly, select spawn position, UI interaction, proper UI
    }
}
