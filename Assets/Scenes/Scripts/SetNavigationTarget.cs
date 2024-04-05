using System.Collections.Generic;
using UnityEngine;
public class SetNavigationTarget : MonoBehaviour
{

    [SerializeField]
    private Camera topDownCamera;
    [SerializeField]
    private GameObject navTargetObject;

    private UnityEngine.AI.NavMeshPath path;
    private LineRenderer line;

    private bool lineToggle = false;

    private void Start()
    {
        path = new UnityEngine.AI.NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {/*
    Debug.Log("Name of current object is: " + transform.name);
    Debug.Log("Name of the target object is: " + navTargetObject.name);
    if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
    {
        lineToggle = !lineToggle;
    }

    if (lineToggle)
    {
        Vector3 spherePosition = transform.GetChild(0).position;
        Debug.Log("Position of the sphere is: " + spherePosition);
        Debug.Log("Position of the \"player\" is: " + transform.position);
        UnityEngine.AI.NavMesh.CalculatePath(spherePosition, navTargetObject.transform.position, UnityEngine.AI.NavMesh.AllAreas, path);
        line.positionCount = path.corners.Length;
        line.SetPositions(path.corners);
        line.enabled = true;
    }
    else
    {
        line.enabled = false;
    }*/
}
}