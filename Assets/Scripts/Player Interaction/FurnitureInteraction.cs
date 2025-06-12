using System;
using UnityEngine;

public class FurnitureInteraction : MonoBehaviour
{
    public OVRInput.RawButton shootingButton;

    public Transform rayOrigin;
    public float maxLineDistance;
    // public GameObject rayPrefab;
    public LineRenderer lineRenderer;

    public LayerMask mask;
    public float heightChangeSpeed;
    public float rotationChangeSpeed;

    RaycastHit currentHit;
    bool hasHit;

    bool isInteractingWithObject;
    Plane currentInteractionPlane;
    Vector3 furnitureToRayOffset;
    GameObject currentInteractingObject;
    float heightOffset;

    void Start()
    {
        //LineRenderer lineRenderer = Instantiate(rayPrefab).GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        Vector3 lineEnd = rayOrigin.position + rayOrigin.forward * maxLineDistance;
        
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out currentHit, maxLineDistance, mask))
        {
            hasHit = true;
            lineEnd = currentHit.point;

        }
        else
        {
            hasHit = false;
        }
        //Debug.Log($"Line renderer: {lineRenderer}");
        //Debug.Log($" lineEnd: {lineEnd}");
        //Debug.Log($" rayOrigin: {rayOrigin}");
        lineRenderer.SetPosition(0, rayOrigin.position);
        lineRenderer.SetPosition(1, lineEnd);

        if (OVRInput.GetDown(shootingButton))
        {
            SelectObject(ray);
        }

        if (OVRInput.GetUp(shootingButton))
        {
            StopSelectObject();
        }

        if (isInteractingWithObject)
        {
            MoveSelectedObject(ray);
            ChangeObjectHeight();
            ChangeObjectRotation();
        }
    }
    

    void SelectObject(Ray ray)
    {

        if (!hasHit)
            return;

        FurnitureVisualization visualization = currentHit.transform.GetComponent<FurnitureVisualization>();

        if (visualization != null && visualization.Moveable) 
        {
            heightOffset = 0;
            currentInteractingObject = visualization.gameObject;
            isInteractingWithObject = true;
            currentInteractionPlane = new Plane (Vector3.up, visualization.transform.position);
            float t = 0;
            currentInteractionPlane.Raycast(ray, out t);
            furnitureToRayOffset = visualization.transform.position - (rayOrigin.position + rayOrigin.forward * t);
        }
    }

    void MoveSelectedObject(Ray ray)
    {
        float t = 0;
        currentInteractionPlane.Raycast(ray, out t);
        Vector3 newpos = rayOrigin.position + rayOrigin.forward * t;
        currentInteractingObject.transform.position = newpos + furnitureToRayOffset + Vector3.up * heightOffset;
    }
    
    void StopSelectObject()
    {
        isInteractingWithObject = false;
    }

    void ChangeObjectHeight()
    {
        heightOffset += OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y * heightChangeSpeed;

    }

    void ChangeObjectRotation()
    {
        currentInteractingObject.transform.Rotate(0f, OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x * rotationChangeSpeed, 0f);
    }
}
