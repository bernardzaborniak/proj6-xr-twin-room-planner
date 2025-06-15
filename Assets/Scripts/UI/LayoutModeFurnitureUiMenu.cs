using UnityEngine;
using UnityEngine.UI;

public class LayoutModeFurnitureUiMenu : FurnitureUiMenu
{
    [SerializeField] UiCustomButton rotateLeft;
    [SerializeField] UiCustomButton rotateRight;
    [SerializeField] LabelSelectorUi labelSelectorUi; 

    LayoutModeFurniture furniture;

    void Start()
    {
        rotateLeft.OnClickCallback += RotateLeft;
        rotateRight.OnClickCallback += RotateRight;

        
    }

    public void SetUp(LayoutModeFurniture furniture)
    {
        this.furniture = furniture;
        labelSelectorUi.SetUp(furniture);
    }

    void RotateLeft()
    {
        furniture.RotateLeftByUi();
    }

    void RotateRight()
    {
        furniture.RotateRightByUi();
    }
}
