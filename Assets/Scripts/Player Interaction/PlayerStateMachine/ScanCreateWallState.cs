using System.Collections.Generic;
using UnityEngine;

public class ScanCreateWallState : PlayerControllerInteractionState
{
    GameObject previewWall;
    //LineRenderer lineRenderer;
    List<Vector3> wallPoints = new List<Vector3>();
    //bool building = false;

    Color lineRendererColorBefore;



    public override void OnStateEnter()
    {
        wallPoints.Clear();
        refs.lineRenderer.enabled = true;
        //building = true;

        //TODO enable required stuuff
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
        HandleUiInteraction();

        if (!runtimeData.raycastWasSuccessfull)
        {
            HandleWallGroundPlacementRay();
        }

        HandleRayVisuals();

        // If we press the deselect button again, exit the scan edit state
        if (OVRInput.GetDown(config.deselectFurnitureButton))
        {
            sm.SetState(sm.scanSelection);
        }
    }

    void HandleWallGroundPlacementRay()
    {
        Ray ray = new Ray(refs.rayOrigin.transform.position, refs.rayOrigin.transform.forward);
        // RaycastHit hit;

        runtimeData.raycastEnd = refs.rayOrigin.position + refs.rayOrigin.forward * config.maxRaycastDistance;
        runtimeData.raycastWasSuccessfull = Physics.Raycast(ray, out runtimeData.raycastHitInfo, config.maxRaycastDistance, config.placeWallMask);


        if (runtimeData.raycastWasSuccessfull)
        {
            runtimeData.raycastEnd = runtimeData.raycastHitInfo.point;

            //Debug.Log(hit.point.x + " " + hit.point.z);

            if (wallPoints.Count > 0)
            {
                ShowPreviewWall(wallPoints[wallPoints.Count - 1], runtimeData.raycastEnd);
            }

            if (OVRInput.GetDown(config.placeWallButton))
            {
                PlacePoint(runtimeData.raycastEnd);
            }

        }
    }

    void OnCancelAddBoxClicked()
    {
        sm.SetState(sm.scanSelection);
    }

    void PlacePoint(Vector3 newPoint)
    {
        Debug.Log("[ScanCreateWallState] Place Wall Point called");

        if (wallPoints.Count == 0)
        {
            wallPoints.Add(newPoint);
            return;
        }

        // Snap to start wall if close enough
        if (Vector3.Distance(newPoint, wallPoints[0]) < config.wallPlacementSnapDistance && wallPoints.Count > 0)
        {
            CreateWall(wallPoints[wallPoints.Count - 1], wallPoints[0]);
            GameObject.Destroy(previewWall);
            //building = false;
            //refs.lineRenderer.enabled = false;

            // automatically exit state on completion
            sm.SetState(sm.scanSelection);

            return;
        }

        CreateWall(wallPoints[wallPoints.Count - 1], newPoint);
        wallPoints.Add(newPoint);
    }

    // Create wall by calling PositionWall()
    void CreateWall(Vector3 start, Vector3 end)
    {
        GameObject wall = GameObject.Instantiate(refs.placeWallPrefab);
        Debug.Log("Wall placed from " + start + " to " + end);
        PositionWall(wall, start, end);
    }

    // Handle the visualization of preview wall
    void ShowPreviewWall(Vector3 start, Vector3 end)
    {
        if (previewWall == null)
        {
            previewWall = GameObject.Instantiate(refs.placeWallPrefab);
            previewWall.GetComponent<Renderer>().material = refs.wallPreviewMaterial;
            Collider col = previewWall.GetComponent<Collider>();
        }
        PositionWall(previewWall, start, end);
    }

    // Position and scale wall between start and end point
    void PositionWall(GameObject wall, Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float length = new Vector3(direction.x, 0f, direction.z).magnitude;

        wall.SetActive(true);
        Vector3 center = (start + end) / 2f;
        center.y = refs.placeWallPrefab.transform.localScale.y / 2;

        wall.transform.position = center;
        wall.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z), Vector3.up);
        wall.transform.localScale = new Vector3(config.wallPlacementThickness, 2f, length);
    }
}