using UnityEngine;

public abstract class RoomVisualization : MonoBehaviour
{
    public abstract void PopulateFromSaveData(RoomData roomData);


    public abstract RoomData SaveChangesToNewRoomData();
}