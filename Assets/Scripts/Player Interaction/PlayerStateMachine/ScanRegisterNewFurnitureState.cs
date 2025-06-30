using UnityEngine;
using UnityEngine.UIElements;

public class ScanRegisterNewFurnitureState : PlayerControllerInteractionState
{
    // tis state is similar to what meta has, it allows to create new bounding boxes with our controllers

    //maybe have a small menu on the left open that allows us to switch between create new furniture and add walls mode 
    // add walls is disabled in one test instance?

    CreateBoundingBoxVisualisation createBoxVisualization;

    Vector3 boxPoint1;
    Vector3 boxPoint2;
    Vector3 boxPoint3;
    Vector3 boxPoint4;

    Plane planeFromPoint1;

    MeshFilter meshDuringCreation;

    enum CreateBoxState
    {
        FirstPointGround,
        SecondPointGround,
        ThirdPointPerpendicular,
        FourthPointHeight
    }

    CreateBoxState createBoxState;

    Color lineRendererColorBefore;

    public override void OnStateEnter()
    {
        createBoxState = CreateBoxState.FirstPointGround;

        // set up hand menu
        refs.scanAddObjectsMenu.gameObject.SetActive(true);
        refs.scanAddObjectsMenu.OnCancelClickedCallback += OnCancelAddBoxClicked;

        // change color of line renderer
        lineRendererColorBefore = refs.lineRenderer.startColor;

        refs.lineRenderer.startColor = config.addNewBoxScanSelectionLineColor;
        refs.lineRenderer.endColor = config.addNewBoxScanSelectionLineColor;

        createBoxVisualization = GameObject.Instantiate(refs.createNewBoundingBoxVisualizationPrefab).GetComponent<CreateBoundingBoxVisualisation>();

    }

    public override void OnStateExit()
    {
        // deregister  hand menu
        refs.scanAddObjectsMenu.gameObject.SetActive(false);
        refs.scanAddObjectsMenu.OnCancelClickedCallback -= OnCancelAddBoxClicked;


        refs.lineRenderer.startColor = lineRendererColorBefore;
        refs.lineRenderer.endColor = lineRendererColorBefore;

        GameObject.Destroy(createBoxVisualization.gameObject);
    }

    public override void UpdateState()
    {
        createBoxVisualization.currentTargetSphere.gameObject.SetActive(false);

        HandleRightHandRay(RaycastType.OnlyHitUi);
        HandleRayVisuals();

        HandleUiInteraction();

        HandlePlaceBoundingBoxPoint();

        if (OVRInput.GetDown(config.generalReturnButton))
        {
            sm.SetState(sm.scanSelection);
        }
    }

    void OnCancelAddBoxClicked()
    {
        sm.SetState(sm.scanSelection);
    }


