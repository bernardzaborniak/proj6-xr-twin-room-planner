using System;
using UnityEngine;

public class CreateNewBoundingBoxScanMenu : MonoBehaviour
{
    [SerializeField] UiCustomButton addFurnitureButton;
    [SerializeField] UiCustomButton addWallButton;
    [SerializeField] UiCustomButton deleteWallsButton;
    [SerializeField] UiCustomButton cancelButton;

    public Action OnAddFurnitureClickedCallback;
    public Action OnAddWallClickedCallback;
    public Action OnCancelClickedCallback;
    public Action OnDeleteWallsClickedCallback;

    void Start()
    {
        addFurnitureButton.OnClickCallback += OnAddFurnitureClicked;
        addWallButton.OnClickCallback += OnAddWallClicked;
        cancelButton.OnClickCallback += OnCancelClicked;
        deleteWallsButton.OnClickCallback += OnDeleteWallsClicked;
    }



    void OnAddFurnitureClicked()
    {
        Debug.Log($"[UI] OnAddFurnitureClicked");
        OnAddFurnitureClickedCallback?.Invoke();
    }

    void OnAddWallClicked()
    {
        OnAddWallClickedCallback?.Invoke();
    }

    void OnCancelClicked()
    {
        OnCancelClickedCallback?.Invoke();
    }

    void OnDeleteWallsClicked()
    {
        OnDeleteWallsClickedCallback?.Invoke();
    }
}
