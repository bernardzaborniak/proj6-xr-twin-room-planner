using Meta.XR.MRUtilityKit;
using System.Threading.Tasks;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class ApplicationManager : MonoBehaviour
{
    // this takes care of opening up the room scan at the start of appliation or no
    // have a scripting order before?

    enum ApplicationMode
    {
        StartWithMetaScannedRoom,
        StartWithMetaScannedRoomWithFallbackOption,
        StartWithBlankRoom
    }

    [SerializeField] ApplicationMode applicationMode;

    [SerializeField] MRUK mrUtilityKit;
    [SerializeField] RoomsManager roomsManager;
    [SerializeField] PlayerController playerController;

    enum ApplicationState
    {
        InitialState,
        WaitingForMrukToLoadRoom,
        RoomLoaded
    }
    [Header("Readonly, dont edit this one :0")]
    [SerializeField] ApplicationState applicationState;

    private Task<MRUK.LoadDeviceResult> loadTask;



    void Awake()
    {
        Debug.Log("[ApplicationManager] startup");

        applicationState = ApplicationState.InitialState;


        if (applicationMode == ApplicationMode.StartWithMetaScannedRoom)
        {
            mrUtilityKit.SceneSettings.DataSource = MRUK.SceneDataSource.Device;

            StartLoadRoomTask();
        }
        else if (applicationMode == ApplicationMode.StartWithMetaScannedRoomWithFallbackOption)
        {
            mrUtilityKit.SceneSettings.DataSource = MRUK.SceneDataSource.DeviceWithPrefabFallback;

            StartLoadRoomTask();

        }
        else if (applicationMode == ApplicationMode.StartWithBlankRoom)
        {
            StartPlayerInBlankRoom();          
        }
    }

    void Update()
    {
        //Debug.Log("[ApplicationManager] Update");

        if (applicationState == ApplicationState.WaitingForMrukToLoadRoom)
        {
            Debug.Log("[ApplicationManager] Try loading room from device");
            CheckLoadTaskStatus();
        }
    }

    void StartLoadRoomTask()
    {
        loadTask = mrUtilityKit.LoadSceneFromDevice(false);
        applicationState = ApplicationState.WaitingForMrukToLoadRoom;
    }

    private void OnDestroy()
    {
        
    }



    void CheckLoadTaskStatus()
    {
        if(loadTask == null)
        {
            Debug.Log("[ApplicationManager] load task has become null :0");
            return;
        }

        if (loadTask.IsCompleted) 
        {
            if (loadTask.IsFaulted)
            {
                Debug.Log("[ApplicationManager]load room from quest device task failed");
            }
            else if (loadTask.IsCanceled)
            {
                Debug.Log("[ApplicationManager] load room from quest device task cenceled");
            }
            else
            {
                MRUK.LoadDeviceResult result = loadTask.Result;
                Debug.Log("[ApplicationManager] load room from quest device complete");
                applicationState = ApplicationState.RoomLoaded;

                if (result == MRUK.LoadDeviceResult.Success) 
                {
                    StartPlayerOnRoomLoadedSuccessfully();
                }
                else
                {
                    // try again?
                    Debug.Log("[ApplicationManager] MRUK.LoadDeviceResult ws not Success -> Try to load room again start");
                    StartLoadRoomTask();

                }

            }
        }
    }

    void StartPlayerInBlankRoom()
    {
        Debug.Log("[ApplicationManager] StartPlayerInBlankRoom");

        roomsManager.CreateNewEmptyRoomScan();
        roomsManager.Create2ExampleRoomVariations();

        playerController.StartWithScanMode();

        applicationState = ApplicationState.RoomLoaded;
        // todo load empty space with instructions how to create rom?

        // TODO start player controller and load and save empty room here
    }

    void StartPlayerOnRoomLoadedSuccessfully()
    {
        Debug.Log("[ApplicationManager] StartPlayerOnRoomLoadedSuccessfully");

        // we recreate the save files every time but thats ok for now also for our test? because the orientation is always wrong somehow, thats annoying
        roomsManager.CaptureCurrentMetaRoom();
        roomsManager.Create2ExampleRoomVariations();

        playerController.StartWithScanMode();

        applicationState = ApplicationState.RoomLoaded;
    }
}
