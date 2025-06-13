using UnityEngine;

public abstract class BaseFurniture : MonoBehaviour, IInteractableFurniture
{
    protected FurnitureData localDataCopy;
    [Header("Hierarchy Refs")]
    [SerializeField] protected MeshFilter boundingBoxMeshFilter;
    [SerializeField] protected MeshRenderer boundingBoxMeshRenderer;
    [SerializeField] protected BoxCollider boxCollider;
    [SerializeField] protected GameObject uiMenu;

    [Header("Material Refs")]

    [SerializeField] protected Material hoverMaterial;
    [SerializeField] protected Material selectedMaterial;

    [Header("Hover & Select Outline Visuals")]
    [SerializeField] protected MeshRenderer rendererToApplyOutlineTo;
    protected Material originalBeforeOutlineMaterial;

    public bool Interactable { get; set; }

    void Awake()
    {
        uiMenu.SetActive(false);
        if (rendererToApplyOutlineTo != null)
        {
            originalBeforeOutlineMaterial = rendererToApplyOutlineTo.materials[0];
        }
    }

    public FurnitureData ConvertToFurnitureDataObject()
    {
        localDataCopy.posInRoom = transform.localPosition;
        localDataCopy.rotInRoom = transform.localRotation;

        return localDataCopy;
    }

    protected void SetBoxCollider()
    {
        boxCollider.center = boundingBoxMeshRenderer.localBounds.center;
        boxCollider.size = boundingBoxMeshRenderer.localBounds.size;
    }

    public void OnHoverStart()
    {
        Debug.Log($"{gameObject.name} OnHoverStart");
        rendererToApplyOutlineTo.materials = new Material[2] { originalBeforeOutlineMaterial, hoverMaterial };
    }

    public void OnHoverEnd()
    {
        Debug.Log($"{gameObject.name} OnHoverEnd");
        rendererToApplyOutlineTo.materials = new Material[1] { originalBeforeOutlineMaterial };
    }

    public void OnSelect()
    {
        rendererToApplyOutlineTo.materials = new Material[2] { originalBeforeOutlineMaterial, selectedMaterial };
        uiMenu.SetActive(true);
    }

    public void OnDeselect()
    {
        rendererToApplyOutlineTo.materials = new Material[1] { originalBeforeOutlineMaterial };
        uiMenu.SetActive(false);
    }
}
