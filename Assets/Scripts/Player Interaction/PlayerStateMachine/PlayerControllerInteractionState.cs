using UnityEngine;

public abstract class PlayerControllerInteractionState
{
    protected PlayerControllerInteractionStateMachine sm;
    protected PlayerControllerReferences refs;
    protected PlayerControllerConfig config;
    protected PlayerControllerRuntimeData runtimeData;

    public enum RaycastType
    {
        OnlyHitUi,
        OnlyHitFurniture,
        HitBothPriorityOnUi,
    }

    public void Initialize(PlayerControllerInteractionStateMachine sm, PlayerControllerReferences refs, PlayerControllerConfig config, PlayerControllerRuntimeData runtimeData)
    {
        this.sm = sm;
        this.refs = refs;
        this.config = config;
        this.runtimeData = runtimeData;
    }


    public abstract void OnStateEnter();

    public abstract void OnStateExit();

    public abstract void UpdateState();


    // Utility Methods used by multiple states here

    protected void HandleRightHandRay(RaycastType castType)
    {
        if (castType == RaycastType.OnlyHitUi)
        {
            HandleUiRay();
        }
        else if (castType == RaycastType.OnlyHitFurniture)
        {
            HandleFurnitureRay();
        }
        else if (castType == RaycastType.HitBothPriorityOnUi)
        {
            HandleUiRay();

            if (!runtimeData.raycastWasSuccessfull)
            {
                HandleFurnitureRay();
            }
        }
    }

    void ResetRaycastDataForThisFrame()
    {
        runtimeData.raycastWasSuccessfull = false;
        runtimeData.raycastEnd = Vector3.zero ;
        runtimeData.uiHitByRay = null;
        runtimeData.furnitureHitByRay = null;
    }

    void HandleUiRay()
    {
        ResetRaycastDataForThisFrame();

        runtimeData.raycastEnd = refs.rayOrigin.position + refs.rayOrigin.forward * config.maxRaycastDistance;
        Ray ray = new Ray(refs.rayOrigin.position, refs.rayOrigin.forward);

        if (Physics.Raycast(ray, out runtimeData.raycastHitInfo, config.maxRaycastDistance, config.uiMask))
        {
            runtimeData.raycastEnd = runtimeData.raycastHitInfo.point;
            runtimeData.raycastWasSuccessfull = true;

            //Debug.Log($"[UI] Hit UI");

            runtimeData.uiHitByRay = runtimeData.raycastHitInfo.collider.gameObject.GetComponent<UiCustomButton>();
            //Debug.Log($"[UI] runtimeData.uiHitByRa: {runtimeData.uiHitByRay}");
            runtimeData.raycastHitType = PlayerControllerRuntimeData.RaycastResultType.HitUi;

            // todo move this to Handle Ui Interaction
        }
        else
        {
            runtimeData.raycastWasSuccessfull = false;
        }
    }

    public void HandleUiInteraction()
    {
        if (OVRInput.GetDown(config.pressUiButton) && runtimeData.uiHitByRay != null)
        {
            runtimeData.uiHitByRay.OnClick();
        }
    }

    void HandleFurnitureRay()
    {
        ResetRaycastDataForThisFrame();

        runtimeData.raycastEnd = refs.rayOrigin.position + refs.rayOrigin.forward * config.maxRaycastDistance;
        Ray ray = new Ray(refs.rayOrigin.position, refs.rayOrigin.forward);

        if (Physics.Raycast(ray, out runtimeData.raycastHitInfo, config.maxRaycastDistance, config.furnitureMask))
        {
            runtimeData.raycastEnd = runtimeData.raycastHitInfo.point;
            runtimeData.raycastWasSuccessfull = true;

            runtimeData.furnitureHitByRay = runtimeData.raycastHitInfo.collider.gameObject.GetComponent<BaseFurniture>();
            runtimeData.raycastHitType = PlayerControllerRuntimeData.RaycastResultType.HitFurniture;
        }
        else
        {
            runtimeData.raycastWasSuccessfull = false;
        }
    }

    public void HandleRayVisuals()
    {
        // UI has priority over furniture

        refs.lineRenderer.SetPosition(0, refs.rayOrigin.position);
        refs.lineRenderer.SetPosition(1, runtimeData.raycastEnd);
    }

    protected void HandleHoverOverFurniture()
    {
        if (runtimeData.furnitureHitByRay != null && runtimeData.furnitureHitByRay.Interactable)
        {
            if (runtimeData.furnitureHitByRay != runtimeData.selectedFurniture && runtimeData.furnitureHitByRay != runtimeData.hoveredOverFurniture)
            {
                if (runtimeData.hoveredOverFurniture != null)
                {
                    // stop previus hover
                    runtimeData.hoveredOverFurniture.OnHoverEnd();
                }

                runtimeData.hoveredOverFurniture = runtimeData.furnitureHitByRay;
                runtimeData.hoveredOverFurniture.OnHoverStart();
            }
        }
        else
        {
            // stop previus hover
            runtimeData.hoveredOverFurniture?.OnHoverEnd();
            runtimeData.hoveredOverFurniture = null;
        }
    }

    /// <summary>
    /// Return whether new furniture was selected
    /// </summary>
    /// <returns></returns>
    protected bool HandleFurnitureSelect()
    {
        if (OVRInput.GetDown(config.selectFurnitureButton) && runtimeData.hoveredOverFurniture != null)
        {
            if (runtimeData.selectedFurniture != null)
            {
                runtimeData.selectedFurniture.OnDeselect();
            }

            runtimeData.selectedFurniture = runtimeData.hoveredOverFurniture;
            //hoveredOverFurniture = null;
            runtimeData.selectedFurniture.OnSelect(refs.rayOrigin.forward);
            return true;
        }

        return false;
    }

}
