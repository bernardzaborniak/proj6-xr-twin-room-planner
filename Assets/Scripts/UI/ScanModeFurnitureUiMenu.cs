using UnityEngine;

public class ScanModeFurnitureUiMenu : FurnitureUiMenu
{
    [SerializeField] UiCustomButton tempButton;
    [SerializeField] GameObject rectEnabledByButton;
    [SerializeField] LabelSelectorUi labelSelectorUi;



    ScanModeFurniture furniture;

    void Start()
    {
        tempButton.OnClickCallback += ToggleRect;
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
}
