using Meta.XR.MRUtilityKit;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class RoomsManager : MonoBehaviour
{
    // Manages the scanned room data and the various furniture rom variations.

    [SerializeField]
    LabelToModelConversionTable labelToMeshConversionTable;
    [Space]


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
            // TODO run this automatically
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

        // if no room variatin is available yet, create 2 new ones
        if (allRoomVariations.Length == 0 && roomScanData != null)
        {
            Create2ExampleRoomVariations();
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

        SaveRoomScan();

        // if no room variatin is available yet, create 2 new ones
        string path = Application.persistentDataPath;
        string[] allRoomVariations = Directory.GetFiles(path, "roomVariation*.json");

        if (allRoomVariations.Length == 0 && roomScanData != null)
        {
            Create2ExampleRoomVariations();
        }

        
    }


    public RoomData ConvertMetaRoomToAppRoom(MRUKRoom mrukRoom)
    {
        RoomData room = new RoomData();

        foreach (MRUKAnchor anchor in mrukRoom.Anchors)
        {
            if(anchor.transform.childCount == 0)
            {
                Debug.Log($"scipped anchor bounds for: {anchor.Label}");

                continue;
            }


            FurnitureData furniture = new FurnitureData();


            FurnitureLabel translatedLabel;
            TryConvertEnumByName<MRUKAnchor.SceneLabels, FurnitureLabel>(anchor.Label, out translatedLabel);
            furniture.label = translatedLabel;
            furniture.tempLabelString = anchor.Label.ToString();

            furniture.posInRoom = anchor.transform.localPosition;
            furniture.rotInRoom = anchor.transform.localRotation;

            // TODO Visualize the Walls aswell

            // meta objects mesh value is not set i dont know why, so we do it in this weird way
            furniture.meshData = new MeshSaveData(anchor.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh);

            if (anchor.VolumeBounds != null)
            {
                furniture.type = FurnitureType.Furniture;
                furniture.volumeBounds = (Bounds)anchor.VolumeBounds;
                Debug.Log($"[Bern] Global Mesh before was: {anchor.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh}");

               

                // For volumes we need to adjust the rotation as meta uses different axies
                // Flip y and z bounds

                ///Vector3 newSize = new Vector3(furniture.volumeBounds.size.x, furniture.volumeBounds.size.z, furniture.volumeBounds.size.y);
                ///furniture.volumeBounds.size = newSize;
                // Rotate it wierdly
                ///furniture.rotInRoom *= Quaternion.Euler(-90f, 0f, 0f) * Quaternion.Euler(0f, -180f, 0f) * Quaternion.Euler(0f, 0f, 180f);


                // All meta bounds somehow have their center right at the top .. meta be crazy lizard followers im telling you

                ///furniture.posInRoom.y -= furniture.volumeBounds.size.y;

                // except for the couch, it has its cetner in the middle, wtf meta
                ///if (furniture.label == FurnitureLabel.COUCH)
                ///{
                 ///   furniture.posInRoom.y += furniture.volumeBounds.size.y / 2;
                ///}

                ///if (anchor.PlaneRect != null)
                ///{
                    // sme volumes like tables or beds also have planes
                 ///   furniture.planeRect = (Rect)anchor.PlaneRect;
                ///}

            }
            else if (anchor.PlaneRect != null)
            {
                furniture.type = FurnitureType.FloorAndWalls;
                ///furniture.planeRect = (Rect)anchor.PlaneRect;
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
        currentVisualization.SetUpFromSaveData(roomScanData, labelToMeshConversionTable);

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
        if (currentVisualization != null)
        {
            Destroy(currentVisualization.gameObject);
        }

        currentVisualization = Instantiate(roomVariationVisualizationPrefab).GetComponent<RoomVisualization>();
        Debug.Log("currentVisualization.PopulateFromSaveData(roomVariationsData);");
        Debug.Log($"roomVariationsData.furniture count: {roomVariationsData[varId].furniture.Count}");
        currentVisualization.SetUpFromSaveData(roomVariationsData[varId], labelToMeshConversionTable);

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
        SaveRoomVariation(varId);
    }



    private bool TryConvertEnumByName<TSource, TTarget>(TSource sourceValue, out TTarget targetValue)
    where TSource : Enum
    where TTarget : struct, Enum
    {
        string name = sourceValue.ToString();
        return Enum.TryParse(name, out targetValue);
    }
}
