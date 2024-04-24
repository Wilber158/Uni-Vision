using UnityEngine;

public class ToggleCanvasVisibility : MonoBehaviour
{
    // Function to toggle the visibility of the parent canvas
    public GameObject classInformationCanvas; // Reference to the original canvas

    public void ToggleVisibility()
    {
        Debug.Log("ToggleVisibility Called");
        // Get the parent canvas component
        Canvas parentCanvas = GetComponentInParent<Canvas>();

        classInformationCanvas.SetActive(!classInformationCanvas.activeSelf);
    }
}
