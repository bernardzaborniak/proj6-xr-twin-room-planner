using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelSelectorUiButton : UiCustomButton
{
    [SerializeField] TMP_Text labelText;
    [SerializeField] Image background;
    [SerializeField] Color selectedColor;
    [SerializeField] Color defaultColor;

    FurnitureLabel label;
    LabelSelectorUi uiController;

    //public Action OnClickCallback;

    public void SetUpButton(FurnitureLabel label, LabelSelectorUi uiController)
    {
        this.label = label;
        this.labelText.text = label.ToString();
        this.uiController = uiController;
    }

    public override void OnClick()
    {
        base.OnClick();
        uiController.OnLabelSelected(label);
        //OnClickCallback.Invoke();
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            background.color = selectedColor;
        }
        else
        {
            background.color = defaultColor;
        }
    }


    public FurnitureLabel GetLabel()
    {
        return label;
    }
}
