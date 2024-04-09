using UnityEngine;
using UnityEngine.AI; // For navigation
using TMPro; // For TMP_Dropdown

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown navigationTargetDropdown;
    private NavMeshPath path;
    private LineRenderer line;
    private Vector3 targetPosition = Vector3.zero;
    private bool lineToggle = false;
    private Transform userIndicatorTransform;


    private void Start()
    {
        path = new NavMeshPath();
        line = GetComponent<LineRenderer>();

        if (transform.childCount > 0)
        {
            userIndicatorTransform = transform.GetChild(0);
        }
        else
        {
            Debug.LogError("User indicator child not found, using parent transform instead.");
            userIndicatorTransform = transform; // Fallback to using the parent's transform
        }
        
        navigationTargetDropdown.onValueChanged.AddListener(SetCurrentNavigationTarget);
    }

    private void Update()
    {
        // Toggle line drawing on touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            lineToggle = !lineToggle;
        }

        // Continuously update the line if it's enabled
        if (lineToggle)
        {
            UpdateNavigationLine();
        }
        else
        {
            line.enabled = false;
        }
    }

    private void UpdateNavigationLine()
    {
        if (targetPosition != Vector3.zero) // Ensure there's a valid target position
        {
            Debug.Log("Current user position: " + transform.position + " Target position: " + targetPosition);
            // Calculate path from current position to target position
            if (NavMesh.CalculatePath(userIndicatorTransform.position, targetPosition, NavMesh.AllAreas, path))
            {
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    // Path is complete, update the line renderer
                    line.positionCount = path.corners.Length;
                    line.SetPositions(path.corners);
                    line.enabled = true;
                }
                else
                {
                    // Handle incomplete or invalid path
                    line.enabled = false;
                }
            }
        }
    }

    public void SetCurrentNavigationTarget(int selectedValue)
    {
        if (selectedValue < 0 || selectedValue >= DynamicDropdownPopulator.navigationTargetObjects.Count) return;

        // Update target position based on dropdown selection
        var target = DynamicDropdownPopulator.navigationTargetObjects[selectedValue];
        targetPosition = target.PositionObject.position;

        // Optionally, force line update when new target is selected
        if (lineToggle) UpdateNavigationLine();

        Debug.Log($"Drop Down selected Target position set to: {targetPosition}");
    }
}
