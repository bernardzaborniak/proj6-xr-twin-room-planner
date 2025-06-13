using System.Data.Common;
using UnityEngine;

public class ScanModeFurniture:MonoBehaviour, IInteractableFurniture
{
    // visualizes the bounds and tags, maybe also offer option to edit them?


    // have a gizmo here showing forward, thats important for setting up the furniture properly later

    FurnitureData localDataCopy;
    [SerializeField] MeshFilter visualizationMeshFilter;
    [SerializeField] MeshRenderer visualizationMeshRenderer;
    [SerializeField] BoxCollider boxCollider;

    public void VisualizeFromData(FurnitureData data)
    {
        localDataCopy = data.DeepCopy();
        transform.localPosition = data.posInRoom;
        transform.localRotation = data.rotInRoom;

        Mesh newMesh = new Mesh();

        newMesh.vertices = data.meshData.vertices;
        newMesh.triangles = data.meshData.triangles;
        newMesh.normals = data.meshData.normals;

        visualizationMeshFilter.sharedMesh = newMesh;

        SetBoxCollider();
    }


    public FurnitureData ConvertToFurnitureDataObject()
    {
        localDataCopy.posInRoom = transform.localPosition;
        localDataCopy.rotInRoom = transform.localRotation;

        return localDataCopy;
    }

    void SetBoxCollider()
    {
        boxCollider.center = visualizationMeshRenderer.localBounds.center;
        boxCollider.size = visualizationMeshRenderer.localBounds.size;
    }
}