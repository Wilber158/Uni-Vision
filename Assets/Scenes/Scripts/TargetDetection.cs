using MyNamespace;
using System.IO;
using UnityEngine;

public class TargetLookChecker : MonoBehaviour
{
    public Transform userCamera; // Assign your AR camera
    public Transform[] targets; // Assign your target transforms
    public float maxAngle = 90f; // Max angle to be considered "looking at" (changed to 90 degrees)
    public float maxDistance = 0.2f; // Max distance to be considered "within range" (adjust as needed)

    // Reference to the SeleniumExample script
    public SeleniumExample scrapper;
    public CanvasCentering canvasCentering;
    public AddTextToTextMeshPro titleModify;
    public createBoxes bCreate;
    // Private backing field for the target name
    private string _targetName;
    private string _lastTargetName; // Track the last target name

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

        bool targetChanged = false; // Flag to track if target has changed

        foreach (var target in targets)
        {
            string targetName = CheckTarget(userCamera, target);
            if (!string.IsNullOrEmpty(targetName))
            {
                // Set the target name
                TargetName = targetName;
                // If target name has changed since last check, log it
                if (TargetName != _lastTargetName)
                {
                    _lastTargetName = TargetName;
                    targetChanged = true;
                }
            }
        }

        // If target hasn't changed since last check, don't spam messages
        if (targetChanged)
        {
            Debug.Log("Target Name: " + TargetName);
            {
                string targetNameWithSpace = TargetName.Insert(3, " ");

                titleModify.ChangeText(targetNameWithSpace);
                canvasCentering.ToggleCanvasVisibility();

                try
                {
                    scrapper.TriggerDatabaseData(TargetName);

                }
                catch
                {
                    string filePath = Path.Combine(Application.dataPath, "Resources", "schedule.txt");
                    if (File.Exists(filePath))
                    {
                        // Delete the file if it exists
                        File.Delete(filePath);
                        Debug.Log("Doesn't work");
                    }
                    GameObject content = GameObject.Find("content");
                    bCreate.DestroyExistingBoxes(content);
                }
                bCreate.boxCreation();
            }
        }
    }
    private string CheckTarget(Transform userCamera, Transform target)
    {
        Vector3 directionToTarget = target.position - userCamera.position;
        float angle = Vector3.Angle(userCamera.forward, directionToTarget);
        float distance = directionToTarget.magnitude;

        // Determine if the target is within range and angle is less than or equal to maxAngle
        if (angle <= maxAngle && distance <= maxDistance)
        {
            return target.name;
        }

        return null;
    }
}