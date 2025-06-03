using System.Data.Common;
using UnityEngine;

public class AnchorVisualization:MonoBehaviour
{
    // visualizes the bounds and tags, maybe also offer option to edit them?


    // have a gizmo here showing forward, thats important for setting up the furniture properly later

    FurnitureData localDataCopy;
    [SerializeField] MeshFilter visualizationMesh;

    public void VisualizeFromData(FurnitureData data)
    {
        localDataCopy = data.DeepCopy();
        transform.localPosition = data.posInRoom;
        transform.localRotation = data.rotInRoom;

        Debug.Log($"[Bern[ Visualize from Data: {data.label} mesh vertex 0: {data.meshData.vertices[0]}");
        Mesh newMesh = new Mesh();
        //mesh.Clear();

        newMesh.vertices = data.meshData.vertices;
        newMesh.triangles = data.meshData.triangles;
        newMesh.normals = data.meshData.normals;

        visualizationMesh.sharedMesh = newMesh;

        /*
                if (data.type == FurnitureType.FloorAndWalls)
                {
                    //temp for now, use actual scanned mesh later
                    visualizationMesh.transform.localScale = Vector3.one * 0.2f;
                }
                else if (data.type == FurnitureType.Furniture)
                {
                    visualizationMesh.transform.localScale = data.volumeBounds.size;
                }*/
    }


    public FurnitureData ConvertToFurnitureDataObject()
    {
        localDataCopy.posInRoom = transform.localPosition;
        localDataCopy.rotInRoom = transform.localRotation;

        return localDataCopy;
    }
}