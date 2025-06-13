using UnityEngine;

[System.Serializable]
public class PlayerControllerRuntimeData
{
    public BaseFurniture hoveredOverFurniture;
    public BaseFurniture selectedFurniture;

    // Raycasts
    public bool furnitureHasHit;
    public RaycastHit furnitureHit;
    public Vector3 furnitureRayEnd;

    public bool uiHasHit;
    public RaycastHit uiHit;
    public Vector3 uiRayEnd;

    // Move furniture

}