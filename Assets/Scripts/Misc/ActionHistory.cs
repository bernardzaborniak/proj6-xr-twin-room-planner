using UnityEngine;
using System.Collections.Generic;

public class ActionHistory : MonoBehaviour
{
    public static ActionHistory Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

   private List<UserAction> actionHistory = new List<UserAction>();

    public void AddAction(UserAction action)
    {
        actionHistory.Add(action);
    }
    
    public void UndoLastAction()
    {
        // TODO: Set to last action (pos, rot)
        if (actionHistory.Count > 0)
        {
            actionHistory.RemoveAt(actionHistory.Count - 1);
            EventLogger.Instance.LogInteraction("Undo", EventLogger.actionTypes.Undo);
        }
    }
}

public class UserAction
{
    public string Name;
    public GameObject Obj;
    public Vector3 Position;
    public Vector3 Rotation;
}
