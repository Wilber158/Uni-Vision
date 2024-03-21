using UnityEngine;
using UnityEngine.UI;

namespace MyNamespace
{
    public class createBoxes : MonoBehaviour
    {
        public int boxCount = 7; // Number of boxes to create
        public Sprite boxSprite; // Sprite for the box image

        void Start()
        {
            // Get the Canvas to be the parent of the boxes
            GameObject content = GameObject.Find("content");

            // Loop to create the specified number of boxes
            for (int i = 0; i < boxCount; i++)
            {
                // Create a new GameObject for the box
                GameObject box = new GameObject("Box" + i);

                // Set the parent of the box to the Canvas
                box.transform.SetParent(content.transform, false);

                // Add an Image component to the box
                Image image = box.AddComponent<Image>();

                // Assign the box image to the Image component
                image.sprite = boxSprite;
                image.color = Color.black;

                // Set the size and position of the box
                RectTransform rectTransform = box.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(800, 250); // Change the size as needed
                rectTransform.anchoredPosition = new Vector2(100 * i, 0); // Change the position as needed
            }
        }
    }
}
