using UnityEngine;

[System.Serializable]
public class FurnitureData
{
    // non mono behaviour model, to allow serialization later on

    public FurnitureType type;
    public FurnitureLabel label;
    public string tempLabelString;

    /// <summary>
    /// Walls and Floors are not movable, but most furniture is
    /// </summary>
    //public bool movable;    // todo abstract in method that checks if furniture type is plane

    // Positioning
    public Vector3 posInRoom;
    public Quaternion rotInRoom;

    // Bounds & visualization
    //public Mesh mesh; // maybe we actually dont need the mesh?  we can just generate it at runtime
    public MeshSaveData meshData;


    public Rect planeRect;
    public Bounds volumeBounds;

    public float newTestFloat = 4.33f;

    public FurnitureData DeepCopy()
    {
        return new FurnitureData
        {
            type = this.type,
            label = this.label,
            tempLabelString = this.tempLabelString,

            posInRoom = this.posInRoom,
            rotInRoom = this.rotInRoom,

            meshData = this.meshData, // Only copies the reference. Consider cloning if needed.
            planeRect = this.planeRect,
            volumeBounds = this.volumeBounds,
            newTestFloat = this.newTestFloat
        };
    }
}
