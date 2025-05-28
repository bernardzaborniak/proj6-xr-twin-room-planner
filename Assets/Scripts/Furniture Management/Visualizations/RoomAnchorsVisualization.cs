using System;
using Unity.VisualScripting;
using UnityEngine;

public class RoomAnchorsVisualization : RoomVisualization
{
    // This is the scanned room from meta, use this object to allow the player to manually edit it

    // spawn all the bounding boxes with allowing to move them and edit etc...

    //RoomData scannedRoomData;
    AnchorVisualization allFurniture;

    [SerializeField] GameObject furnitureScanVisualizationPrefab;

    public override void PopulateFromSaveData(RoomData roomData)
    {
        // spawn object based on save data

        Instantiate(furnitureScanVisualizationPrefab,transform.position, Quaternion.identity, this.transform);
    }

    public override RoomData SaveChangesToNewRoomData()
    {
        return null;
    }

}