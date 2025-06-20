public class ScanEditDetailsState : PlayerControllerInteractionState
{
    public override void OnStateEnter()
    {

    }

    public override void OnStateExit()
    {
        runtimeData.selectedFurniture?.OnDeselect();
        runtimeData.selectedFurniture = null;
    }

    public override void UpdateState()
    {
        HandleRightHandRay(RaycastType.OnlyHitUi);
        HandleRayVisuals();

        HandleUiInteraction();

        // If we press the deselect button again, exit the scan edit state
        if (OVRInput.GetDown(config.deselectFurnitureButton))
        {
            sm.SetState(sm.scanSelection);
        }

    }
}