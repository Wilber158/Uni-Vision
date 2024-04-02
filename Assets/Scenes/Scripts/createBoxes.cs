using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;

namespace MyNamespace
{
    public class createBoxes : MonoBehaviour
    {
        private Sprite boxSprite;

        void Start()
        {
            // Get the Canvas to be the parent of the boxes
            GameObject content = GameObject.Find("content");

            // Load the default font asset for TextMeshPro
            TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/ARIAL SDF");

            // Load the schedule file from the Resources folder
            TextAsset scheduleTextAsset = Resources.Load<TextAsset>("schedule");

            // Split the text of the schedule file into lines
            string[] scheduleLines = scheduleTextAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // Loop to create the specified number of boxes
            for (int i = 0; i < scheduleLines.Length-1; i += 2)
            {
                // Create a new GameObject for the box
                GameObject box = new GameObject("Box" + i);

                // Set the parent of the box to the Canvas
                box.transform.SetParent(content.transform, false);

                // Add an Image component to the box
                Image image = box.AddComponent<Image>();

                // Assign the box image to the Image component
                image.sprite = boxSprite;
                image.color = new Color(253f / 255f, 253f / 255f, 253f / 255f);

                // Set the size and position of the box
                RectTransform rectTransform = box.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(74, 22); // Change the size as needed

                // Create a new GameObject for the text
                GameObject textObject = new GameObject("Text" + i);

                // Set the parent of the text to the box
                textObject.transform.SetParent(box.transform, false);

                // Add a TextMeshProUGUI component to the text GameObject
                TextMeshProUGUI textMeshPro = textObject.AddComponent<TextMeshProUGUI>();

                // Set the text content to the class and time from the schedule file
                textMeshPro.text = scheduleLines[i] + "\n" + scheduleLines[i + 1];

                // Set the font asset
                textMeshPro.font = fontAsset;

                // Set the font size
                textMeshPro.fontSize = 5; // Adjust font size as needed

                // Set the color of the text to black
                textMeshPro.color = Color.black;

                // Set the alignment
                textMeshPro.alignment = TextAlignmentOptions.Center;

                // Set the size of the text to match the size of the box
                RectTransform textRectTransform = textObject.GetComponent<RectTransform>();
                textRectTransform.sizeDelta = new Vector2(70, 15);
            }
        }
    }
}
