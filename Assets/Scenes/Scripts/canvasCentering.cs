using UnityEngine;

public class CanvasCentering : MonoBehaviour
{
    public Camera arCamera; // Reference to the AR camera
    public GameObject classInformationCanvas; // Reference to the original canvas

    // Method to center the canvas in front of the AR camera
    public void CenterCanvas()
    {
        // Ensure the canvas object is assigned
        if (classInformationCanvas == null)
        {
            Debug.LogError("Canvas object is not assigned!");
            return;
        }

        // Calculate the position for the canvas based on the AR camera's forward direction
        Vector3 spawnPosition = arCamera.transform.position + arCamera.transform.forward * 2f; // Adjust the distance as needed

        // Set the canvas position to the calculated spawn position
        classInformationCanvas.transform.position = spawnPosition;

        // Set the canvas rotation to match the AR camera's rotation
        classInformationCanvas.transform.rotation = arCamera.transform.rotation;

        // Reset the rotation around the z-axis to ensure it's not flipped
        Vector3 eulerRotation = classInformationCanvas.transform.rotation.eulerAngles;
        eulerRotation.z = 180f;
        classInformationCanvas.transform.rotation = Quaternion.Euler(eulerRotation);
    }
}
