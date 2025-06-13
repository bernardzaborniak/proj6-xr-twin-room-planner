public class ScanEditState : PlayerControllerInteractionState
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
        HandleUiRay();
        HandleRayVisuals(runtimeData.uiRayEnd);

        // If we press the select button again, exit the scan edit state
        if (OVRInput.GetDown(config.deselectFurnitureButton))
        {
            sm.SetState(sm.scanSelection);
        }

    }
}