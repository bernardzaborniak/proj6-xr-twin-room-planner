using System;
using TMPro;
using UnityEngine;


public class UiCustomButton : MonoBehaviour
{
    public Action OnClickCallback;

    public virtual void OnClick()
    {
        EventLogger.Instance?.LogInteraction("Menu option clicked.", EventLogger.actionTypes.MenuInteraction);
        OnClickCallback?.Invoke();
    }
}
