using UnityEngine;

public class ScanModeFurnitureUiMenu : FurnitureUiMenu
{
    [SerializeField] UiCustomButton tempButton;
    [SerializeField] GameObject rectEnabledByButton;
    [SerializeField] LabelSelectorUi labelSelectorUi;
    [SerializeField] UiCustomButton deleteButton;



    ScanModeFurniture furniture;

    void Start()
    {
        tempButton.OnClickCallback += ToggleRect;
        deleteButton.OnClickCallback += Delete;
    }

    public void SetUp(ScanModeFurniture furniture)
    {
        this.furniture = furniture;
        labelSelectorUi.SetUp(furniture);

    }


    void ToggleRect()
    {
        rectEnabledByButton.SetActive(!rectEnabledByButton.activeSelf);
    }

    void Delete()
    {
        furniture.DeleteByUi();
    }
}
