using System;
using TMPro;
using UnityEngine;


public class UiCustomButton : MonoBehaviour
{
    public Action OnClickCallback;

    public virtual void OnClick()
    {
        OnClickCallback?.Invoke();
    }
}
