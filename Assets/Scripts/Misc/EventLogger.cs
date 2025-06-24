using UnityEngine;
using System.IO;
using System;
using Unity.IO.LowLevel.Unsafe;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Logs events to the console and stores them for review of user study.
/// EventLogger.Instance.LogInteraction("...");
/// </summary>
/// 

// TODO: classify type of action, add logger to each interaction, set folder path for Quest

public class EventLogger : MonoBehaviour
{

    public static EventLogger Instance { get; private set; }

    private string filePath;
    private string logFolderPath;
    private List<string> actions = new List<string>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        } 
        else
        {
            Instance = this; 
        }

        logFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DT4XR_Interaction_Logs");

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

    public void LogInteraction(string action)
    {
        string line = $"{getTimestamp()} {action}";
        actions.Add(line);
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
        WriteInFile($"\nAmount of actions: {actions.Count()}\nUsage time: {getTimeSinceStart()}\n");
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
