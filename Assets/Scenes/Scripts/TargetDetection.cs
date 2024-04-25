using MyNamespace;
using System.IO;
using UnityEngine;
using UnityEngine.UI; // Required for Button and Color manipulation

public class TargetLookChecker : MonoBehaviour
{
    [SerializeField] private GameObject Targets; // GameObject containing all targets as children
    public Transform userCamera; // Assign your AR camera
    public Transform[] targets; // Populated at runtime
    public float maxAngle = 90f; // Max angle for "looking at"
    public float maxDistance = 0.2f; // Max distance for "within range"

    // References to other components
    public SeleniumExample scrapper;
    public CanvasCentering canvasCentering;
    public AddTextToTextMeshPro titleModify;
    public createBoxes bCreate;

    // UI Button to change color
    [SerializeField] private Button destinationButton; // Button to change color
    public Color normalColor = Color.white; // Default color
    public Color reachedColor = Color.green; // Color when target is reached

    private string _lastTargetName; // Track the last target name

    private void Start()
    {
        if (Targets != null)
        {
            targets = new Transform[Targets.transform.childCount];
            for (int i = 0; i < Targets.transform.childCount; i++)
            {
                targets[i] = Targets.transform.GetChild(i);
            }
            destinationButton.image.color = normalColor; // Set button to normal color initially
        }
        else
        {
            Debug.LogError("Targets GameObject is not assigned!");
        }
    }

    private void Update()
    {
        foreach (var target in targets)
        {
            if (CheckTarget(userCamera, target))
            {
                if (_lastTargetName != target.name)
                {
                    _lastTargetName = target.name;
                    Debug.Log($"New target reached: {_lastTargetName}");
                    ProcessTargetChange();
                }
                return; // Exit loop once the target is processed
            }
        }

        // Reset to normal color if no targets are processed
        destinationButton.image.color = normalColor;
    }

    private bool CheckTarget(Transform userCamera, Transform target)
    {
        Vector3 directionToTarget = target.position - userCamera.position;
        float angle = Vector3.Angle(userCamera.forward, directionToTarget);
        float distance = directionToTarget.magnitude;

        // Check if within range and angle
        bool isTargetFocused = distance <= maxDistance && angle <= maxAngle;

        // Debugging to show which target is being checked and the result'
        if(isTargetFocused){
            //Debug.Log($"Checking target: {target.name} | Distance: {distance} | Angle: {angle} | Focused: {isTargetFocused}");
        }
        return isTargetFocused;
    }


    private void ProcessTargetChange()
    {
        // Change the button color to indicate the target has been reached
        destinationButton.image.color = reachedColor;
        Debug.Log($"Color changed to reachedColor for target: {_lastTargetName}");

        // Perform other actions like updating text or handling data
        titleModify.ChangeText(_lastTargetName); // Change the text to indicate the reached target
        bCreate.boxCreation(); // Create boxes or other visual feedback

        // Triggering data-related actions without affecting visibility
        try
        {
            scrapper.TriggerDatabaseData(_lastTargetName);
        }
        catch
        {
            HandleDataFailure();
        }
    }

    private void HandleDataFailure()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources", "schedule.txt");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Database access failed, schedule.txt deleted.");
        }
    }
}
