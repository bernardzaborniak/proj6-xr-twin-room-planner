using System.Collections.Generic;

[System.Serializable]
public class RoomData
{
    public string roomName;

    public List<FurnitureData> furniture;

    public RoomData() 
    {
        furniture = new List<FurnitureData>();
    }

    public RoomData DeepCopy()
    {
        RoomData copy = new RoomData();
        copy.roomName = this.roomName;

        foreach (var item in this.furniture)
        {
            copy.furniture.Add(item.DeepCopy());
        }

        return copy;
    }
}