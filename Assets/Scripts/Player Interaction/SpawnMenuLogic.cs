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

    public FurnitureInteraction rayInteraction;
    public RoomsManager roomManager;

    [SerializeField]
    private List<SpawnMenuFurnitureItem> furnitureList = new List<SpawnMenuFurnitureItem>();

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
            SpawnFurniture();
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

    void SpawnFurniture()
    {
        SpawnMenuFurnitureItem furnitureItemToSpawn = furnitureList[currentItem];

        Vector3 spawnPoint = transform.position + (transform.forward * 0.5f) + (-Vector3.up * 0.5f);

        roomManager.AddFurnitureToCurrentVisualization(furnitureItemToSpawn.data, spawnPoint);
    }

    private void updateMenu(int item)
    {
        Debug.Log("Updating menu");

        menuOption.text = furnitureList[item].fName;
        menuImage.sprite = furnitureList[item].image;
        count.text = (item + 1) + " / " + (furnitureList.Count);
    }
}
