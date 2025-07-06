using UnityEngine;
using UnityEngine.UI;

public class OpenControlSheet : MonoBehaviour
{

    public Canvas controls;

    private bool isOpen = false;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            showControlSheet();
        }
    }

    void showControlSheet()
    {
        isOpen = !isOpen;
        controls.gameObject.SetActive(isOpen);
    }
}
