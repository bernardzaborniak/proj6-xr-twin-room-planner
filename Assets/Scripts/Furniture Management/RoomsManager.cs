using Meta.XR.MRUtilityKit;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class RoomsManager : MonoBehaviour
{
    // Manages the scanned room data and the various furniture rom variations.


    /// <summary>
    /// This is the scanned and potentially adjusted room to use as a base for the others
    /// </summary>
    [SerializeField] RoomData roomScanData;

    [SerializeField] RoomVisualization currentVisualization;
    [SerializeField] GameObject roomAnchorsVisualizationPrefab;

    /// <summary>
    /// Those are the differents layouts of furniture that we tried out.
    /// </summary>
    [SerializeField] List<RoomData> roomVariationsData;
    [SerializeField] GameObject roomVariationVisualizationPrefab;





    void Start()
    {
        LoadSavedRoomData(); // load stuff saved on persistence drive first
    }

    void Update()
    {
        // Record current room
        if (Input.GetKeyDown(KeyCode.C))
        {
            Create2ExampleRoomVariations();
        }

        // Record current room
        if (Input.GetKeyDown(KeyCode.R))
        {
            MRUKRoom room = MRUK.Instance.GetCurrentRoom();
            roomScanData = ConvertMetaRoomToAppRoom(room);
        }

        // Save current room
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveRoomScan();
        }
    }

    public void LoadSavedRoomData()
    {
        string path = Application.persistentDataPath;

        string roomScanPath = Path.Combine(path, "roomScan.json");
        if (File.Exists(roomScanPath))
        {
            string json = File.ReadAllText(roomScanPath);
            roomScanData = JsonUtility.FromJson<RoomData>(json);
        }

        string[] allRoomVariations = Directory.GetFiles(path, "roomVariation*.json");
        foreach (string file in allRoomVariations)
        {
            string json = File.ReadAllText(file);
            RoomData variation = JsonUtility.FromJson<RoomData>(json);
            if (variation != null)
            {
                roomVariationsData.Add(variation);
            }
        }
    }

    public void Create2ExampleRoomVariations()
    {
        RoomData roomVar1 = roomScanData.DeepCopy();
        RoomData roomVar2 = roomScanData.DeepCopy();

        roomVariationsData.Add(roomVar1);
        roomVariationsData.Add(roomVar2);

        SaveRoomVariation(0);
        SaveRoomVariation(1);
    }

    public void CaptureCurrentMetaRoom()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        roomScanData = ConvertMetaRoomToAppRoom(room);
    }

    public RoomData ConvertMetaRoomToAppRoom(MRUKRoom mrukRoom)
    {
        RoomData room = new RoomData();

        foreach (MRUKAnchor anchor in mrukRoom.Anchors)
        {
            FurnitureData furniture = new FurnitureData();


            FurnitureLabel translatedLabel;
            TryConvertEnumByName<MRUKAnchor.SceneLabels, FurnitureLabel>(anchor.Label, out translatedLabel);
            furniture.label = translatedLabel;
            furniture.tempLabelString = anchor.Label.ToString();
            furniture.posInRoom = anchor.transform.localPosition;
            furniture.rotInRoom = anchor.transform.localRotation;

            //TODO how to find out if plane or rect?
            if (anchor.PlaneRect != null)
            {
                furniture.type = FurnitureType.FloorAndWalls;
                furniture.planeRect = (Rect)anchor.PlaneRect;
            }
            else if (anchor.VolumeBounds != null)
            {

                furniture.type = FurnitureType.Furniture;
                furniture.volumeBounds = (Bounds)anchor.VolumeBounds;
            }


            room.furniture.Add(furniture);

        }

        return room;
    }


    public void ShowRoomScan()
    {
        if (currentVisualization != null)
        {
            Destroy(currentVisualization.gameObject); // maybe put this into seperate method later
        }

        currentVisualization = Instantiate(roomAnchorsVisualizationPrefab).GetComponent<RoomVisualization>();
        currentVisualization.PopulateFromSaveData(roomScanData);

        //auto hide variations

        // TODO enable edit visualized
        // enable currentScanVisualization
    }

    public void HideCurrentVisualization()
    {
        //...
    }

    public void SaveRoomScanFromVisualization()
    {
        // 1 update data based on changes made by player to the visualization
        roomScanData = currentVisualization.SaveChangesToNewRoomData();
        SaveRoomScan();
    }

    public void SaveRoomScan()
    {
        // 2 save changes to disk
        string savename = "roomScan.json";

        string jsonString = JsonUtility.ToJson(roomScanData);
        string fullPath = Path.Combine(Application.persistentDataPath, savename);

        File.WriteAllText(fullPath, jsonString);
    }

    public void ShowRoomVariation(int varId)
    {
        if(currentVisualization != null)
        {
            Destroy(currentVisualization.gameObject);
        }

        currentVisualization = Instantiate(roomVariationVisualizationPrefab).GetComponent<RoomVisualization>();
        Debug.Log("currentVisualization.PopulateFromSaveData(roomVariationsData);");
        Debug.Log($"roomVariationsData.furniture count: {roomVariationsData[varId].furniture.Count}");
        currentVisualization.PopulateFromSaveData(roomVariationsData[varId]);

        // check if any other variation is shown right now

        // show currentVariationVisualization 
    }

    public void CreateNewRoomVariation()
    {
        //todo
    }

    public void SaveRoomVariationFromVisualization(int varId)
    {
        roomVariationsData[varId] = currentVisualization.SaveChangesToNewRoomData();
        SaveRoomVariation(varId);

    }

    public void SaveRoomVariation(int varId)
    {
        // 1 update data based on changes made by player to the visualization

        // 2 save changes to disk
        string savename = $"roomVariation{varId}.json";

        string jsonString = JsonUtility.ToJson(roomVariationsData[varId]);
        string fullPath = Path.Combine(Application.persistentDataPath, savename);

        File.WriteAllText(fullPath, jsonString);
    }

    /// <summary>
    /// Reverts the room back to the original meta scan that is currently saved
    /// </summary>
    /// <param name="varId"></param>
    public void RevertRoomVariation(int varId)
    {
        roomVariationsData[varId] = roomScanData.DeepCopy();
        SaveRoomVariation(varId );
    }



    private bool TryConvertEnumByName<TSource, TTarget>(TSource sourceValue, out TTarget targetValue)
    where TSource : Enum
    where TTarget : struct, Enum
    {
        string name = sourceValue.ToString();
        return Enum.TryParse(name, out targetValue);
    }
}
