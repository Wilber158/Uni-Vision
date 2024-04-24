using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

namespace MyNamespace
{
    public class createBoxes : MonoBehaviour
    {
        public void boxCreation()
        {
            // Get the Canvas to be the parent of the boxes
            GameObject content = GameObject.Find("content");
            if (content == null)
            {
                Debug.LogError("Failed to find 'content' GameObject.");
                return;
            }
            DestroyExistingBoxes(content);

            // Load the default font asset for TextMeshPro
            TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/ARIAL SDF");
            if (fontAsset == null)
            {
                Debug.LogError("Failed to load font asset.");
            }

            TextAsset textAsset = Resources.Load<TextAsset>("schedule");

            if (textAsset == null) {
                    Debug.LogError("Failed to load the schedule text file.");
                    return;
            
            
            }
            // Split the text of the schedule file into lines
            string[] scheduleLines = textAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (scheduleLines.Length == 0)
            {
                Debug.LogError("Schedule file is empty or not formatted correctly.");
                return;
            }


            // Loop to create the specified number of boxes
            for (int i = 0; i < scheduleLines.Length - 1; i += 2)
            {
                string className = scheduleLines[i];
                string classTime = scheduleLines[i + 1];

                Debug.Log($"Creating box for class: {className} at {classTime}");

                // Create a new GameObject for the box
                GameObject box = new GameObject("Box" + i);

                // Set the parent of the box to the Canvas
                box.transform.SetParent(content.transform, false);

                // Add an Image component to the box
                Image image = box.AddComponent<Image>();
                image.color = new Color(253f / 255f, 253f / 255f, 253f / 255f); // Light grey

                // Set the size and position of the box
                RectTransform rectTransform = box.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(2213, 657);

                // Create a new GameObject for the text
                GameObject textObject = new GameObject("Text" + i);
                textObject.transform.SetParent(box.transform, false);

                // Add a TextMeshProUGUI component to the text GameObject
                TextMeshProUGUI textMeshPro = textObject.AddComponent<TextMeshProUGUI>();
                textMeshPro.text = className + "\n" + classTime;
                textMeshPro.font = fontAsset;
                textMeshPro.fontSize = 140;
                textMeshPro.color = Color.black;
                textMeshPro.alignment = TextAlignmentOptions.Center;

                RectTransform textRectTransform = textObject.GetComponent<RectTransform>();
                textRectTransform.sizeDelta = new Vector2(2213, 657);
            }

            Debug.Log("All boxes created successfully.");
        }

        public void DestroyExistingBoxes(GameObject parent)
        {
            Debug.Log($"Destroying existing boxes under parent: {parent.name}");
            while (parent.transform.childCount > 0)
            {
                Transform child = parent.transform.GetChild(0);
                try
                {
                    DestroyImmediate(child.gameObject);
                    Debug.Log("Destroyed box: " + child.name);
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to destroy child: " + e.Message);
                }
            }
        }
    }
}
