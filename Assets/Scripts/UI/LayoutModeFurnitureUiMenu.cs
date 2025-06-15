using UnityEngine;
using UnityEngine.UI;

public class LayoutModeFurnitureUiMenu : FurnitureUiMenu
{
    [SerializeField] UiInteractionCustomButton rotateLeft;
    [SerializeField] UiInteractionCustomButton rotateRight;

    LayoutModeFurniture furniture;

    void Start()
    {
        rotateLeft.OnClick += RotateLeft;
        rotateRight.OnClick += RotateRight;
    }

    public void SetUp(LayoutModeFurniture furniture)
    {
        this.furniture = furniture;
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
