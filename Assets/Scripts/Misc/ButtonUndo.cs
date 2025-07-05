using UnityEngine;

public class ButtonUndo : MonoBehaviour
{
    public Transform rightHandAnchor;
    public LayerMask uiMask;

    private UiCustomButton buttonHitByRay = null;

 
    void Update()
    {
        HandleUiInteraction();
    }

    void HandleUiInteraction()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && buttonHitByRay != null)
        {
            buttonHitByRay.OnClick();
        }
    }
}
