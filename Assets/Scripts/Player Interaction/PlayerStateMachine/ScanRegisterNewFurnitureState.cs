using UnityEngine;

public class ScanRegisterNewFurnitureState : PlayerControllerInteractionState
{
    // tis state is similar to what meta has, it allows to create new bounding boxes with our controllers

    //maybe have a small menu on the left open that allows us to switch between create new furniture and add walls mode 
    // add walls is disabled in one test instance?

    CreateBoundingBoxVisualisation createBoxVisualization;

    Vector3 boxPoint1;
    Vector3 boxPoint2;
    Vector3 boxPoint3;
    Vector3 boxPoint4;

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

        createBoxVisualization = GameObject.Instantiate(refs.createNewBoundingBoxVisualizationPrefab).GetComponent<CreateBoundingBoxVisualisation>();

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
        createBoxVisualization.currentTargetSphere.gameObject.SetActive(false);

        HandleRightHandRay(RaycastType.OnlyHitUi);
        HandleRayVisuals();

        HandleUiInteraction();

        HandlePlaceBoundingBoxPoint();

        if (OVRInput.GetDown(config.generalReturnButton))
        {
            sm.SetState(sm.scanSelection);
        }
    }

    void OnCancelAddBoxClicked()
    {
        sm.SetState(sm.scanSelection);
    }


    /// <summary>
    /// Here the main logic happens
    /// </summary>
    void HandlePlaceBoundingBoxPoint()
    {
        if (runtimeData.furnitureHitByRay != null)
        {
            return;
        }

        // Do Rayast first
        Ray ray = new Ray(refs.rayOrigin.transform.position, refs.rayOrigin.transform.forward);
        // RaycastHit hit;

        runtimeData.raycastEnd = refs.rayOrigin.position + refs.rayOrigin.forward * config.maxRaycastDistance;
        runtimeData.raycastWasSuccessfull = Physics.Raycast(ray, out runtimeData.raycastHitInfo, config.maxRaycastDistance, config.createNewBoundingBoxUniversalGround);

        if (runtimeData.raycastWasSuccessfull)
        {
            runtimeData.raycastEnd = runtimeData.raycastHitInfo.point;

            createBoxVisualization.currentTargetSphere.gameObject.SetActive(true);
            createBoxVisualization.currentTargetSphere.transform.position = runtimeData.raycastEnd;

            switch (createBoxState)
            {
                case CreateBoxState.FirstPointGround:
                    {
                        if (OVRInput.GetDown(config.createBoundingBoxPointButton))
                        {
                            createBoxState = CreateBoxState.SecondPointGround;
                            boxPoint1 = runtimeData.raycastEnd;
                        }

                        

                        createBoxVisualization.lineRenderer.SetPositions(new Vector3[] { runtimeData.raycastEnd });
                    }
                    break;
                case CreateBoxState.SecondPointGround:
                    {
                        if (OVRInput.GetDown(config.createBoundingBoxPointButton))
                        {
                            createBoxState = CreateBoxState.ThirdPointPerpendicular;
                            boxPoint2 = runtimeData.raycastEnd;
                        }

                        createBoxVisualization.lineRenderer.SetPositions(new Vector3[] { boxPoint1, runtimeData.raycastEnd });
                    }
                    break;
                case CreateBoxState.ThirdPointPerpendicular:
                    {
                        // TODO only allow to place it perpendicular

                        if (OVRInput.GetDown(config.createBoundingBoxPointButton))
                        {
                            createBoxState = CreateBoxState.FourthPointHeight;
                            boxPoint3 = runtimeData.raycastEnd;
                        }

                        createBoxVisualization.lineRenderer.SetPositions(new Vector3[] { boxPoint1, boxPoint2, runtimeData.raycastEnd });
                    }
                    break;
                case CreateBoxState.FourthPointHeight:
                    {
                        // TODO only allow to place it vertical


                        if (OVRInput.GetDown(config.createBoundingBoxPointButton))
                        {
                            // finito
                            boxPoint4 = runtimeData.raycastEnd;
                            sm.SetState(sm.scanSelection);
                        }

                        createBoxVisualization.lineRenderer.SetPositions(new Vector3[] { boxPoint1, boxPoint2, boxPoint3, runtimeData.raycastEnd });
                    }
                    break;



            }
        }

    }
}