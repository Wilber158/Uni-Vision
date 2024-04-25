using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using MyNamespace;

namespace MyNamespace
{
    public class TargetLookChecker : MonoBehaviour
    {
        private Dictionary<string, List<string>> eventData; // Declaration of eventData

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
        public CreateBoxes createBoxes;

        private Transform[] targets; // Populated at runtime
        private string lastTargetName; // Track the last target name

        private void Start()
        {
            if (Targets == null)
            {
                Debug.LogError("Targets GameObject is not assigned!");
                return;
            }

            PopulateTargets();
            destinationButton.image.color = normalColor; // Set button to normal color initially
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
                    if (lastTargetName != target.name)
                    {
                        lastTargetName = target.name;
                        Debug.Log($"New target reached: {lastTargetName}");
                        ProcessTargetChange(lastTargetName);
                    }
                    isAnyTargetFocused = true;
                    break; // Exit loop once the target is processed
                }
            }

            if (!isAnyTargetFocused && lastTargetName != null)
            {
                ResetFocus();
            }
        }

        private bool CheckTarget(Transform userCamera, Transform target)
        {
            Vector3 directionToTarget = target.position - userCamera.position;
            float angle = Vector3.Angle(userCamera.forward, directionToTarget);
            float distance = Vector3.Distance(userCamera.position, target.position);

            return distance <= maxDistance && angle <= maxAngle;
        }

        private void ProcessTargetChange(string targetName)
        {
            destinationButton.image.color = reachedColor;
            titleModify.ChangeText(targetName); // Update UI text
            eventData = scrapper.RetrieveEventDataFromDatabase(targetName);
            createBoxes.UpdateBoxes(eventData); // Update the boxes only after the data is processed
        }

        private void ResetFocus()
        {
            destinationButton.image.color = normalColor;
            lastTargetName = null; // Reset the last target name as no target is focused
            Debug.Log("No target is currently focused. Resetting color.");
        }
    }
}