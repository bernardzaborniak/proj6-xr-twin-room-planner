using UnityEngine;
using System.Collections.Generic;

public class ActionHistory : MonoBehaviour
{
    public static ActionHistory Instance { get; private set; }
    [SerializeField] UiCustomButton undoAction;

    private void Start()
    {
        if (undoAction != null)
        {
            undoAction.OnClickCallback -= UndoLastAction;
            undoAction.OnClickCallback += UndoLastAction;
        }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }



   private List<UserAction> actionHistory = new List<UserAction>();

    public void AddAction(UserAction action)
    {
        Debug.Log($"Added action to history: {action}");
        actionHistory.Add(action);
    }
    
    public void UndoLastAction()
    {
        if (actionHistory.Count > 0)
        {
            UserAction lastAction = actionHistory[actionHistory.Count - 1];

            Debug.Log($"Undoing object: {lastAction.Obj.name} (ID={lastAction.Obj.GetInstanceID()})");

            EventLogger.Instance?.LogInteraction("Undo", EventLogger.actionTypes.Undo);

            lastAction.Obj.transform.position = lastAction.Position;
            lastAction.Obj.transform.rotation = Quaternion.Euler(lastAction.Rotation);

            actionHistory.RemoveAt(actionHistory.Count - 1);
        }
    }
}

public class UserAction
{
    public GameObject Obj;
    public Vector3 Position;
    public Vector3 Rotation;
}
