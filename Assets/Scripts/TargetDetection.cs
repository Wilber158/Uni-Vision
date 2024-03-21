using UnityEngine;

public class TargetLookChecker : MonoBehaviour
{
    public Transform userCamera; // Assign your AR camera
    public Transform[] targets; // Assign your target transforms
    public float maxAngle = 10f; // Max angle to be considered "looking at"
    public float maxDistance = 5f; // Max distance to be considered "within range"

    private void Update()
    {
        foreach (var target in targets)
        {
            CheckTarget(userCamera, target);
        }
    }

    private void CheckTarget(Transform userCamera, Transform target)
    {
        Vector3 directionToTarget = target.position - userCamera.position;
        float angle = Vector3.Angle(userCamera.forward, directionToTarget);
        float distance = directionToTarget.magnitude;

        if (angle <= maxAngle && distance <= maxDistance)
        {
            Debug.Log("User is looking at target: " + target.name);
            // Perform actions when the user is looking at the target within the range
        }
    }
}
