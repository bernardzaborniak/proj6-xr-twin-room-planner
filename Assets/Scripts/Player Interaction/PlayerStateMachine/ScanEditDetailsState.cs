using UnityEngine;

public class ScanEditDetailsState : PlayerControllerInteractionState
{
    ScanModeFurniture selectedFurniture;


    public void SetCurrentFurniture(ScanModeFurniture furniture)
    {
        this.selectedFurniture = furniture;
        Debug.Log($"[Delete1] selectedFurniture.OnDeleteByUiClicked");
    }

    public override void OnStateEnter()
    {
        selectedFurniture.OnDeleteByUiClicked += OnDeletedInspectedFurniture;
    }

    public override void OnStateExit()
    {
        runtimeData.selectedFurniture?.OnDeselect();
        runtimeData.selectedFurniture = null;

        selectedFurniture.OnDeleteByUiClicked -= OnDeletedInspectedFurniture;

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

    void OnDeletedInspectedFurniture()
    {
        Debug.Log($"[Delete1] LayoutEditState - On deleted by UI inside base furniture");


        sm.SetState(sm.scanSelection);
    }
}