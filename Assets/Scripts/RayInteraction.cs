using UnityEngine;

public class RayInteraction : MonoBehaviour
{
    public OVRInput.RawButton shootingButton;

    public Transform rayOrigin;
    public float maxLineDistance;
    // public GameObject rayPrefab;
    public LineRenderer lineRenderer;

    public LayerMask mask;

    RaycastHit currentHit;
    bool hasHit;

    void Start()
    {
        //LineRenderer lineRenderer = Instantiate(rayPrefab).GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        Vector3 lineEnd = rayOrigin.position + rayOrigin.forward * maxLineDistance;
        
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out currentHit, maxLineDistance, mask))
        {
            hasHit = true;
            lineEnd = currentHit.point;

        }
        else
        {
            hasHit = false;
        }
        //Debug.Log($"Line renderer: {lineRenderer}");
        //Debug.Log($" lineEnd: {lineEnd}");
        //Debug.Log($" rayOrigin: {rayOrigin}");
        lineRenderer.SetPosition(0, rayOrigin.position);
        lineRenderer.SetPosition(1, lineEnd);

        if (OVRInput.GetDown(shootingButton))
        {
            SelectObject();
        }
    }

    void SelectObject()
    {
        if (!hasHit)
            return;

        // for moving in xz-plane
        FurnitureVisualization visualization = currentHit.transform.GetComponent<FurnitureVisualization>();

        if (visualization != null && visualization.Moveable) 
        {
            // DO the move
            Vector3 newPos = Vector3.zero;//TODO set the new position somehow
            visualization.transform.position = newPos;
        }
    }
}
