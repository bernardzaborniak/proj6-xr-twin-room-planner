using System;
using UnityEngine;

public class CreateNewBoundingBoxScanMenu : MonoBehaviour
{
    [SerializeField] UiCustomButton addFurnitureButton;
    [SerializeField] UiCustomButton addWallButton;
    [SerializeField] UiCustomButton cancelButton;

    public Action OnAddFurnitureClickedCallback;
    public Action OnAddWallClickedCallback;
    public Action OnCancelClickedCallback;

    void Start()
    {
        addFurnitureButton.OnClickCallback += OnAddFurnitureClicked;
        addWallButton.OnClickCallback += OnAddWallClicked;
        cancelButton.OnClickCallback += OnCancelClicked;
    }



    void OnAddFurnitureClicked()
    {
        Debug.Log($"[UI] OnAddFurnitureClicked");
        OnAddFurnitureClickedCallback.Invoke();
    }

    void OnAddWallClicked()
    {
        OnAddWallClickedCallback.Invoke();
    }

    void OnCancelClicked()
    {
        OnCancelClickedCallback.Invoke();
    }
}
