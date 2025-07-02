using UnityEngine;
using static UnityEngine.Rendering.ProbeAdjustmentVolume;

public class LayoutSelectionAndMoveState : PlayerControllerInteractionState
{
    bool isInteractingWithObject;
    GameObject currentInteractingObject;
    Plane currentInteractionPlane;
    Vector3 furnitureToRayOffset;
    Vector3 startMoveFurnitureRaycastPoint;
    float heightOffset;

    public override void OnStateEnter()
    {
        // place object menu - enable
        refs.spawnObjectMenu.gameObject.SetActive(true);
    }

    public override void OnStateExit()
    {
        runtimeData.hoveredOverFurniture?.OnHoverEnd();
        runtimeData.hoveredOverFurniture = null;

        refs.spawnObjectMenu.gameObject.SetActive(false);
        // place object menu - set false

        refs.inGameMenu.HideMenu();
    }

    public override void UpdateState()
    {
        HandleRightHandRay(RaycastType.HitBothPriorityOnUi);
        HandleRayVisuals();

        HandleUiInteraction();
        HandleHoverOverFurniture();

        // TODO implement move inside this state
        // HandleMoveFurniture();

        // If we press the select button on a furniture we select it and enter the scan edit state
        if (HandleFurnitureSelect())
        {
            sm.layoutEdit.SetCurrentFurniture(runtimeData.selectedFurniture as LayoutModeFurniture);
            sm.SetState(sm.layoutEdit);
            return;
        }

        HandleFurnitureMove();

        HandleInGameMenuEnableByButton();
    }

    void HandleFurnitureMove()
    {
        Ray ray = new Ray(refs.rayOrigin.position, refs.rayOrigin.forward);


        if (OVRInput.GetDown(config.moveFurnitureHoldButton))
        {
            if (runtimeData.hoveredOverFurniture != null)
            {
                EventLogger.Instance?.LogInteraction("Object moved.", EventLogger.actionTypes.ObjectMoved);
                isInteractingWithObject = true;
                currentInteractingObject = runtimeData.hoveredOverFurniture.gameObject;
                heightOffset = 0;
                currentInteractionPlane = new Plane(Vector3.up, currentInteractingObject.transform.position);
                float t = 0;
                currentInteractionPlane.Raycast(ray, out t);
                startMoveFurnitureRaycastPoint = (refs.rayOrigin.position + refs.rayOrigin.forward * t);
                furnitureToRayOffset = currentInteractingObject.transform.position - startMoveFurnitureRaycastPoint;
            }
        }

        if (OVRInput.GetUp(config.moveFurnitureHoldButton))
        {
            isInteractingWithObject = false;
            currentInteractingObject = null;

        }

        if (isInteractingWithObject)
        {
            MoveSelectedObject(ray);
            ChangeObjectHeight();
            ChangeObjectRotation();
        }
    }

    void MoveSelectedObject(Ray ray)
    {
        float t = 0;
        currentInteractionPlane.Raycast(ray, out t);
        if (t < 0) t = 0;
        Vector3 newPos = refs.rayOrigin.position + refs.rayOrigin.forward * t;
        Vector3 oldPosToNewPose = newPos - startMoveFurnitureRaycastPoint;

        // multiply by square root to slow down movement a bit
        if (oldPosToNewPose.magnitude > 1)
        {
            oldPosToNewPose = oldPosToNewPose.normalized * Mathf.Sqrt(oldPosToNewPose.magnitude);
        }

        newPos = startMoveFurnitureRaycastPoint + oldPosToNewPose;

        currentInteractingObject.transform.position = newPos + furnitureToRayOffset + Vector3.up * heightOffset;
    }


    void ChangeObjectHeight()
    {
        heightOffset += OVRInput.Get(config.moveFurnitureUpButton).y * config.heightChangeSpeed;

    }

    void ChangeObjectRotation()
    {
        Debug.Log($"do rotation {OVRInput.Get(config.rotateFurnitureButton).x}");

        currentInteractingObject.transform.Rotate(0f, OVRInput.Get(config.rotateFurnitureButton).x * config.rotationChangeSpeed, 0f);
    }
}