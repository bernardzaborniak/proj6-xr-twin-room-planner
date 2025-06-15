using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LabelSelectorUi : UiCustomButton
{
    [Header("minimized Refs")]
    [SerializeField] TMP_Text currentSelectedLabelText;

    [Header("selector panel refs")]
    [SerializeField] Transform spawedButtonsParent;
    [SerializeField] GameObject labelSelectButtonPrefab;
    [SerializeField] float verticalSpawnSpacing;
    [SerializeField] float horizontalSpawnSpacing;
    [SerializeField] int verticalRows;

    BaseFurniture furniture;
    List<LabelSelectorUiButton> spawnedButtons = new List<LabelSelectorUiButton>();


    public void SetUp(BaseFurniture furniture)
    {
        this.furniture = furniture;
        SpawnButtons();
        spawedButtonsParent.gameObject.SetActive(false); // hide 
        currentSelectedLabelText.text = furniture.LocalDataCopy.label.ToString();

    }

    public override void OnClick()
    {
        Debug.Log($"[Label UI] OnLabelButtonClicked active? {spawedButtonsParent.gameObject.activeSelf}");

        if (spawedButtonsParent.gameObject.activeSelf)
        {
            HideLabels();
        }
        else
        {
            ShowLabels();
        }
    }

    public void SpawnButtons()
    {
        // TODO orient them nicer
        Debug.Log($"[Label UI] SpawnButtons {System.Enum.GetValues(typeof(FurnitureLabel)).Length}");

        float verticalAdjuster = 0;

        for (int i = 0; i < System.Enum.GetValues(typeof(FurnitureLabel)).Length; i++) 
        {

            FurnitureLabel currentLabel = (FurnitureLabel)System.Enum.GetValues(typeof(FurnitureLabel)).GetValue(i);

            if (FurnitureLabelUtilities.IsLabelFlatWall(currentLabel))
                continue;

            LabelSelectorUiButton button = Instantiate(labelSelectButtonPrefab,spawedButtonsParent).GetComponent<LabelSelectorUiButton>();
            button.transform.localPosition = Vector3.zero;
            button.transform.localPosition += Vector3.up * -verticalAdjuster;

            button.SetUpButton(currentLabel,  this);
            spawnedButtons.Add(button);

            verticalAdjuster += verticalSpawnSpacing;
        }
    }

    public void ShowLabels()
    {
        UpdateSelectedColors();
        spawedButtonsParent.gameObject.SetActive(true); // hide 

    }

    void UpdateSelectedColors()
    {
        for (int i = 0; i < spawnedButtons.Count; i++)
        {
            spawnedButtons[i].SetSelected(furniture.LocalDataCopy.label == spawnedButtons[i].GetLabel());
        }
    }

    public void HideLabels() 
    {
        spawedButtonsParent.gameObject.SetActive(false); // hide 

    }

    public void OnLabelSelected(FurnitureLabel label)
    {
        furniture.ChangeLabelByUi(label);
        currentSelectedLabelText.text = furniture.LocalDataCopy.label.ToString();
        UpdateSelectedColors();
    }
}
