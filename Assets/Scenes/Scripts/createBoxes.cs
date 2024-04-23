using UnityEngine;
using TMPro;
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
            DestroyExistingBoxes(content);

            // Load the default font asset for TextMeshPro
            TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/ARIAL SDF");

            string filePath = Path.Combine(Application.dataPath, "Resources", "schedule.txt");

            // Read the content of the file
            string scheduleTextAsset = File.ReadAllText(filePath);

            // Split the text of the schedule file into lines
            string[] scheduleLines = scheduleTextAsset.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // Loop to create the specified number of boxes
            for (int i = 0; i < scheduleLines.Length - 1; i += 2)
            {
                string className = scheduleLines[i];
                string classTime = scheduleLines[i + 1];

                // Create a new GameObject for the box
                GameObject box = new GameObject("Box" + i);

                // Set the parent of the box to the Canvas
                box.transform.SetParent(content.transform, false);

                // Add an Image component to the box
                Image image = box.AddComponent<Image>();

                // Assign the box image to the Image component
                image.color = new Color(253f / 255f, 253f / 255f, 253f / 255f);

                // Set the size and position of the box
                RectTransform rectTransform = box.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(2213, 657); // Change the size as needed

                // Create a new GameObject for the text
                GameObject textObject = new GameObject("Text" + i);

                // Set the parent of the text to the box
                textObject.transform.SetParent(box.transform, false);

                // Add a TextMeshProUGUI component to the text GameObject
                TextMeshProUGUI textMeshPro = textObject.AddComponent<TextMeshProUGUI>();

                // Set the text content to the class and time from the schedule file
                textMeshPro.text = className + "\n" + classTime;

                // Set the font asset
                textMeshPro.font = fontAsset;

                // Set the font size
                textMeshPro.fontSize = 140; // Adjust font size as needed

                // Set the color of the text to black
                textMeshPro.color = Color.black;

                // Set the alignment
                textMeshPro.alignment = TextAlignmentOptions.Center;

                // Set the size of the text to match the size of the box
                RectTransform textRectTransform = textObject.GetComponent<RectTransform>();
                textRectTransform.sizeDelta = new Vector2(2213, 657); // Change the size as needed
            }

        }
        public void DestroyExistingBoxes(GameObject parent)
        {
            // Find all children of the parent
            while (parent.transform.childCount > 0)
            {
                // Get the first child
                Transform child = parent.transform.GetChild(0);

                // Destroy the child GameObject
                try
                {
                    DestroyImmediate(child.gameObject);
                }
                catch
                {
                    Debug.Log("error");
                }
            }
        }


    }
}
