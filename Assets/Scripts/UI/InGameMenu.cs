using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] UiCustomButton backToMainMenuButton;

    [Tooltip("Resets the empty room o the scan from the glasses again")]
    [SerializeField] UiCustomButton resetRoomScanButton;
    [Tooltip("Resets the empty room to start from scratch again")]
    [SerializeField] UiCustomButton clearRoomScanButton;
    [Tooltip("Resets the layout to be generated based on scan again")]
    [SerializeField] UiCustomButton resetLayoutModeButton;
    [SerializeField] UiCustomButton closeMainMenuButton;

    [Space]
    [SerializeField] int mainMenuSceneIndex = 0;
    [SerializeField] GameObject uiParent;

    void Start()
    {
        HideMenu();

        backToMainMenuButton.OnClickCallback += BackToMainMenu;
        resetRoomScanButton.OnClickCallback += ResetRoomScan;
        clearRoomScanButton.OnClickCallback += ClearRoomScan;
        resetLayoutModeButton.OnClickCallback += ResetLayoutModeFromScan;
        closeMainMenuButton.OnClickCallback += ExitMenuClicked;
    }


    public void ToggleMenu(Vector3 cameraPos, Vector3 cameraForward)
    {
        if (uiParent.activeSelf)
        {
            HideMenu();
        }
        else
        {
            ShowMenu(cameraPos, cameraForward);
        }
    }

    public void ShowMenu(Vector3 cameraPos, Vector3 cameraForward)
    {
        uiParent.SetActive(true);

        cameraPos.y = 0;
        uiParent.transform.position = cameraPos + cameraForward * 2 + Vector3.up * 2;

        cameraForward.y = cameraForward.y * 0.5f; // make rotation slightly less steep
        uiParent.transform.forward = cameraForward;
    }

    public void HideMenu()
    {
        uiParent.SetActive(false);
    }

    void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    void ResetRoomScan()
    {
        RoomsManager.Instance.CaptureCurrentMetaRoom();
        RoomsManager.Instance.ShowRoomScan();
    }

    void ClearRoomScan()
    {
        RoomsManager.Instance.CreateNewEmptyRoomScan();
        RoomsManager.Instance.ShowRoomScan();
    }

    void ResetLayoutModeFromScan()
    {
        RoomsManager.Instance.RevertRoomVariation(0);
        RoomsManager.Instance.ShowRoomVariation(0);
    }

    void ExitMenuClicked()
    {
        HideMenu();
    }

}
