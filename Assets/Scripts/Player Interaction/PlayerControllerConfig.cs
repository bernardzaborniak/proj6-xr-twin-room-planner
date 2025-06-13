using UnityEngine;

[System.Serializable]
public class PlayerControllerConfig
{
    [Header("Input Settings")]
    public OVRInput.RawButton switchRoomModeButton;

    public OVRInput.RawButton selectFurnitureButton;
    public OVRInput.RawButton deselectFurnitureButton;
    public OVRInput.RawButton pressUiButton;

    public OVRInput.RawButton moveFurnitureHoldButton;
    public OVRInput.Axis2D moveFurnitureUpButton;
    public OVRInput.Axis2D rotateFurnitureButton;

    [Header("Raycasts")]
    public LayerMask furnitureMask;
    public LayerMask uiMask;
    public float maxRaycastDistance;

    [Header("Move Furniture")]
    public float heightChangeSpeed;
    public float rotationChangeSpeed;


}