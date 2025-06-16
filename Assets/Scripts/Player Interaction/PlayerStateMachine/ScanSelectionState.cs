using UnityEngine;

public class ScanSelectionState : PlayerControllerInteractionState
{
    public override void OnStateEnter()
    {
        // set up hand menu
        refs.scanAddObjectsMenu.gameObject.SetActive(true);
        // Hookup to the menu callbacks state change to this menu?
        refs.scanAddObjectsMenu.OnAddFurnitureClickedCallback += OnMenuAddFurnitureClicked;
    }

    public override void OnStateExit()
    {
        runtimeData.hoveredOverFurniture?.OnHoverEnd();
        runtimeData.hoveredOverFurniture = null;

        // loose  hand menu
        refs.scanAddObjectsMenu.gameObject.SetActive(false);
        refs.scanAddObjectsMenu.OnAddFurnitureClickedCallback -= OnMenuAddFurnitureClicked;
    }

    public override void UpdateState()
    {
        HandleUiRay();
        if (!runtimeData.uiHasHit)
        {
            HandleFurnitureRay();
            HandleRayVisuals(runtimeData.furnitureRayEnd);
        }
        else
        {
            HandleRayVisuals(runtimeData.uiRayEnd);
        }

        

        HandleHoverInteractions();

        // If we press the select button on a furniture we select it and enter the scan edit state
        if (HandleFurnitureSelect())
        {
            sm.SetState(sm.scanEditDetails);
        }
    }

    void OnMenuAddFurnitureClicked()
    {
        Debug.Log($"[UI3] OnAddFurnitureClicked");

        sm.SetState(sm.scanRegisterNew);
    }
}