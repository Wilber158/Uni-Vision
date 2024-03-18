using UnityEngine;
using UnityEngine.AI;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject navTargetObject; // Your target object

    private UnityEngine.AI.NavMeshPath path;
    private LineRenderer line;
    private bool lineToggle = false;

    private void Start ()
    {
        path = new UnityEngine.AI.NavMeshPath();
        line = GetComponent<LineRenderer>();
    }

    private void Update ()
    {
        Debug.Log("In Update.....");
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Debug.Log("What is going on???");
            lineToggle = !lineToggle;

            if (lineToggle) // Only try to calculate and display the path if lineToggle is enabled
            {
                Vector3 indicatorPosition = transform.position;
                NavMeshHit hit;

                // Check if the indicator's position is inside the NavMesh
                if (NavMesh.SamplePosition(indicatorPosition, out hit, 1.0f, NavMesh.AllAreas))
                {
                    indicatorPosition = hit.position; // Snap indicator to the nearest valid NavMesh position
                }
                else
                {
                    // If outside, you might want to handle this differently, e.g., disable path display
                    line.enabled = false;
                    return;
                }

                // Assuming navTargetObject is always within NavMesh bounds for simplicity
                NavMesh.CalculatePath(indicatorPosition, navTargetObject.transform.position, NavMesh.AllAreas, path);
                line.positionCount = path.corners.Length;
                line.SetPositions(path.corners);
                line.enabled = true;
            }
            else
            {
                line.enabled = false; // Disable the line if toggled off
            }
        }
    }
}
