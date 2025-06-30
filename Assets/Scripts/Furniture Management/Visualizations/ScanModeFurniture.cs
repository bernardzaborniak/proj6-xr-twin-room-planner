using System.Data.Common;
using UnityEngine;

public class ScanModeFurniture : BaseFurniture
{
    // visualizes the bounds and tags, maybe also offer option to edit them?


    // have a gizmo here showing forward, thats important for setting up the furniture properly later

    //FurnitureData localDataCopy;
    //[SerializeField] MeshFilter boundingBoxMeshFilter;
    //[SerializeField] MeshRenderer boundingBoxMeshRenderer;
    //[SerializeField] BoxCollider boxCollider;

    ScanModeFurnitureUiMenu scanUiMenu;


    public void VisualizeFromData(FurnitureData data)
    {
        LocalDataCopy = data.DeepCopy();
        transform.localPosition = data.posInRoom;
        transform.localRotation = data.rotInRoom;

        Mesh newMesh = new Mesh();

        newMesh.vertices = data.meshData.vertices;
        newMesh.triangles = data.meshData.triangles;
        newMesh.normals = data.meshData.normals;

        //Vector2[] uv = new Vector2[8] {new Vector2(0,0), new Vector2(0,1), new Vector2(1,1), new Vector2(1,0), new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
        //newMesh.uv = uv;

        boundingBoxMeshFilter.sharedMesh = newMesh;

        SetBoxCollider();

        if (FurnitureLabelUtilities.IsLabelFlatWall(data.label))
        {
            Interactable = false;
        }
        else
        {
            Interactable = true;
        }

        if (!(uiMenu is ScanModeFurnitureUiMenu)) Debug.Log("mak sure the uiMenu assigned is of type ScanModeFurnitureUiMenu ");
        scanUiMenu = uiMenu as ScanModeFurnitureUiMenu;
        scanUiMenu.SetUp(this);
    } 

    protected override void OnUiChangedData()
    {

    }


    void OnDrawGizmos()
    {
        //MeshFilter meshFilter = GetComponent<MeshFilter>();
        //if (meshFilter == null || meshFilter.sharedMesh == null) return;

        Mesh mesh = boundingBoxMeshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;


        Gizmos.color = Color.green;

        // Transform from local to world space
        for (int i = 0; i < vertices.Length; i++)
        {
            // Get the three vertices of the triangle
            Vector3 v0 = transform.TransformPoint(vertices[triangles[i]]);
            Vector3 v1 = transform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 v2 = transform.TransformPoint(vertices[triangles[i + 2]]);

            // Compute the center of the triangle
            Vector3 center = (v0 + v1 + v2) / 3f;

            // Compute the face normal
            Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

            // Draw the normal
            Gizmos.DrawLine(center, center + normal * 0.35f);
        }
    }
}