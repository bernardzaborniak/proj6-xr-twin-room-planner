public class ScanSelectionState : PlayerControllerInteractionState
{
    public override void OnStateEnter()
    {
        
    }

    public override void OnStateExit()
    {
        runtimeData.hoveredOverFurniture?.OnHoverEnd();
        runtimeData.hoveredOverFurniture = null;
    }

    public override void UpdateState()
    {
        HandleFurnitureRay();
        HandleRayVisuals(runtimeData.furnitureRayEnd);

        HandleHoverInteractions();

        // If we press the select button on a furniture we select it and enter the scan edit state
        if (HandleFurnitureSelect())
        {
            sm.SetState(sm.scanEdit);
        }
    }
}