using UnityEngine;

[System.Serializable]
public class PlayerControllerRuntimeData
{
    public BaseFurniture hoveredOverFurniture;
    public BaseFurniture selectedFurniture;

    // Raycasts
    

    public enum RaycastResultType
    {
        HitUi,
        HitFurniture
    }


    public bool raycastWasSuccessfull;
    public RaycastHit raycastHitInfo;
    public RaycastResultType raycastHitType;
    public Vector3 raycastEnd;

    public UiCustomButton uiHitByRay;
    public BaseFurniture furnitureHitByRay;

    /*
    public bool furnitureHasHit;
    public RaycastHit furnitureHit;
    public Vector3 furnitureRayEnd;

    public bool uiHasHit;
    public RaycastHit uiHit;
    public Vector3 uiRayEnd;
    */

    // Move furniture

}