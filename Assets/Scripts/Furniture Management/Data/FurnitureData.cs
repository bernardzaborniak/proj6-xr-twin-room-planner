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

    // used to correct the y rotation of furniture pieces
    public float rotationAdjuster;

    // Bounds & visualization
    //public Mesh mesh; // maybe we actually dont need the mesh?  we can just generate it at runtime
    public MeshSaveData meshData;

    public Rect planeRect;
    public Bounds volumeBounds;


    public FurnitureData DeepCopy()
    {
        return new FurnitureData
        {
            type = this.type,
            label = this.label,
            tempLabelString = this.tempLabelString,

            posInRoom = this.posInRoom,
            rotInRoom = this.rotInRoom,

            rotationAdjuster = this.rotationAdjuster,

            meshData = this.meshData, // Only copies the reference. Consider cloning if needed.
            planeRect = this.planeRect,
            volumeBounds = this.volumeBounds,
        };
    }
}
