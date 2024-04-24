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
    private bool isFinalDestination; // To check if the current target is the final destination

    private void Start()
    {
        path = new NavMeshPath();
        line = GetComponent<LineRenderer>();
        userIndicatorTransform = transform.childCount > 0 ? transform.GetChild(0) : transform;

        navigationTargetDropdown.onValueChanged.AddListener((value) => SetCurrentNavigationTarget(value, true));
        navigationTargetDropdown.value = 0;  // Set default value to the first index
        SetCurrentNavigationTarget(0, true);  // Initialize with the first target
        destinationReachedGUI.SetActive(false); // Ensure the GUI is initially hidden

        // Subscribe to the floor change event
        floorManager.OnFloorChanged += (newFloor) => SetCurrentNavigationTarget(navigationTargetDropdown.value, false);
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        floorManager.OnFloorChanged -= (newFloor) => SetCurrentNavigationTarget(navigationTargetDropdown.value, false);
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetKeyDown(KeyCode.T))
        {
            lineToggle = !lineToggle;
            if (!lineToggle)
            {
                line.enabled = false;
                destinationText.text = ""; // Clear the text when line is not active
            }
        }

        if (lineToggle)
        {
            float distanceMoved = Vector3.Distance(userIndicatorTransform.position, lastPositionUpdate);
            if (distanceMoved > updateThreshold || !line.enabled)
            {
                lastPositionUpdate = userIndicatorTransform.position;
                UpdateNavigationLine();
                CheckProximityToTarget();
            }
        }
    }

    private void UpdateNavigationLine()
    {
        if (targetPosition != Vector3.zero && NavMesh.CalculatePath(userIndicatorTransform.position, targetPosition, NavMesh.AllAreas, path))
        {
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                line.positionCount = path.corners.Length;
                line.SetPositions(path.corners);
                line.enabled = true;
            }
            else
            {
                line.enabled = false;
            }
        }
    }

    private void SetCurrentNavigationTarget(int selectedValue, bool forceUpdate)
    {
        var target = DynamicDropdownPopulator.Instance.navigationTargetObjects[selectedValue];

        // Determine if the floor change or force update requires recalculating the path
        if (forceUpdate || floorManager.currentFloor != target.FloorNumber)
        {
            // Adjust the target based on the floor change
            if (floorManager.currentFloor != target.FloorNumber)
            {
                targetPosition = FindNearestStairwell(floorManager.currentFloor)?.PositionObject.position ?? target.PositionObject.position;
                isFinalDestination = false; // Mark as not final if it is a stairwell
            }
            else
            {
                targetPosition = target.PositionObject.position;
                isFinalDestination = !target.IsStair;
            }
            UpdateNavigationLine();
        }
        else if (!forceUpdate && floorManager.currentFloor == target.FloorNumber)
        {
            // This ensures that if we are already on the correct floor, we confirm it is the final destination.
            isFinalDestination = !target.IsStair;
        }

        allowUIAppear = true; // Always allow UI to appear again after an update
    }


    private void CheckProximityToTarget()
    {
        Debug.Log($"Checking proximity: position={userIndicatorTransform.position}, targetPosition={targetPosition}, isFinalDestination={isFinalDestination}");
        if (allowUIAppear && Vector3.Distance(userIndicatorTransform.position, targetPosition) < proximityThreshold && isFinalDestination)
        {
            Debug.Log("Destination reached, showing UI.");
            destinationText.text = "Reached: " + DynamicDropdownPopulator.Instance.navigationTargetObjects[navigationTargetDropdown.value].Name;
            destinationReachedGUI.SetActive(true); // Show the GUI when the destination is reached
        }
    }


    private Target FindNearestStairwell(int currentFloor)
    {
        return DynamicDropdownPopulator.Instance.navigationTargetObjects
            .Where(t => t.IsStair && t.FloorNumber == currentFloor)
            .OrderBy(t => Vector3.Distance(userIndicatorTransform.position, t.PositionObject.position))
            .FirstOrDefault();
    }

    public void ResetUIAppearance()
    {
        allowUIAppear = false;
        destinationReachedGUI.SetActive(false);
    }
}
