using UnityEngine;

public class RayInteraction : MonoBehaviour
{
    public OVRInput.RawButton shootingButton;

    public Transform rayOrigin;
    public LineRenderer ray;
    public float maxLineDistance;
    public GameObject rayPrefab;
    private LineRenderer lineRenderer;

    public LayerMask mask;


    void Start()
    {
        LineRenderer lineRenderer = Instantiate(rayPrefab).GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        Vector3 lineEnd = rayOrigin.position + rayOrigin.forward * maxLineDistance;
        RaycastHit hit;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out hit, maxLineDistance, mask))
        {
            lineEnd = hit.point;

        }
        lineRenderer.SetPosition(0, rayOrigin.position);
        lineRenderer.SetPosition(1, lineEnd);

        if (OVRInput.GetDown(shootingButton))
        {
            SelectObject();
        }
    }

    void SelectObject()
    {
        // for moving in xz-plane
    }
}
