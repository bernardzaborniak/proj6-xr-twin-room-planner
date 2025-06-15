using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseFurniture : MonoBehaviour
{
    public FurnitureData LocalDataCopy { get; protected set; }
    [Header("Hierarchy Refs")]
    [SerializeField] protected MeshFilter boundingBoxMeshFilter;
    [SerializeField] protected MeshRenderer boundingBoxMeshRenderer;
    [SerializeField] protected BoxCollider boxCollider;
    [SerializeField] protected FurnitureUiMenu uiMenu;
   

    [Header("Material Refs")]

    [SerializeField] protected Material hoverMaterial;
    [SerializeField] protected Material selectedMaterial;
    enum HoverSelectState
    {
       NotSelectedNorHovered,
       Hovered,
       Selected
    }
    HoverSelectState selectState;

    [Header("Hover & Select Outline Visuals")]
    [SerializeField] protected MeshRenderer rendererToApplyOutlineTo;
    protected Material originalBeforeOutlineMaterial;

    public bool Interactable { get; set; }

    #region Default Methods  

    void Awake()
    {
        uiMenu.gameObject.SetActive(false);
        if (rendererToApplyOutlineTo != null)
        {
            originalBeforeOutlineMaterial = rendererToApplyOutlineTo.materials[0];
        }
    }

    public FurnitureData ConvertToFurnitureDataObject()
    {
        LocalDataCopy.posInRoom = transform.localPosition;
        LocalDataCopy.rotInRoom = transform.localRotation;

        return LocalDataCopy;
    }

    protected void SetBoxCollider()
    {
        boxCollider.center = boundingBoxMeshRenderer.localBounds.center;
        boxCollider.size = boundingBoxMeshRenderer.localBounds.size;
    }

    public void OnHoverStart()
    {
        if (selectState == HoverSelectState.NotSelectedNorHovered)
        {
            selectState = HoverSelectState.Hovered;

            rendererToApplyOutlineTo.materials = new Material[2] { originalBeforeOutlineMaterial, hoverMaterial };
        }
    }

    public void OnHoverEnd()
    {
        if (selectState == HoverSelectState.Hovered)
        {
            selectState = HoverSelectState.NotSelectedNorHovered;

            rendererToApplyOutlineTo.materials = new Material[1] { originalBeforeOutlineMaterial };
        }
    }

    public void OnSelect(Vector3 selectDirection)
    {
        if (selectState == HoverSelectState.Hovered)
        {
            selectState = HoverSelectState.Selected;

            rendererToApplyOutlineTo.materials = new Material[2] { originalBeforeOutlineMaterial, selectedMaterial };
            uiMenu.OrientToPlayer(selectDirection);
            uiMenu.gameObject.SetActive(true);
        }        
    }

    public void OnDeselect()
    {
        if (selectState == HoverSelectState.Selected)
        {
            selectState = HoverSelectState.NotSelectedNorHovered;
            if (rendererToApplyOutlineTo != null)
                rendererToApplyOutlineTo.materials = new Material[1] { originalBeforeOutlineMaterial };

            if (!uiMenu.IsDestroyed())
            {
                uiMenu?.gameObject.SetActive(false);
            }
        }
    }

    #endregion


    #region UI Methods 

    protected abstract void OnUiChangedData();

    public void ChangeLabelByUi(FurnitureLabel newLabel)
    {
        LocalDataCopy.label = newLabel;
        LocalDataCopy.tempLabelString = newLabel.ToString();
        OnUiChangedData();
    }


    #endregion
}
