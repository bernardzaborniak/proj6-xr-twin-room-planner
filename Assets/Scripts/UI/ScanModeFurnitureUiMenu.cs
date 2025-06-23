using UnityEngine;

public class ScanModeFurnitureUiMenu : FurnitureUiMenu
{
    [SerializeField] LabelSelectorUi labelSelectorUi;
    [SerializeField] UiCustomButton deleteButton;



    ScanModeFurniture furniture;

    void Start()
    {
        deleteButton.OnClickCallback += Delete;
    }

    public void SetUp(ScanModeFurniture furniture)
    {
        this.furniture = furniture;
        labelSelectorUi.SetUp(furniture);

    }


    void Delete()
    {
        furniture.DeleteByUi();
    }
}
