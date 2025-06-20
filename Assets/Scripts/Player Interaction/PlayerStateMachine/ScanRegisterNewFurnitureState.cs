using UnityEngine;

public class ScanRegisterNewFurnitureState : PlayerControllerInteractionState
{
    // tis state is similar to what meta has, it allows to create new bounding boxes with our controllers

    //maybe have a small menu on the left open that allows us to switch between create new furniture and add walls mode 
    // add walls is disabled in one test instance?

    enum CreateBoxState
    {
        FirstPointGround,
        SecondPointGround,
        ThirdPointPerpendicular,
        FourthPointHeight
    }

    CreateBoxState createBoxState;

    Color lineRendererColorBefore;

    public override void OnStateEnter()
    {
        createBoxState = CreateBoxState.FirstPointGround;

        // set up hand menu
        refs.scanAddObjectsMenu.gameObject.SetActive(true);
        refs.scanAddObjectsMenu.OnCancelClickedCallback += OnCancelAddBoxClicked;

        // change color of line renderer
        lineRendererColorBefore = refs.lineRenderer.startColor;

        refs.lineRenderer.startColor = config.addNewBoxScanSelectionLineColor;
        refs.lineRenderer.endColor = config.addNewBoxScanSelectionLineColor;

       

    }

    public override void OnStateExit()
    {
        // deregister  hand menu
        refs.scanAddObjectsMenu.gameObject.SetActive(false);
        refs.scanAddObjectsMenu.OnCancelClickedCallback -= OnCancelAddBoxClicked;


        refs.lineRenderer.startColor = lineRendererColorBefore;
        refs.lineRenderer.endColor = lineRendererColorBefore;
    }

    public override void UpdateState()
    {
        HandleRightHandRay(RaycastType.OnlyHitUi);
        HandleRayVisuals();

        HandleUiInteraction();

        if (OVRInput.GetDown(config.generalReturnButton))
        {
            sm.SetState(sm.scanSelection);
        }
    }

    void OnCancelAddBoxClicked()
    {
        sm.SetState(sm.scanSelection);
    }
}