using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;
public class SetNavigationTarget : MonoBehaviour {

    [SerializeField]
    private TMP_Dropdown navigationTargetDropdown;

    [SerializeField]
    private List<Target> navigationTargetObjects = new List<Target>();
    private UnityEngine.AI.NavMeshPath path;
    private LineRenderer line;
    private Vector3 targetPosition = Vector3.zero;

    private bool lineToggle = false;

   private void Start ()
{
    path = new UnityEngine.AI.NavMeshPath();
    line = transform.GetComponent<LineRenderer>();
}

// Update is called once per frame
private void Update ()
{
    if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
    {
        lineToggle = !lineToggle;
    }

    if (lineToggle && targetPosition != Vector3.zero) 
    {
        UnityEngine.AI.NavMesh.CalculatePath(transform.position, targetPosition, UnityEngine.AI.NavMesh.AllAreas, path);
        line.positionCount = path.corners.Length;
        line.SetPositions(path.corners);
        line.enabled = true;
    }
    else
    {
        line.enabled = false;
    }
}

public void SetCurrentNavigationTarget(int selectedValue){
    targetPosition = Vector3.zero;
    string targetName = navigationTargetDropdown.options[selectedValue].text;
    Target currentTarget = navigationTargetObjects.Find(x => x.Name.Equals(targetName));
    if(currentTarget != null){
        targetPosition = currentTarget.PositionObject.transform.position;
    }
}
}

