using UnityEngine;
using System.IO;
using System;
using Unity.IO.LowLevel.Unsafe;
using System.Linq;
using System.Collections.Generic;

// TODO: add logger to each interaction, fix actions count, add right trigger count
public class EventLogger : MonoBehaviour
{

    public enum actionTypes { MenuOpened, MenuClosed, MenuInteraction, ObjectMoved, ObjectAdded, ObjectRemoved, WallDrawn, Undo }
    static Dictionary<actionTypes, int> actionCounts = new Dictionary<actionTypes, int>();


    public static EventLogger Instance { get; private set; }

    private string filePath;
    private string logFolderPath;
    private int actions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //logFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DT4XR_Interaction_Logs"); Windows
        logFolderPath = Path.Combine(Application.persistentDataPath, "DT4XR_Interaction_Logs");

        if (!Directory.Exists(logFolderPath))
        {
            Directory.CreateDirectory(logFolderPath);
        }

        string timestamp = DateTime.Now.ToString("dd-MM_HH-mm");
        filePath = Path.Combine(logFolderPath, $"log-{timestamp}.log");
        File.WriteAllText(filePath, $"{getTimestamp()} Application started\n");
    }

    private string getTimestamp()
    {
        return DateTime.Now.ToString("[HH:mm:ss]");
    }

    /// <summary>
    /// Logs events to the console and stores them for later review during the user study.
    /// Logs are saved in the folder "DT4XR_Interaction_Logs".
    /// </summary>
    /// <param name="action">A string describing what action was performed (e.g. "Spawned Object X").</param>
    /// <param name="actionType">Type of action used to categorize the interaction (e.g. MenuClosed, MenuInteraction, ObjectMoved).</param>
    public void LogInteraction(string action, actionTypes actionType)
    {
        if (actionCounts.ContainsKey(actionType))
        {
            actionCounts[actionType]++;
        }
        else
        {
            actionCounts[actionType] = 1;
        }

        string line = $"{getTimestamp()} [{actionType}] {action}";
        actions +=1;
        WriteInFile(line);
        return;
    }

    private void WriteInFile(string line)
    {
        File.AppendAllText(filePath, line + "\n");
    }
    
    private void Summary()
    {
        WriteInFile("\n\n=== Session Summary ===");
        WriteInFile($"\nAmount of actions: {actions}\nUsage time: {getTimeSinceStart()}\n");

        foreach (actionTypes actionType in Enum.GetValues(typeof(actionTypes)))
        {
            int count = actionCounts.ContainsKey(actionType) ? actionCounts[actionType] : 0;
            WriteInFile($"{actionType}: {count}\n");
        }

        WriteInFile("=======================");
        return;
    }

    private string getTimeSinceStart()
    {
        float runtimeSeconds = Time.realtimeSinceStartup;
        TimeSpan runtime = TimeSpan.FromSeconds(runtimeSeconds);

        string duration = string.Format("{0:D2}:{1:D2}:{2:D2}", runtime.Hours, runtime.Minutes, runtime.Seconds);

        return duration;
    }

    private void OnApplicationQuit()
    {
        Summary();
    }
}
