using UnityEngine;

public class ScanModeFurnitureUiMenu : FurnitureUiMenu
{
    [SerializeField] UiInteractionCustomButton tempButton;
    [SerializeField] GameObject rectEnabledByButton;


    ScanModeFurniture furniture;

    void Start()
    {
        tempButton.OnClick += ToggleRect;
    }

    public void SetUp(ScanModeFurniture furniture)
    {
        this.furniture = furniture;
    }

   
    void ToggleRect()
    {
        rectEnabledByButton.SetActive(!rectEnabledByButton.activeSelf);
    }
}
