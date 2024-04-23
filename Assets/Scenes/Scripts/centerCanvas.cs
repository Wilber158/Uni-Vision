using UnityEngine;

public class CanvasCentering : MonoBehaviour
{
    public Camera arCamera; // Reference to the AR camera
    public GameObject classInformationCanvas; // Reference to the original canvas

    // Method to center the canvas in front of the AR camera
    public void ToggleCanvasVisibility()
    {
        // Ensure the canvas object is assigned
        if (classInformationCanvas == null)
        {
            Debug.LogError("Canvas object is not assigned!");
            return;
        }

        // Toggle the canvas visibility
        if (classInformationCanvas.activeSelf == false)
        {
            classInformationCanvas.SetActive(!classInformationCanvas.activeSelf);

        }
    }
}