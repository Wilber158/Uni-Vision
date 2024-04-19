using UnityEngine;
using TMPro; 

public class FloorManager : MonoBehaviour
{
    [SerializeField] private TMP_Text floorText; // UI text to display current floor
    public int currentFloor = -1; // Initialize with an invalid floor number


    void Start()
    {
        Debug.Log("Checking initial floor...");
        int layerMask = LayerMask.GetMask("FloorLayer");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.1f, layerMask);
        foreach (Collider hitCollider in hitColliders)
        {
            FloorIdentifier floor = hitCollider.GetComponent<FloorIdentifier>();
            if (floor != null)
            {
                currentFloor = floor.floorNumber;
                UpdateFloorDisplay();
                Debug.Log("Starting on Floor: " + floor.floorNumber);
                break;
            }
        }
        if (currentFloor == -1) Debug.Log("No floor detected at start.");
    }

    
    public void SetCurrentFloor(int floor)
    {
        currentFloor = floor;
        UpdateFloorDisplay();
    }

    public void ClearCurrentFloor()
    {
        currentFloor = -1;
        UpdateFloorDisplay();
    }

    private void UpdateFloorDisplay()
    {
        if (floorText != null)
        {
            if (currentFloor >= 0)
                floorText.text = "Floor: " + currentFloor;
            else
                floorText.text = "In between floors";
        }
    }
}
