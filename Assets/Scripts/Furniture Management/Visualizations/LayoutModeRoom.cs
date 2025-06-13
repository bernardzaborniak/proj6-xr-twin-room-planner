using UnityEngine;
using System.Collections.Generic;


public class LayoutModeRoom : RoomVisualization
{

    LabelToModelConversionTable labelToModelConversionTable;
    [SerializeField] GameObject furnitureVisualizationPrefab;

    List<LayoutModeFurniture> furnitureVisualizations = new List<LayoutModeFurniture> ();
    string currentRoomName;


    public override void SetUpFromSaveData(RoomData roomData, LabelToModelConversionTable labelToModelConversionTable)
    {
        currentRoomName = roomData.roomName;
        this.labelToModelConversionTable = labelToModelConversionTable;

        for (int i = 0; i < roomData.furniture.Count; i++)
        {
            // spawn object based on save data
            LayoutModeFurniture newFurniture = Instantiate(furnitureVisualizationPrefab, this.transform).GetComponent<LayoutModeFurniture>();
            newFurniture.gameObject.name = i.ToString() + ": " + roomData.furniture[i].label;
            newFurniture.VisualizeFromData(roomData.furniture[i], labelToModelConversionTable);
            furnitureVisualizations.Add(newFurniture);
        }
    }

    public override RoomData SaveChangesToNewRoomData()
    {
        RoomData newData = new RoomData();
        newData.roomName = currentRoomName;

        foreach (LayoutModeFurniture furniture in furnitureVisualizations)
        {
            newData.furniture.Add(furniture.ConvertToFurnitureDataObject());
        }

        return newData;
    }

    public void AddFurnitureFromCatalogue(FurnitureData data, Vector3 worldspacePos)
    {
        data.posInRoom = transform.InverseTransformPoint(worldspacePos);

        LayoutModeFurniture newFurniture = Instantiate(furnitureVisualizationPrefab, this.transform).GetComponent<LayoutModeFurniture>();
        newFurniture.VisualizeFromData(data, labelToModelConversionTable);
        furnitureVisualizations.Add(newFurniture);
    }
}