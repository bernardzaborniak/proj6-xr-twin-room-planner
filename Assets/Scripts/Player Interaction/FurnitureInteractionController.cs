using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FurnitureInteractionController : MonoBehaviour
{
    // hovers with a ray over all bounding boxes, on hover - the boxes display their hover material
    // able to select one,than hover material changes and UI is being displayed
    // interact with some kind of interaction interface for the furniture

   

    public OVRInput.RawButton selectFurnitureButton;
    public OVRInput.RawButton pressUiButton;
    // public OVRInput.RawButton moveFuritureButton; // only in the layout version

    BaseFurniture hoveredOverFurniture;
    BaseFurniture selectedFurniture;

    [Header("Raycast")]
    public Transform rayOrigin;
    public float maxLineDistance;
    public LineRenderer lineRenderer;

    // Furniture Raycast
    public LayerMask furnitureMask;
    bool furnitureHasHit;
    RaycastHit furnitureHit;
    Vector3 furnitureRayEnd;

    // Ui Raycast
    public LayerMask uiMask;
    bool uiHasHit;
    RaycastHit uiHit;
    Vector3 uiRayEnd;

    [Header("Additional Functionalities")]
    [SerializeField] bool allowMoveFurniture;

    protected virtual void Start()
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

    protected virtual void Update()
    {
        HandleUiRay();
        HandleRayFurniturePhysicsCheck();
        HandleRayVisuals(uiHasHit);


        //HandleHoverInteractions();
        //HandleFurnitureSelectInput();

        // handlemoveinput in other version
    }


    protected void HandleUiRay()
    {
        furnitureRayEnd = rayOrigin.position + rayOrigin.forward * maxLineDistance;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

        if (Physics.Raycast(ray, out uiHit, maxLineDistance, uiMask))
        {
            uiRayEnd = uiHit.point;
            uiHasHit = true;

            UiInteractionHelper uiInteractionHelper = uiHit.collider.gameObject.GetComponent<UiInteractionHelper>();

            if (OVRInput.GetDown(pressUiButton) && uiInteractionHelper != null)
            {
                uiInteractionHelper.OnClick();
            }
        }
        else
        {
            uiHasHit = false;
        }
    }

    protected void HandleRayFurniturePhysicsCheck()
    {
        furnitureRayEnd = rayOrigin.position + rayOrigin.forward * maxLineDistance;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out furnitureHit, maxLineDistance, furnitureMask))
        {
            furnitureHasHit = true;
            furnitureRayEnd = furnitureHit.point;

        }
        else
        {
            furnitureHasHit = false;
        }
    }

    protected void HandleRayVisuals(bool setToUiRay)
    {
        // UI has priority over furniture

        lineRenderer.SetPosition(0, rayOrigin.position);

        if (setToUiRay)
        {
            lineRenderer.SetPosition(1, uiRayEnd);
        }
        else
        {
            lineRenderer.SetPosition(1, furnitureRayEnd);
        }
    }

    protected void HandleHoverInteractions()
    {
        if(uiHasHit)
        {
            hoveredOverFurniture?.OnHoverEnd();
            hoveredOverFurniture = null;
            return;
        }   
        

        BaseFurniture newHoveredFurniture = null;
        if (furnitureHasHit) newHoveredFurniture = furnitureHit.transform.GetComponent<BaseFurniture>();

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

    protected void HandleMoveFurniture()
    {
        if (!allowMoveFurniture)
            return;
    }

    protected void HandleFurnitureSelectInput()
    {
        if (uiHasHit) return;


        if (OVRInput.GetDown(selectFurnitureButton) && hoveredOverFurniture != null)
        {
            if (selectedFurniture != null)
            {
                selectedFurniture.OnDeselect();
            }

            selectedFurniture = hoveredOverFurniture;
            //hoveredOverFurniture = null;
            selectedFurniture.OnSelect(rayOrigin.forward);
        }
    }

    protected virtual void OnSelectedFurnitureDetails()
    {

    }

    protected virtual void OnDeselectedFurnitureDetails()
    {

    }

}
