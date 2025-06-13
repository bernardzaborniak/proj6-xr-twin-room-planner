using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScanModeRoom : RoomVisualization
{
    [SerializeField] GameObject furnitureScanVisualizationPrefab;

    List<ScanModeFurniture> anchorVisualizations = new List<ScanModeFurniture>();
    string currentRoomName;

    public override void SetUpFromSaveData(RoomData roomData, LabelToModelConversionTable labelToMeshConversionTable)
    {
        currentRoomName = roomData.roomName;

        for (int i = 0; i < roomData.furniture.Count; i++)
        {
            // spawn object based on save data
            ScanModeFurniture newFurniture = Instantiate(furnitureScanVisualizationPrefab, this.transform).GetComponent<ScanModeFurniture>();
            newFurniture.gameObject.name = i.ToString() + ": " + roomData.furniture[i].label;
            newFurniture.VisualizeFromData(roomData.furniture[i]);
            anchorVisualizations.Add(newFurniture);
        }
    }

    public override RoomData SaveChangesToNewRoomData()
    {
        RoomData newData = new RoomData();
        newData.roomName = currentRoomName;

        foreach (ScanModeFurniture furniture in anchorVisualizations)
        {
            newData.furniture.Add(furniture.ConvertToFurnitureDataObject());
        }

        return newData;
    }

}