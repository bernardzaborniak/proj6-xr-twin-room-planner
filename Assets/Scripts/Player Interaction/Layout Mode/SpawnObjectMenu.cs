using System.Collections.Generic;
using Meta.XR.MRUtilityKit.SceneDecorator;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SpawnObjectMenu : MonoBehaviour
{

    public GameObject spawnMenu;
    public TMP_Text menuOption;
    public UnityEngine.UI.Image menuImage;
    public TMP_Text count;


    [SerializeField]
    private List<SpawnMenuFurnitureItem> furnitureList = new List<SpawnMenuFurnitureItem>();

    //bool isOpened = false;
    int currentItem = 0;

    private void Awake()
    {
        updateMenu(0);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            OpenMenu();
            Debug.Log("Spawn Menu used");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            SpawnFurniture();
            updateMenu(currentItem);
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft))
        {
            if (currentItem > 0)
            {
                currentItem -= 1;
            }
            else
            {
                currentItem = furnitureList.Count - 1;
            }
            updateMenu(currentItem);
        }

        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight))
        {
            if (currentItem < furnitureList.Count - 1)
            {
                currentItem += 1;
            }
            else
            {
                currentItem = 0;
            }
            updateMenu(currentItem);
        }
    }

    void OpenMenu()
    {
        EventLogger.Instance?.LogInteraction("Opened furniture spawn menu.", EventLogger.actionTypes.MenuOpened);
        updateMenu(currentItem);
    }

    private GameObject furniture;

    void SpawnFurniture()
    {
        EventLogger.Instance?.LogInteraction("Furniture spawned.", EventLogger.actionTypes.ObjectAdded);

        SpawnMenuFurnitureItem furnitureItemToSpawn = furnitureList[currentItem];

        Vector3 spawnPoint = transform.position + (transform.forward * 0.5f) + (-Vector3.up * 0.5f);

        if(spawnPoint.y<0) spawnPoint.y = 0;

        // temporary spawn the mesh to calculate its bounding box
        GameObject spawnedTempFurniture = Instantiate(RoomsManager.Instance.labelToMeshConversionTable.labelToPrefabDict[furnitureItemToSpawn.label]);
        MeshFilter boundsMeshFilter = spawnedTempFurniture.GetComponent<MeshFilter>();

        if (boundsMeshFilter == null)
        {
            Debug.LogError(" make sure tha assigned furniture object has a mesh filter inside the spawn menu");
        }
        Bounds bounds = boundsMeshFilter.sharedMesh.bounds;
        Mesh boundsMesh = GenerateBoundsMesh(bounds, furnitureItemToSpawn.boundsSizeAdjustment);

        spawnedTempFurniture.SetActive(false);
        Destroy(spawnedTempFurniture);

        FurnitureData newData = new FurnitureData();
        newData.posInRoom = spawnPoint;
        newData.rotInRoom = Quaternion.identity;
        newData.meshData = new MeshSaveData(boundsMesh);
        newData.label = furnitureItemToSpawn.label;

        RoomsManager.Instance.AddFurnitureToCurrentVisualization(newData, spawnPoint);
    }

    private void updateMenu(int item)
    {
        EventLogger.Instance?.LogInteraction("Switched furniture menu selection.", EventLogger.actionTypes.MenuInteraction);
        Debug.Log("Updating menu");

        menuOption.text = furnitureList[item].fName;
        menuImage.sprite = furnitureList[item].image;
        count.text = (item + 1) + " / " + (furnitureList.Count);
    }

    // this ones from chat gpt
    Mesh GenerateBoundsMesh(Bounds bounds, Vector3 boundsSizeAdjustment)
    {
        Mesh mesh = new Mesh();

        Vector3[] unitCubeVertices = {
        // front face
        new Vector3(-0.5f, -0.5f,  0.5f),
        new Vector3( 0.5f, -0.5f,  0.5f),
        new Vector3( 0.5f,  0.5f,  0.5f),
        new Vector3(-0.5f,  0.5f,  0.5f),
        // back face
        new Vector3(-0.5f, -0.5f, -0.5f),
        new Vector3( 0.5f, -0.5f, -0.5f),
        new Vector3( 0.5f,  0.5f, -0.5f),
        new Vector3(-0.5f,  0.5f, -0.5f),
    };

        int[] triangles = {
        // front
        0, 2, 1, 0, 3, 2,
        // right
        1, 2, 6, 1, 6, 5,
        // back
        5, 6, 7, 5, 7, 4,
        // left
        4, 7, 3, 4, 3, 0,
        // top
        3, 7, 6, 3, 6, 2,
        // bottom
        4, 0, 1, 4, 1, 5
    };

        Vector3[] transformedVertices = new Vector3[unitCubeVertices.Length];
        for (int i = 0; i < unitCubeVertices.Length; i++)
        {
            // Scale and offset according to bounds
            transformedVertices[i] = Vector3.Scale(unitCubeVertices[i], Vector3.Scale(bounds.size ,boundsSizeAdjustment)) + bounds.center;
        }

        mesh.vertices = transformedVertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
