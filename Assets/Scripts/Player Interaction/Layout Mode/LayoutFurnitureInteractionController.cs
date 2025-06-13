using UnityEngine;

public class LayoutFurnitureInteractionController : FurnitureInteractionController
{
    enum LayoutPlayerControllerState
    {
        DefaultAllowMoveFurniture,
        SelectedAndEditingFurnitureData
    }

    LayoutPlayerControllerState state;

    protected override void Start()
    {
        base.Start();
        state = LayoutPlayerControllerState.DefaultAllowMoveFurniture;
    }

    protected override void Update()
    {
        base.Update();

        if(state == LayoutPlayerControllerState.DefaultAllowMoveFurniture)
        {

        }
    }
}
