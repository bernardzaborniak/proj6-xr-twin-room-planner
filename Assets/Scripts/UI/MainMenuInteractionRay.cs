using System.Security.Cryptography;
using UnityEngine;

public class MainMenuInteractionRay : MonoBehaviour
{
    public Transform rightHandAnchor;
    public LayerMask uiMask;

    private LineRenderer lineRenderer;
    private UiCustomButton buttonHitByRay = null;
    private RaycastHit hit;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.red;
    }

    void Update()
    {
        HandleUiRay();
        HandleUiInteraction();
    }

    void HandleUiRay()
    {
        buttonHitByRay = null;

        Vector3 rayOrigin = rightHandAnchor.position;
        Vector3 rayDirection = rightHandAnchor.forward;
        Vector3 rayEnd = rayOrigin + rayDirection * 20f;

        Ray ray = new Ray(rayOrigin, rayDirection);

        if (Physics.Raycast(ray, out hit, 20f, uiMask))
        {
            rayEnd = hit.point;
            buttonHitByRay = hit.collider.GetComponent<UiCustomButton>();
        }

        lineRenderer.SetPosition(0, rayOrigin);
        lineRenderer.SetPosition(1, rayEnd);
    }

    void HandleUiInteraction()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && buttonHitByRay != null)
        {
            EventLogger.Instance.LogInteraction("Menu option clicked.");
            buttonHitByRay.OnClick();
        }
    }
}
