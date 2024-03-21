using UnityEngine;
using TMPro;

public class AddTextToTextMeshPro : MonoBehaviour
{
    void Start()
    {
        // Call the AddTextToTextMeshPro function with "RLC 102" as a parameter
        AddTextToTextMeshProP(" RLC 102");
    }

    void AddTextToTextMeshProP(string textToAdd)
    {
        // Get the TextMeshProUGUI component attached to the same GameObject
        TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();

        if (textMeshPro != null)
        {
            // Append the provided text to the existing text of the TextMeshPro component
            textMeshPro.text += textToAdd;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component not found on the GameObject.");
        }
    }
}
