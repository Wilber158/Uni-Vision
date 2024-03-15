using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNavigationTarget : MonoBehaviour {

    [SerializeField]
    private Camera topDownCamera;
    [SerializeField]
    private GameObject navTargetObject;

    private UnityEngine.AI.NavMeshPath path;
    private LineRenderer line;

    private bool lineToggle = false;

    private void Start ()
    {
        path = new UnityEngine.AI.NavMeshPath();
        line = transform.GetComponent<LineRenderer>();

    }

    private void Update ()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            lineToggle = !lineToggle;
        }

        if (lineToggle)
        {
            UnityEngine.AI.NavMesh.CalculatePath(transform.position, navTargetObject.transform.position, UnityEngine.AI.NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            line.enabled = true;
        }
    }



}