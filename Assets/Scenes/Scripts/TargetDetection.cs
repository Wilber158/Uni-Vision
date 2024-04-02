using UnityEngine;

public class TargetLookChecker : MonoBehaviour
{
    public Transform userCamera; // Assign your AR camera
    public Transform[] targets; // Assign your target transforms
    public float maxAngle = 10f; // Max angle to be considered "looking at"
    public float maxDistance = 5f; // Max distance to be considered "within range"

    // Private backing field for the target name
    private string _targetName;

    // Property to access the target name
    public string TargetName
    {
        get { return _targetName; }
        private set { _targetName = value; }
    }

    private void Update()
    {
        // Reset target name
        TargetName = "";

        foreach (var target in targets)
        {
            string targetName = CheckTarget(userCamera, target);
            if (!string.IsNullOrEmpty(targetName))
            {
                // Set the target name
                TargetName = targetName;
                // Log the target name for demonstration
                Debug.Log("Target Name: " + TargetName);
                // Pass the target name to the Scraper script
                // Note: Uncomment and replace `scraperScript` with your actual reference to the Scraper script
                //if (scraperScript != null)
                //{
                //    scraperScript.SaveTargetName(TargetName);
                //}
                //else
                //{
                //    Debug.LogWarning("Scraper script reference is null. Make sure it is assigned.");
                //}
            }
        }
    }

    private string CheckTarget(Transform userCamera, Transform target)
    {
        Vector3 directionToTarget = target.position - userCamera.position;
        float angle = Vector3.Angle(userCamera.forward, directionToTarget);
        float distance = directionToTarget.magnitude;

        // Determine if the target is within range (uncomment if needed)
        //if (angle <= maxAngle && distance <= maxDistance)
        //{
        // Return the target name when the conditions are met
        return target.name;
        //}

        // Return null if the conditions are not met
        //return null;
    }
}
