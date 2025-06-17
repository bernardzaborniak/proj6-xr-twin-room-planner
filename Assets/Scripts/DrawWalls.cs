using System.Collections.Generic;
using Oculus.Interaction.PoseDetection.Debug.Editor.Generated;
using UnityEngine;

public class DrawWalls : MonoBehaviour
{
    public GameObject wallPrefab;
    public float wallThickness = 0.1f;
    public float snapDistance = 0.4f;
    public Material transparentMat;
    public GameObject rayOrigin;

    GameObject previewWall;
    LineRenderer lineRenderer;
    List<Vector3> wallPoints = new List<Vector3>();
    bool building = false;

    void Start()
    {
        GameObject lineVis = new GameObject("LineVis");
        lineRenderer = lineVis.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.red;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        if (!building)
        {
            return;
        }

        Ray detectionRay = new Ray(rayOrigin.transform.position, rayOrigin.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(detectionRay, out hit, 100f))
        {               
            lineRenderer.SetPosition(0, rayOrigin.transform.position);
            lineRenderer.SetPosition(1, hit.point);

            //Debug.Log(hit.point.x + " " + hit.point.z);

            if (wallPoints.Count > 0)
            {
                ShowPreviewWall(wallPoints[wallPoints.Count - 1], hit.point);
            }

            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                PlacePoint(hit.point);
            }
        }
    }

    // Place start and end point, if close enough to start wall snap to it and disable building
    void PlacePoint(Vector3 newPoint)
        {
            if (wallPoints.Count == 0)
            {
                wallPoints.Add(newPoint);
                return;
            }

            // Snap to start wall if close enough
            if (Vector3.Distance(newPoint, wallPoints[0]) < snapDistance && wallPoints.Count > 0)
            {
                CreateWall(wallPoints[wallPoints.Count - 1], wallPoints[0]);
                Destroy(previewWall);
                building = false;
                lineRenderer.enabled = false;
                return;
            }

            CreateWall(wallPoints[wallPoints.Count - 1], newPoint);
            wallPoints.Add(newPoint);
    }

    // Initiate building
    public void StartBuilding()
    {
        wallPoints.Clear();
        lineRenderer.enabled = true;
        building = true;
    }

    // Create wall by calling PositionWall()
    void CreateWall(Vector3 start, Vector3 end)
    {
        GameObject wall = Instantiate(wallPrefab);
        Debug.Log("Wall placed from " + start + " to " + end);
        PositionWall(wall, start, end);
    }

    // Handle the visualization of preview wall
    void ShowPreviewWall(Vector3 start, Vector3 end)
    {
        if (previewWall == null)
        {
            previewWall = Instantiate(wallPrefab);
            previewWall.GetComponent<Renderer>().material = transparentMat;
            Collider col = previewWall.GetComponent<Collider>();
        }
        PositionWall(previewWall, start, end);
    }

    // Position and scale wall between start and end point
    void PositionWall(GameObject wall, Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float length = new Vector3(direction.x, 0f, direction.z).magnitude;

        wall.SetActive(true);
        Vector3 center = (start + end) / 2f;
        center.y = wallPrefab.transform.localScale.y / 2;

        wall.transform.position = center;
        wall.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z), Vector3.up);
        wall.transform.localScale = new Vector3(wallThickness, 2f, length);
    }
}
