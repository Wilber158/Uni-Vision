using UnityEngine;
using TMPro;

public class AddTextToTextMeshPro : MonoBehaviour
{
    public GameObject classInformationTitle; // Reference to the GameObject with TextMeshProUGUI component

    // Method to change the text of the TextMeshProUGUI component
    public void ChangeText(string targetName)
    {
        if (classInformationTitle == null)
        {
            Debug.LogWarning("GameObject with TextMeshProUGUI component not assigned.");
            return;
        }

        // Get TextMeshProUGUI component attached to classInformationTitle GameObject
        TextMeshProUGUI textMeshPro = classInformationTitle.GetComponent<TextMeshProUGUI>();

        // Check if TextMeshProUGUI component exists
        if (textMeshPro != null)
        {
            // Change the text of the TextMeshProUGUI component
            textMeshPro.text = "";
            textMeshPro.text = targetName;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component not found on the GameObject.");
        }
    }
}
