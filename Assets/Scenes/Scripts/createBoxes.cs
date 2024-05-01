    using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
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

        public void UpdateBoxes(Dictionary<string,List<string>>eventData)
        {
            // Ensure existing boxes are destroyed before creating new ones
            DestroyExistingBoxes();
            Debug.Log($"Event Data: {eventData}");
            CreateBoxesFromEventData(eventData);
            Debug.Log("Createboxes called!");
        }

        private void CreateBoxesFromEventData(Dictionary<string, List<string>> eventData)
        {
            int index = 0;
            foreach (KeyValuePair<string, List<string>> entry in eventData)
            {
                string className = entry.Key;
                foreach (string classTime in entry.Value)
                {
                    CreateBox(className, classTime, index);
                    index++;
                }
            }
        }

        private void CreateBox(string className, string classTime, int index)
        {
            GameObject box = new GameObject($"Box_{index}");
            box.transform.SetParent(content.transform, false);

            Image image = box.AddComponent<Image>();


            for (int i = 1; i <= 6; i++)
            {
                string current_class = PlayerPrefs.GetString("class" + i);
                string classNameWithoutSpaces = className.Replace(" ", "");

                if (current_class.Equals(classNameWithoutSpaces, StringComparison.OrdinalIgnoreCase))
                {
                    // Match found, change the image color to red
                    image.color = new Color(1.0f, 0.3f, 0.3f);
                    // Exit the loop since we found a match
                    break;
                }
                else
                {
                    // No match, set the image color to light grey
                    image.color = new Color(0.99f, 0.99f, 0.99f);
                }
            }

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
