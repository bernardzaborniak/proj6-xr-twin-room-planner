using UnityEngine;
using System.Collections.Generic;

public class RoomVariationVisualization : RoomVisualization
{
    [SerializeField] GameObject furnitureVisualizationPrefab;

    // the int key is the index of the furniture in the original data object
    List<FurnitureVisualization> furnitureVisualizations = new List<FurnitureVisualization> ();
    string currentRoomName;

    // todo maybe later actually save a backup of roomData here and keep some kind of dictionary to see which
    // objects were there before and had updatd positions or labels and which were added extra.

    public override void PopulateFromSaveData(RoomData roomData)
    {
        currentRoomName = roomData.roomName;

        for (int i = 0; i < roomData.furniture.Count; i++)
        {
            // spawn object based on save data
            FurnitureVisualization newFurniture = Instantiate(furnitureVisualizationPrefab, this.transform).GetComponent<FurnitureVisualization>();
            newFurniture.VisualizeFromData(roomData.furniture[i]);
            furnitureVisualizations.Add(newFurniture);
        }
    }

    public override RoomData SaveChangesToNewRoomData()
    {
        RoomData newData = new RoomData();
        newData.roomName = currentRoomName;

        foreach (FurnitureVisualization furniture in furnitureVisualizations)
        {
            newData.furniture.Add(furniture.ConvertToFurnitureDataObject());
        }

        return newData;
    }
}