using UnityEngine;
using UnityEngine.AI; // For navigation
using TMPro; // For TMP_Dropdown
using System.Linq;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown navigationTargetDropdown;
    [SerializeField] private FloorManager floorManager;

    private NavMeshPath path;
    private LineRenderer line;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 lastPositionUpdate = Vector3.zero;
    private bool lineToggle = false;
    private Transform userIndicatorTransform;
    private float updateThreshold = 1f; // Threshold distance to update the path

    private void Start() {
        path = new NavMeshPath();
        line = GetComponent<LineRenderer>();
        userIndicatorTransform = transform.childCount > 0 ? transform.GetChild(0) : transform;

        navigationTargetDropdown.onValueChanged.AddListener(SetCurrentNavigationTarget);
        navigationTargetDropdown.value = 0;  // Set default value to the first index
        SetCurrentNavigationTarget(0);  // Initialize with the first target

        UpdateNavigationLine(); // Update line at start if necessary
    }

    private void Update() {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            lineToggle = !lineToggle;

        if (lineToggle && Vector3.Distance(userIndicatorTransform.position, lastPositionUpdate) > updateThreshold) {
            UpdateNavigationLine();
            lastPositionUpdate = userIndicatorTransform.position;
        } else if (!lineToggle) {
            line.enabled = false;
        }
    }

    private void UpdateNavigationLine() {
        if (targetPosition != Vector3.zero && NavMesh.CalculatePath(userIndicatorTransform.position, targetPosition, NavMesh.AllAreas, path)) {
            if (path.status == NavMeshPathStatus.PathComplete) {
                line.positionCount = path.corners.Length;
                line.SetPositions(path.corners);
                line.enabled = true;
            } else {
                line.enabled = false;
            }
        }
    }

    public void SetCurrentNavigationTarget(int selectedValue)
    {
        if (DynamicDropdownPopulator.Instance == null)
        {
            Debug.LogError("DynamicDropdownPopulator.Instance is null");
            return;
        }

        if (selectedValue < 0 || selectedValue >= DynamicDropdownPopulator.Instance.navigationTargetObjects.Count)
        {
            Debug.LogError("Selected value is out of range: " + selectedValue);
            return;
        }

        var target = DynamicDropdownPopulator.Instance.navigationTargetObjects[selectedValue];
        Debug.Log("Selected target: " + target.Name);

        if (floorManager == null)
        {
            Debug.LogError("FloorManager is not assigned in the inspector");
            return;
        }

        if (floorManager.currentFloor != target.FloorNumber)
        {
            Debug.Log("Target is on a different floor. Finding nearest stairwell...");
            var nearestStairwell = FindNearestStairwell(floorManager.currentFloor);

            if (nearestStairwell == null)
            {
                Debug.LogError("No nearest stairwell found");
                return;
            }

            Debug.Log("Nearest stairwell found: " + nearestStairwell.Name + " at position " + nearestStairwell.PositionObject.position);
            targetPosition = nearestStairwell.PositionObject.position;
        }
        else
        {
            Debug.Log("Target is on the same floor.");
            targetPosition = target.PositionObject.position;
        }

        if (lineToggle)
        {
            Debug.Log("Updating navigation line.");
            UpdateNavigationLine();
        }
    }

    private Target FindNearestStairwell(int currentFloor)
    {
        Debug.Log("Finding nearest stairwell on floor: " + currentFloor);

        var stairwells = DynamicDropdownPopulator.Instance.navigationTargetObjects
            .Where(t => t.IsStair && t.FloorNumber == currentFloor)
            .OrderBy(t => Vector3.Distance(userIndicatorTransform.position, t.PositionObject.position))
            .ToList();

        if (!stairwells.Any())
        {
            Debug.LogError("No stairwells found on floor: " + currentFloor);
            return null;
        }

        var nearestStairwell = stairwells.FirstOrDefault();
        Debug.Log("Nearest stairwell is " + nearestStairwell.Name);
        return nearestStairwell;
    }
}
