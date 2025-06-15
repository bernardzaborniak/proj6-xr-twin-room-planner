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
}