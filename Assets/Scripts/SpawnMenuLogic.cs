using System.Collections.Generic;
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

    public RayInteraction rayInteraction;
    public RoomsManager roomManager;

    [SerializeField]
    private List<FurnitureItem> furnitureList = new List<FurnitureItem>();

    bool isOpened = false;
    int currentItem = 0;

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
            spawnFurniture();
            updateMenu(currentItem);
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft) && isOpened)
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

        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight) && isOpened)
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
        updateMenu(currentItem);

        spawnMenu.SetActive(!isOpened);
        isOpened = !isOpened;
    }

    private GameObject furniture;

    public void OnClick()
    {
        //Debug.Log("Object spawned");
        //furniture = new GameObject("Furniture Visualization (Spawned)");
        //furniture.transform.position += new Vector3(50f, 0f, 0f);

        //// Add script
        //BoxCollider collider = furniture.AddComponent<BoxCollider>();
        //FurnitureVisualization furnitureVis = furniture.AddComponent<FurnitureVisualization>();

        //// Create child "MeshBounds"
        //GameObject childMeshBounds = new GameObject("MeshBounds");
        //childMeshBounds.AddComponent<MeshFilter>();
        //childMeshBounds.AddComponent<MeshRenderer>();
        //childMeshBounds.transform.SetParent(furnitureVis.transform);

        //// Create child "ScalingHelper"
        //GameObject childScalingHelper = new GameObject("ScalingHelper");
        //childScalingHelper.transform.SetParent(furnitureVis.transform);

        //// Create child of ScalingHelper "Furniture Piece"
        //GameObject subchildFurniturePiece = new GameObject("Furniture Piece");
        //MeshFilter meshFilter = subchildFurniturePiece.AddComponent<MeshFilter>();
        //meshFilter.mesh = furnitureList[currentItem].mesh;
        //meshFilter.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //subchildFurniturePiece.transform.SetParent(childScalingHelper.transform);
        //MeshRenderer renderer = subchildFurniturePiece.AddComponent<MeshRenderer>();
        //renderer.material = furnitureList[currentItem].material;

        //furnitureVis.Set(collider, childMeshBounds.GetComponent<MeshFilter>(), childMeshBounds.GetComponent<MeshRenderer>(), childScalingHelper.transform);
        //furnitureVis.Moveable = true;

        //furniture.transform.position += new Vector3(0f, 0f, 1f);
        //furniture.layer = LayerMask.NameToLayer("Furniture Visualization");


    }

    void spawnFurniture()
    {
        FurnitureItem furnitureItemToSpawn = furnitureList[currentItem];
        roomManager.AddFurnitureToCurrentVisualization(furnitureItemToSpawn.data);
    }

    private void updateMenu(int item)
    {
        Debug.Log("Updating menu");

        menuOption.text = furnitureList[item].fName;
        menuImage.sprite = furnitureList[item].image;
        count.text = (item + 1) + " / " + (furnitureList.Count);
    }
}
