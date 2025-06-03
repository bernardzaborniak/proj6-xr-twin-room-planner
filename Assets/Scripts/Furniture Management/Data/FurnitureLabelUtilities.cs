using UnityEngine;

/// <summary>
/// Helper class for stuff surrounding our Furniture Labels
/// </summary>
public static class FurnitureLabelUtilities 
{
    public static bool IsLabelFlatWall(FurnitureLabel label)
    {
        return (
            label == FurnitureLabel.FLOOR ||
            label == FurnitureLabel.CEILING ||
            label == FurnitureLabel.WALL_FACE ||
            label == FurnitureLabel.WINDOW_FRAME ||
            label == FurnitureLabel.DOOR_FRAME ||
            label == FurnitureLabel.GLOBAL_MESH
            );
    }
}
