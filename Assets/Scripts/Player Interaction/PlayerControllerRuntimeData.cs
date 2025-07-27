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
}