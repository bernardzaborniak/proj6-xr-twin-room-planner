using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomAnchorsVisualization : RoomVisualization
{
    [SerializeField] GameObject furnitureScanVisualizationPrefab;

    List<AnchorVisualization> anchorVisualizations = new List<AnchorVisualization>();
    string currentRoomName;

    public override void PopulateFromSaveData(RoomData roomData)
    {
        currentRoomName = roomData.roomName;

        for (int i = 0; i < roomData.furniture.Count; i++)
        {
            // spawn object based on save data
            AnchorVisualization newFurniture = Instantiate(furnitureScanVisualizationPrefab, this.transform).GetComponent<AnchorVisualization>();
            newFurniture.VisualizeFromData(roomData.furniture[i]);
            anchorVisualizations.Add(newFurniture);
        }
    }

    public override RoomData SaveChangesToNewRoomData()
    {
        RoomData newData = new RoomData();
        newData.roomName = currentRoomName;

        foreach (AnchorVisualization furniture in anchorVisualizations)
        {
            newData.furniture.Add(furniture.ConvertToFurnitureDataObject());
        }

        return newData;
    }

}