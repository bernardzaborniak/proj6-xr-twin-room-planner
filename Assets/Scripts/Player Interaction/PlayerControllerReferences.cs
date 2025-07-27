
using UnityEngine;

[System.Serializable]
public class PlayerControllerReferences
{
    public SpawnObjectMenu spawnObjectMenu;
    public CreateNewBoundingBoxScanMenu scanAddObjectsMenu;
    public RoomsManager roomManager;
    public InGameMenu inGameMenu;
    public Transform playerCameraTransform;



    [Header("Raycast")]
    public Transform rayOrigin;
    public LineRenderer lineRenderer;

    [Header("Wall placement")]
    public GameObject placeWallPrefab;
    public Material wallMaterial;
    public Material wallPreviewMaterial;

    [Header("Create new bounding Box")]
    public GameObject createNewBoundingBoxVisualizationPrefab;
}