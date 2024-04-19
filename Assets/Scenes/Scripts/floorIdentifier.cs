using UnityEngine;

public class FloorIdentifier : MonoBehaviour
{
    public int floorNumber;  // Public variable to set in the inspector for each floor

    private void OnTriggerEnter(Collider other)
    {
        FloorManager manager = other.GetComponentInParent<FloorManager>();
        if (manager != null)
        {
            manager.SetCurrentFloor(floorNumber);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FloorManager manager = other.GetComponentInParent<FloorManager>();
        if (manager != null)
        {
            manager.ClearCurrentFloor();
        }
    }
}
