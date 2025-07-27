using UnityEngine;

public abstract class RoomVisualization : MonoBehaviour
{
    public abstract void SetUpFromSaveData(RoomData roomData, LabelToModelConversionTable labelToMeshConversionTable);


    public abstract RoomData SaveChangesToNewRoomData();
}