using UnityEngine;

// Create a serializable wrapper class for the mesh data
// (class Mesh is sealed and not serializable)
// all properties of this class must be public for the serializer
// source: https://discussions.unity.com/t/is-it-posible-to-save-mesh-to-json/255406/2

[System.Serializable]
public class MeshSaveData
{
    public int[] triangles;
    public Vector3[] vertices;
    public Vector3[] normals;

    // add whatever properties of the mesh you need...

    public MeshSaveData(Mesh mesh)
    {
        this.vertices = mesh.vertices;
        this.triangles = mesh.triangles;
        this.normals = mesh.normals;
        // further properties...
    }
}