    /// <summary>
    /// Here the main logic happens, its a bit dirt, but working ok
    /// </summary>
    void HandlePlaceBoundingBoxPoint()
    {
        if (runtimeData.furnitureHitByRay != null)
        {
            return;
        }


        // Do Rayast first
        Ray ray = new Ray(refs.rayOrigin.transform.position, refs.rayOrigin.transform.forward);
        // RaycastHit hit;

        runtimeData.raycastEnd = refs.rayOrigin.position + refs.rayOrigin.forward * config.maxRaycastDistance;
        runtimeData.raycastWasSuccessfull = Physics.Raycast(ray, out runtimeData.raycastHitInfo, config.maxRaycastDistance, config.createNewBoundingBoxUniversalGround);



        switch (createBoxState)
        {
            case CreateBoxState.FirstPointGround:
                {
                    if (runtimeData.raycastWasSuccessfull)
                    {

                        runtimeData.raycastEnd = runtimeData.raycastHitInfo.point;
                        createBoxVisualization.currentTargetSphere.gameObject.SetActive(true);

                        if (OVRInput.GetDown(config.createBoundingBoxPointButton))
                        {
                            createBoxState = CreateBoxState.SecondPointGround;
                            boxPoint1 = runtimeData.raycastEnd;
                            planeFromPoint1 = new Plane(Vector3.up, boxPoint1);
                        }


                        createBoxVisualization.currentTargetSphere.transform.position = runtimeData.raycastEnd;
                        createBoxVisualization.lineRenderer.positionCount = 1;
                        createBoxVisualization.lineRenderer.SetPositions(new Vector3[] { runtimeData.raycastEnd });
                    }

                    break;
                }
            case CreateBoxState.SecondPointGround:
                {
                    // custom intersect with the start placing new ground plane
                    Vector3 intersectPoint = Vector3.zero;

                    float enter = 0.0f;
                    if (planeFromPoint1.Raycast(ray, out enter))
                    {
                        // The ray intersects the plane
                        intersectPoint = ray.origin + ray.direction * enter;
                    }


                    runtimeData.raycastEnd = intersectPoint;
                    createBoxVisualization.currentTargetSphere.gameObject.SetActive(true);

                    if (OVRInput.GetDown(config.createBoundingBoxPointButton))
                    {
                        createBoxState = CreateBoxState.ThirdPointPerpendicular;
                        boxPoint2 = runtimeData.raycastEnd;
                    }

                    createBoxVisualization.currentTargetSphere.transform.position = runtimeData.raycastEnd;
                    createBoxVisualization.lineRenderer.positionCount = 2;
                    createBoxVisualization.lineRenderer.SetPositions(new Vector3[] { boxPoint1, runtimeData.raycastEnd });


                    break;
                }
            case CreateBoxState.ThirdPointPerpendicular:
                {
                    // custom intersect with the start placing new ground plane
                    Vector3 intersectPoint = Vector3.zero;

                    float enter = 0.0f;
                    if (planeFromPoint1.Raycast(ray, out enter))
                    {
                        // The ray intersects the plane
                        intersectPoint = ray.origin + ray.direction * enter;
                    }


                    runtimeData.raycastEnd = intersectPoint;


                    createBoxVisualization.currentTargetSphere.gameObject.SetActive(true);

                    Vector3 directionSoFar = (boxPoint2 - boxPoint1).normalized;
                    Vector3 perpendicularLine = Vector3.Cross(Vector3.up, directionSoFar).normalized;

                    // Project hit point onto the perpendicular direction
                    Vector3 origin = boxPoint2;
                    Vector3 toHit = runtimeData.raycastEnd - origin;
                    float distance = Vector3.Dot(toHit, perpendicularLine);
                    Vector3 projectedPoint = origin + perpendicularLine * distance;


                    if (OVRInput.GetDown(config.createBoundingBoxPointButton))
                    {
                        createBoxState = CreateBoxState.FourthPointHeight;
                        boxPoint3 = projectedPoint;

                        GameObject meshobj = new GameObject("temp volume box");
                        meshDuringCreation = meshobj.AddComponent<MeshFilter>();
                        MeshRenderer meshRend = meshobj.AddComponent<MeshRenderer>();
                        meshRend.material = config.placeholderNewFurnitureScannedMaterial;


                    }


                    createBoxVisualization.currentTargetSphere.transform.position = projectedPoint;
                    createBoxVisualization.lineRenderer.positionCount = 3;
                    createBoxVisualization.lineRenderer.SetPositions(new Vector3[] { boxPoint1, boxPoint2, projectedPoint });



                    break;
                }
            case CreateBoxState.FourthPointHeight:
                {
                    // Constrain point vertically above boxPoint3

                    Plane invisiblePlane = new Plane(-ray.direction, boxPoint3);
                    Vector3 verticalIntersectPoint = Vector3.zero;

                    float enter = 0.0f;
                    if (invisiblePlane.Raycast(ray, out enter))
                    {
                        // The ray intersects the plane
                        verticalIntersectPoint = ray.origin + ray.direction * enter;
                    }

                    verticalIntersectPoint.x = boxPoint3.x;
                    verticalIntersectPoint.z = boxPoint3.z;

                    verticalIntersectPoint.y = Mathf.Max(verticalIntersectPoint.y, boxPoint1.y + 0.1f);

                    CreateTempMeshBasedOnPoints(boxPoint1, boxPoint2, boxPoint3, verticalIntersectPoint);


                    createBoxVisualization.currentTargetSphere.gameObject.SetActive(true);
                    createBoxVisualization.currentTargetSphere.transform.position = verticalIntersectPoint;
                    createBoxVisualization.lineRenderer.positionCount = 4;
                    createBoxVisualization.lineRenderer.SetPositions(new Vector3[] { boxPoint1, boxPoint2, boxPoint3, verticalIntersectPoint });


                    if (OVRInput.GetDown(config.createBoundingBoxPointButton))
                    {
                        GameObject.Destroy(meshDuringCreation.gameObject);

                        // finito
                        boxPoint4 = verticalIntersectPoint;
                        CreateMeshAndDataBasedOnPoints(boxPoint1, boxPoint2, boxPoint3, boxPoint4);
                        sm.SetState(sm.scanSelection);
                    }




                    break;
                }
        }
    }

