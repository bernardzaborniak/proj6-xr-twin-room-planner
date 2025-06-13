using UnityEngine;

public class LayoutSelectionAndMoveState : PlayerControllerInteractionState
{
    bool isInteractingWithObject;
    GameObject currentInteractingObject;
    Plane currentInteractionPlane;
    Vector3 furnitureToRayOffset;
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
    }

    public override void UpdateState()
    {
        HandleFurnitureRay();
        HandleRayVisuals(runtimeData.furnitureRayEnd);

        HandleHoverInteractions();

        // TODO implement move inside this state
        // HandleMoveFurniture();

        // If we press the select button on a furniture we select it and enter the scan edit state
        if (HandleFurnitureSelect())
        {
            sm.SetState(sm.layoutEdit);
            return;
        }

        HandleFurnitureMove();
    }

    void HandleFurnitureMove()
    {
        Ray ray = new Ray(refs.rayOrigin.position, refs.rayOrigin.forward);


        if (OVRInput.GetDown(config.moveFurnitureHoldButton))
        {
            if (runtimeData.hoveredOverFurniture != null)
            {
                isInteractingWithObject = true;
                currentInteractingObject = runtimeData.hoveredOverFurniture.gameObject;
                heightOffset = 0;
                currentInteractionPlane = new Plane(Vector3.up, currentInteractingObject.transform.position);
                float t = 0;
                currentInteractionPlane.Raycast(ray, out t);
                furnitureToRayOffset = currentInteractingObject.transform.position - (refs.rayOrigin.position + refs.rayOrigin.forward * t);
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
        Vector3 newpos = refs.rayOrigin.position + refs.rayOrigin.forward * t;
        currentInteractingObject.transform.position = newpos + furnitureToRayOffset + Vector3.up * heightOffset;
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