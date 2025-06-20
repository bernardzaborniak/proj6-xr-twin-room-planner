
using UnityEngine;

[System.Serializable]
public class PlayerControllerReferences
{
    public SpawnObjectMenu spawnObjectMenu;
    public CreateNewBoundingBoxScanMenu scanAddObjectsMenu;

    [Header("Raycast")]
    public Transform rayOrigin;
    public LineRenderer lineRenderer;

    [Header("Wall placement")]
    public GameObject placeWallPrefab;
    public Material wallMaterial;
    public Material wallPreviewMaterial;
}