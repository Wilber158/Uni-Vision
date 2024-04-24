using UnityEngine;
using TMPro;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private TMP_Text floorText; // UI text to display current floor
    public int currentFloor = -1; // Initialize with an invalid floor number

    // Define an event to notify subscribers of a floor change.
    public delegate void FloorChangeHandler(int newFloor);
    public event FloorChangeHandler OnFloorChanged;

    void Start()
    {
        CheckInitialFloor();
    }

    public void SetCurrentFloor(int floor)
    {
        if (currentFloor != floor) // Only update if there's an actual change
        {
            currentFloor = floor;
            UpdateFloorDisplay();
            OnFloorChanged?.Invoke(currentFloor); // Trigger the event
        }
    }

    public void ClearCurrentFloor()
    {
        currentFloor = -1;
        UpdateFloorDisplay();
        OnFloorChanged?.Invoke(currentFloor); // Trigger the event
    }

    private void UpdateFloorDisplay()
    {
        if (floorText != null)
        {
            floorText.text = currentFloor >= 0 ? "Floor: " + currentFloor : "In between floors";
        }
    }

    private void CheckInitialFloor()
    {
        Debug.Log("Checking initial floor...");
        int layerMask = LayerMask.GetMask("FloorLayer");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.1f, layerMask);
        foreach (Collider hitCollider in hitColliders)
        {
            FloorIdentifier floor = hitCollider.GetComponent<FloorIdentifier>();
            if (floor != null)
            {
                SetCurrentFloor(floor.floorNumber);
                break;
            }
        }
        if (currentFloor == -1) Debug.Log("No floor detected at start.");
    }
}
