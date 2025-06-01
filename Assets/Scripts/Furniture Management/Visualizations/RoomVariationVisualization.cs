using UnityEngine;
using System.Collections.Generic;

public class RoomVariationVisualization : RoomVisualization
{
    [SerializeField] GameObject furnitureVisualizationPrefab;

    List<FurnitureVisualization> furnitureVisualizations = new List<FurnitureVisualization> ();
    string currentRoomName;


    public override void SetUpFromSaveData(RoomData roomData, LabelToModelConversionTable labelToMeshConversionTable)
    {
        currentRoomName = roomData.roomName;

        for (int i = 0; i < roomData.furniture.Count; i++)
        {
            // spawn object based on save data
            FurnitureVisualization newFurniture = Instantiate(furnitureVisualizationPrefab, this.transform).GetComponent<FurnitureVisualization>();
            newFurniture.VisualizeFromData(roomData.furniture[i], labelToMeshConversionTable);
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

    public void AddFurnitureFromCatalogue(FurnitureData data)
    {
        //TODO
        //furnitureVisualizations.Add(spanwed object);
    }
}