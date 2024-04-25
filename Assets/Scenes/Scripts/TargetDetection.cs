using MyNamespace;
using System.IO;
using UnityEngine;
using UnityEngine.UI; // Required for Button and Color manipulation

public class TargetLookChecker : MonoBehaviour
{
    [SerializeField] private GameObject Targets; // GameObject containing all targets as children
    [SerializeField] private Transform userCamera; // Assign your AR camera
    [SerializeField] private float maxAngle = 90f; // Max angle for "looking at"
    [SerializeField] private float maxDistance = 0.2f; // Max distance for "within range"
    [SerializeField] private Button destinationButton; // Button to change color
    [SerializeField] private Color normalColor = Color.white; // Default color
    [SerializeField] private Color reachedColor = Color.green; // Color when target is reached

    // References to other components
    public SeleniumExample scrapper;
    public CanvasCentering canvasCentering;
    public AddTextToTextMeshPro titleModify;
    public createBoxes bCreate;

    private Transform[] targets; // Populated at runtime
    private string _lastTargetName; // Track the last target name

    private void Start()
    {
        if (Targets != null)
        {
            PopulateTargets();
            destinationButton.image.color = normalColor; // Set button to normal color initially
        }
        else
        {
            Debug.LogError("Targets GameObject is not assigned!");
        }
    }

    private void PopulateTargets()
    {
        targets = new Transform[Targets.transform.childCount];
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = Targets.transform.GetChild(i);
        }
    }

    private void Update()
    {
        bool isAnyTargetFocused = false;

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
                isAnyTargetFocused = true;
                break; // Exit loop once the target is processed
            }
        }

        if (!isAnyTargetFocused && _lastTargetName != null)
        {
            // Reset to normal color if no targets are currently processed
            destinationButton.image.color = normalColor;
            _lastTargetName = null; // Reset the last target name as no target is focused
            Debug.Log("No target is currently focused. Resetting color.");
        }
    }


    private bool CheckTarget(Transform userCamera, Transform target)
    {
        Vector3 directionToTarget = target.position - userCamera.position;
        float angle = Vector3.Angle(userCamera.forward, directionToTarget);
        float distance = Vector3.Distance(userCamera.position, target.position);

        // Check if within range and angle
        return distance <= maxDistance && angle <= maxAngle;
    }

    private void ProcessTargetChange()
    {
        destinationButton.image.color = reachedColor;
        titleModify.ChangeText(_lastTargetName); // Update UI text
        bCreate.boxCreation(); // Trigger UI updates for new target

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
        string filePath = Path.Combine(Application.persistentDataPath, "schedule.txt");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.LogError("Database access failed, schedule.txt deleted.");
        }
    }
}
