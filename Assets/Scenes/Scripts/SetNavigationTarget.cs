using UnityEngine;
using UnityEngine.AI; // For navigation
using TMPro; // For TMP_Dropdown
using System.Linq;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown navigationTargetDropdown;
    [SerializeField] private FloorManager floorManager;
    [SerializeField] private TextMeshProUGUI destinationText; // Assign this in the Inspector
    [SerializeField] private GameObject destinationReachedGUI; // GUI GameObject to show upon arrival

    private NavMeshPath path;
    private LineRenderer line;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 lastPositionUpdate = Vector3.zero;
    private bool lineToggle = false;
    private bool allowUIAppear = true; // Flag to control when the UI can appear
    private Transform userIndicatorTransform;
    private float updateThreshold = 1f; // Threshold distance to update the path
    private float proximityThreshold = 3f; // Distance to consider "reached" the target

    private void Start() {
        path = new NavMeshPath();
        line = GetComponent<LineRenderer>();
        userIndicatorTransform = transform.childCount > 0 ? transform.GetChild(0) : transform;

        navigationTargetDropdown.onValueChanged.AddListener(SetCurrentNavigationTarget);
        navigationTargetDropdown.value = 0;  // Set default value to the first index
        SetCurrentNavigationTarget(0);  // Initialize with the first target
        destinationReachedGUI.SetActive(false); // Ensure the GUI is initially hidden
    }

    private void Update() {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            lineToggle = !lineToggle;
            if (!lineToggle) {
                line.enabled = false;
                destinationText.text = ""; // Clear the text when line is not active
            }
        }

        if (lineToggle) {
            float distanceMoved = Vector3.Distance(userIndicatorTransform.position, lastPositionUpdate);
            if (distanceMoved > updateThreshold || !line.enabled) {
                lastPositionUpdate = userIndicatorTransform.position;
                if (floorManager.currentFloor != DynamicDropdownPopulator.Instance.navigationTargetObjects[navigationTargetDropdown.value].FloorNumber) {
                    targetPosition = FindNearestStairwell(floorManager.currentFloor)?.PositionObject.position ?? targetPosition;
                }
                UpdateNavigationLine();
                CheckProximityToTarget();
            }
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

    public void SetCurrentNavigationTarget(int selectedValue) {
        if (DynamicDropdownPopulator.Instance == null) {
            Debug.LogError("DynamicDropdownPopulator.Instance is null");
            return;
        }

        if (selectedValue < 0 || selectedValue >= DynamicDropdownPopulator.Instance.navigationTargetObjects.Count) {
            Debug.LogError("Selected value is out of range: " + selectedValue);
            return;
        }

        var target = DynamicDropdownPopulator.Instance.navigationTargetObjects[selectedValue];
        Debug.Log("Selected target: " + target.Name);

        if (floorManager == null) {
            Debug.LogError("FloorManager is not assigned in the inspector");
            return;
        }

        targetPosition = target.PositionObject.position;
        allowUIAppear = true;  // Reset the flag to allow the UI to appear again
        UpdateNavigationLine();
    }

    private void CheckProximityToTarget() {
        if (allowUIAppear && Vector3.Distance(userIndicatorTransform.position, targetPosition) < proximityThreshold) {
            destinationText.text = "Reached: " + DynamicDropdownPopulator.Instance.navigationTargetObjects[navigationTargetDropdown.value].Name;
            destinationReachedGUI.SetActive(true); // Show the GUI when the destination is reached
        }
    }

    private Target FindNearestStairwell(int currentFloor) {
        Debug.Log("Finding nearest stairwell on floor: " + currentFloor);
        return DynamicDropdownPopulator.Instance.navigationTargetObjects
            .Where(t => t.IsStair && t.FloorNumber == currentFloor)
            .OrderBy(t => Vector3.Distance(userIndicatorTransform.position, t.PositionObject.position))
            .FirstOrDefault();
    }

    // Call this from the GUI back button to prevent reappearing until a new target is selected
    public void ResetUIAppearance() {
        allowUIAppear = false;
        destinationReachedGUI.SetActive(false);
    }
}