    void CreateTempMeshBasedOnPoints(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point7)
    {
        Vector3 objectDirection = (point2 - point1).normalized;
        Quaternion objectDirectionRot = Quaternion.LookRotation(objectDirection);
        Vector3 point4 = (point1 + point3 - point2); // point 5 is above of the cube
        Vector3 centroid = (point1 + point2 + point3 + point4) / 4;

        Vector3[] worldSpaceVertices = new Vector3[8]
       {
            point1,
            point2,
            point3,
            point4,
            new Vector3(point1.x,point7.y,point1.z),
            new Vector3(point2.x,point7.y,point2.z),
            point7,
            new Vector3(point4.x,point7.y,point4.z),
       };


        //Create Mesh
        Mesh mesh = new Mesh();
        // Convert world space vertices to local space
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(objectDirectionRot);
        Vector3[] localVertices = new Vector3[worldSpaceVertices.Length];
        for (int i = 0; i < worldSpaceVertices.Length; i++)
        {

            Vector3 translatedPoint = worldSpaceVertices[i] - centroid;
            // InverseTransformPoint to convert from world space to local space manually
            localVertices[i] = rotationMatrix.inverse.MultiplyPoint(translatedPoint);
        }

        mesh.vertices = localVertices;

        int[] triangles = new int[]
       {
           // bottom
            0, 1, 2,
            0, 2, 3,   
    
            // top
            4, 5, 6,
            4, 6, 7,   

           // front
            0, 4, 7,
            0, 7, 3,   

          // back
            1, 5, 6,
            1, 6, 2,   

            // left
            0, 4, 5,
            0, 5, 1,   

            // right
            3, 7, 6,
            3, 6, 2
       };
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshDuringCreation.mesh = mesh;
        meshDuringCreation.transform.position = centroid;
        meshDuringCreation.transform.rotation = objectDirectionRot;



    }

    void CreateMeshAndDataBasedOnPoints(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point7)
    {
        Vector3 objectDirection = (point2 - point1).normalized;
        Vector3 point4 = (point1 + point3 - point2); // point 5 is above of the cube
        Vector3 centroid = (point1 + point2 + point3 + point4) / 4;


        FurnitureData newData = new FurnitureData();

        //newData.posInRoom = centroid;
        newData.rotInRoom = Quaternion.LookRotation(objectDirection);
        newData.label = FurnitureLabel.OTHER;

        // calculate all vertices in worldspace
        Vector3[] worldSpaceVertices = new Vector3[8]
        {
            point1,
            point2,
            point3,
            point4,
            new Vector3(point1.x,point7.y,point1.z),
            new Vector3(point2.x,point7.y,point2.z),
            point7,
            new Vector3(point4.x,point7.y,point4.z),
        };




        //Create Mesh
        Mesh mesh = new Mesh();
        // Convert world space vertices to local space
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(newData.rotInRoom);
        Vector3[] localVertices = new Vector3[worldSpaceVertices.Length];
        for (int i = 0; i < worldSpaceVertices.Length; i++)
        {

            Vector3 translatedPoint = worldSpaceVertices[i] - centroid;
            // InverseTransformPoint to convert from world space to local space manually
            localVertices[i] = rotationMatrix.inverse.MultiplyPoint(translatedPoint);
        }

        mesh.vertices = localVertices;

        int[] triangles = new int[]
        {
           // bottom
            0, 1, 2,   
            0, 2, 3,   
    
            // top
            4, 5, 6,   
            4, 6, 7,   

           // front
            0, 4, 7,   
            0, 7, 3,   

          // back
            1, 5, 6,   
            1, 6, 2,   

            // left
            0, 4, 5,   
            0, 5, 1,   

            // right
            3, 7, 6,   
            3, 6, 2   
        };


        mesh.triangles = triangles;
        // mesh.normals = normals;
        mesh.RecalculateNormals();

        newData.meshData = new MeshSaveData(mesh);

        refs.roomManager.AddFurnitureToCurrentVisualization(newData, centroid);

        // rooms manager add the newly added data
    }
}