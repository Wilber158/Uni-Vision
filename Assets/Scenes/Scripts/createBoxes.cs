using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;

namespace MyNamespace
{
    public class CreateBoxes : MonoBehaviour
    {
        [SerializeField] private GameObject content; // Assign this in the inspector to avoid runtime errors
        [SerializeField] private TMP_FontAsset fontAsset; // Also assign this in the inspector

        private void Start()
        {
            if (content == null)
            {
                Debug.LogError("Content GameObject is not assigned in the inspector.");
                return;
            }

            if (fontAsset == null)
            {
                Debug.LogError("Font asset is not assigned in the inspector.");
                return;
            }
        }

        public void UpdateBoxes(string targetName)
        {
            // Ensure existing boxes are destroyed before creating new ones
            DestroyExistingBoxes();

            // Attempt to load the scheduled data file for the current target
            string scheduleFilePath = $"Resources/{targetName}_schedule.txt";
            TextAsset textAsset = Resources.Load<TextAsset>(scheduleFilePath);

            if (textAsset == null)
            {
                Debug.LogError($"Failed to load the schedule text file for: {targetName}");
                return;
            }

            CreateBoxesFromSchedule(textAsset.text);
        }

        private void CreateBoxesFromSchedule(string scheduleData)
        {
            string[] scheduleLines = scheduleData.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (scheduleLines.Length == 0)
            {
                Debug.LogError("Schedule file is empty or not formatted correctly.");
                return;
            }

            for (int i = 0; i < scheduleLines.Length; i += 2)
            {
                string className = scheduleLines[i];
                string classTime = scheduleLines[i + 1];
                CreateBox(className, classTime, i);
            }
        }

        private void CreateBox(string className, string classTime, int index)
        {
            GameObject box = new GameObject($"Box_{index}");
            box.transform.SetParent(content.transform, false);
            Image image = box.AddComponent<Image>();
            image.color = new Color(0.99f, 0.99f, 0.99f); // Light grey

            RectTransform rectTransform = box.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(2213, 657);

            GameObject textObject = new GameObject("Text");
            textObject.transform.SetParent(box.transform, false);

            TextMeshProUGUI textMeshPro = textObject.AddComponent<TextMeshProUGUI>();
            textMeshPro.text = $"{className}\n{classTime}";
            textMeshPro.font = fontAsset;
            textMeshPro.fontSize = 140;
            textMeshPro.color = Color.black;
            textMeshPro.alignment = TextAlignmentOptions.Center;

            RectTransform textRectTransform = textObject.GetComponent<RectTransform>();
            textRectTransform.sizeDelta = new Vector2(2213, 657);
        }

        private void DestroyExistingBoxes()
        {
            foreach (Transform child in content.transform)
            {
                Destroy(child.gameObject);
            }
            Debug.Log("All existing boxes have been destroyed.");
        }
    }
}
