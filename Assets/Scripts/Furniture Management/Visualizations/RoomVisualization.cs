using UnityEngine;

public abstract class RoomVisualization : MonoBehaviour
{
    // LabelToModelConversionTable may be later abstracted into a singleton
    public abstract void SetUpFromSaveData(RoomData roomData, LabelToModelConversionTable labelToMeshConversionTable);


    public abstract RoomData SaveChangesToNewRoomData();
}