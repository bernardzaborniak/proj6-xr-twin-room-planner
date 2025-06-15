using UnityEngine;

public abstract class PlayerControllerInteractionState 
{
    protected PlayerControllerInteractionStateMachine sm;
    protected PlayerControllerReferences refs;
    protected PlayerControllerConfig config;
    protected PlayerControllerRuntimeData runtimeData;

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

    protected void HandleUiRay()
    {
        runtimeData.uiRayEnd = refs.rayOrigin.position + refs.rayOrigin.forward * config.maxRaycastDistance;
        Ray ray = new Ray(refs.rayOrigin.position, refs.rayOrigin.forward);

        if (Physics.Raycast(ray, out runtimeData.uiHit, config.maxRaycastDistance, config.uiMask))
        {
            runtimeData.uiRayEnd = runtimeData.uiHit.point;
            runtimeData.uiHasHit = true;

            UiCustomButton uiButton = runtimeData.uiHit.collider.gameObject.GetComponent<UiCustomButton>();

            if (OVRInput.GetDown(config.pressUiButton) && uiButton != null)
            {
                Debug.Log($"[Label UI] OVRInput.GetDown(config.pressUiButton) in frame: {Time.frameCount}");

                uiButton.OnClick();
            }
        }
        else
        {
            runtimeData.uiHasHit = false;
        }
    }

    protected void HandleFurnitureRay()
    {
        runtimeData.furnitureRayEnd = refs.rayOrigin.position + refs.rayOrigin.forward * config.maxRaycastDistance;
        Ray ray = new Ray(refs.rayOrigin.position, refs.rayOrigin.forward);
        if (Physics.Raycast(ray, out runtimeData.furnitureHit, config.maxRaycastDistance, config.furnitureMask))
        {
            runtimeData.furnitureHasHit = true;
            runtimeData.furnitureRayEnd = runtimeData.furnitureHit.point;

        }
        else
        {
            runtimeData.furnitureHasHit = false;
        }
    }

    protected void HandleRayVisuals(Vector3 rayEnd)
    {
        // UI has priority over furniture

        refs.lineRenderer.SetPosition(0, refs.rayOrigin.position);
        refs.lineRenderer.SetPosition(1, rayEnd); 
    }

    protected void HandleHoverInteractions()
    {
        BaseFurniture newHoveredFurniture = null;
        if (runtimeData.furnitureHasHit) newHoveredFurniture = runtimeData.furnitureHit.transform.GetComponent<BaseFurniture>();

        if (newHoveredFurniture != null && newHoveredFurniture.Interactable)
        {
            if (newHoveredFurniture != runtimeData.selectedFurniture && newHoveredFurniture != runtimeData.hoveredOverFurniture)
            {
                if (runtimeData.hoveredOverFurniture != null)
                {
                    // stop previus hover
                    runtimeData.hoveredOverFurniture.OnHoverEnd();
                }

                runtimeData.hoveredOverFurniture = newHoveredFurniture;
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
