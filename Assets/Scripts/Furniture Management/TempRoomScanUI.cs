using UnityEngine;
using UnityEngine.UI;

public class TempRoomScanUI : MonoBehaviour
{
    [SerializeField] RoomsManager roomManager;
    [Space]
    [SerializeField] Button captureCurrentMetaRoomBtn;
    [Space]
    [SerializeField] Button showRoomScanBtn;
    [SerializeField] Button saveRoomScanBtn;
    [Space]
    [SerializeField] Button showRoomVar1Btn;
    [SerializeField] Button saveRoomVar1Btn;
    [SerializeField] Button revertRoomVar1Btn;
    [Space]
    [SerializeField] Button showRoomVar2Btn;
    [SerializeField] Button saveRoomVar2Btn;
    [SerializeField] Button revertRoomVar2Btn;

    // Set the callbacks
    void Start()
    {
        captureCurrentMetaRoomBtn.onClick.AddListener(OnCaptureCurrentMetaRoom);

        showRoomScanBtn.onClick.AddListener(OnShowRoomScan);
        saveRoomScanBtn.onClick.AddListener(OnSaveRoomScan);

        showRoomVar1Btn.onClick.AddListener(OnShowRoomVar1);
        saveRoomVar1Btn.onClick.AddListener(OnSaveRoomVar1);
        revertRoomVar1Btn.onClick.AddListener(OnRevertVar1);

        showRoomVar2Btn.onClick.AddListener(OnShowRoomVar2);
        saveRoomVar2Btn.onClick.AddListener(OnSaveRoomVar2);
        revertRoomVar2Btn.onClick.AddListener(OnRevertVar2);
    }

    void OnCaptureCurrentMetaRoom()
    {
        roomManager.CaptureCurrentMetaRoom();
        OnRevertVar1();
        OnRevertVar2();
    }


    void OnShowRoomScan()
    {
        roomManager.ShowRoomScan();
    }

    void OnSaveRoomScan()
    {
        roomManager.SaveRoomScanFromVisualization();
    }


    void OnShowRoomVar1()
    {
        roomManager.ShowRoomVariation(0);
    }

    void OnSaveRoomVar1()
    {
        roomManager.SaveRoomVariationFromVisualization(0);

    }

    void OnRevertVar1()
    {
        roomManager.RevertRoomVariation(0);
    }


    void OnShowRoomVar2()
    {
        roomManager.ShowRoomVariation(1);
    }

    void OnSaveRoomVar2()
    {
        roomManager.SaveRoomVariationFromVisualization(1);
    }

    void OnRevertVar2()
    {
        roomManager.RevertRoomVariation(1);
    }
}
