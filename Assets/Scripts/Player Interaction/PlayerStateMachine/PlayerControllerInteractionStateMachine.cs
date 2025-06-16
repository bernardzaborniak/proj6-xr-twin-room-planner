using UnityEngine;

[System.Serializable]
public class PlayerControllerInteractionStateMachine 
{
    // All states
    public ScanSelectionState scanSelection = new ScanSelectionState();
    public ScanEditDetailsState scanEditDetails = new ScanEditDetailsState();
    public ScanRegisterNewFurnitureState scanRegisterNew = new ScanRegisterNewFurnitureState();

    public LayoutSelectionAndMoveState layoutSelectionAndMove = new LayoutSelectionAndMoveState();
    public LayoutEditState layoutEdit = new LayoutEditState();
   

    public PlayerControllerInteractionState CurrentInteractionState { get; protected set; }

    public string debugCurrentStateName;

    public PlayerControllerInteractionStateMachine(PlayerControllerReferences refs, PlayerControllerConfig config, PlayerControllerRuntimeData runtimeData)
    {
        scanSelection.Initialize(this, refs, config, runtimeData);
        scanEditDetails.Initialize(this, refs, config, runtimeData);
        scanRegisterNew.Initialize(this, refs, config, runtimeData);

        layoutSelectionAndMove.Initialize(this, refs, config, runtimeData);
        layoutEdit.Initialize(this, refs, config, runtimeData);
       

        SetState(scanSelection);
    }

    public void SetState(PlayerControllerInteractionState newState)
    {
        if (CurrentInteractionState == newState)
            return;

        CurrentInteractionState?.OnStateExit();
        CurrentInteractionState = newState;
        debugCurrentStateName = newState.ToString();
        CurrentInteractionState?.OnStateEnter();
    }

    public void Update()
    {
        CurrentInteractionState?.UpdateState();
    }
}
