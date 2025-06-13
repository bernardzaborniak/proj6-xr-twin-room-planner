using UnityEngine;
using UnityEngine.EventSystems;

public class FurnitureInteractionController : MonoBehaviour
{
    // hovers with a ray over all bounding boxes, on hover - the boxes display their hover material
    // able to select one,than hover material changes and UI is being displayed
    // interact with some kind of interaction interface for the furniture

    public OVRInput.RawButton selectFurnitureButton;
    public OVRInput.RawButton pressUiButton;
    // public OVRInput.RawButton moveFuritureButton; // only in the layout version

    IInteractableFurniture hoveredOverFurniture;
    IInteractableFurniture selectedFurniture;

    [Header("Raycast")]
    public Transform rayOrigin;
    public float maxLineDistance;
    public LineRenderer lineRenderer;
    public LayerMask furnitureMask;
    public LayerMask uiMask;

    RaycastHit currentHit;
    bool hasHitFurn;
    Vector3 currentRayEnd;

    void Start()
    {
        lineRenderer.positionCount = 2;
    }

    void OnEnable()
    {
        hoveredOverFurniture = null;
        selectedFurniture = null;
    }

    void OnDisable()
    {
        //hoveredOverFurniture?.OnHoverEnd();
        hoveredOverFurniture = null;

        //selectedFurniture?.OnDeselect();
        selectedFurniture = null;
    }

    void Update()
    {
        if ((!HandleUiRay()))
        {         
            HandleRayFurniturePhysicsCheck();
        }
        HandleRayVisuals();

        
        HandleHoverInteractions();
        HandleSelectInput();
        // handlemoveinput in other version
    }

    protected bool HandleUiRay()
    {
        bool hitUi;
        currentRayEnd = rayOrigin.position + rayOrigin.forward * maxLineDistance;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

        if (Physics.Raycast(ray, out currentHit, maxLineDistance, uiMask))
        {
            currentRayEnd = currentHit.point;
            hitUi =  true;

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = currentHit.point
            };

            // Trigger pointer enter event (like hovering over the UI element)
            ExecuteEvents.Execute(currentHit.collider.gameObject, pointerData, ExecuteEvents.pointerEnterHandler);

            // If the controller button is pressed, trigger the click event
            if (OVRInput.GetDown(pressUiButton)) // Customize this based on your input button
            {
                ExecuteEvents.Execute(currentHit.collider.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
            }

        }
        else
        {
            hitUi =  false;
        }


        return hitUi;
    }

    protected void HandleRayFurniturePhysicsCheck()
    {
        currentRayEnd = rayOrigin.position + rayOrigin.forward * maxLineDistance;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out currentHit, maxLineDistance, furnitureMask))
        {
            hasHitFurn = true;
            currentRayEnd = currentHit.point;

        }
        else
        {
            hasHitFurn = false;
        }
    }



    protected void HandleRayVisuals()
    {
        lineRenderer.SetPosition(0, rayOrigin.position);
        lineRenderer.SetPosition(1, currentRayEnd);
    }

    protected void HandleHoverInteractions()
    {
        IInteractableFurniture newHoveredFurniture = null;
        if (hasHitFurn) newHoveredFurniture = currentHit.transform.GetComponent<IInteractableFurniture>();

        if (newHoveredFurniture != null && newHoveredFurniture.Interactable)
        {
            if (newHoveredFurniture != selectedFurniture && newHoveredFurniture != hoveredOverFurniture)
            {
                if (hoveredOverFurniture != null)
                {
                    // stop previus hover
                    hoveredOverFurniture.OnHoverEnd();
                }

                hoveredOverFurniture = newHoveredFurniture;
                hoveredOverFurniture.OnHoverStart();
            }  
        }
        else 
        {
            // stop previus hover
            hoveredOverFurniture?.OnHoverEnd();
            hoveredOverFurniture = null;
        }
    }


    protected void HandleSelectInput()
    {
        if (OVRInput.GetDown(selectFurnitureButton) && hoveredOverFurniture != null)
        {
            if (selectedFurniture != null)
            {
                selectedFurniture.OnDeselect();
            }

            selectedFurniture = hoveredOverFurniture;
            selectedFurniture.OnSelect(rayOrigin.forward);
        }
    }

}
