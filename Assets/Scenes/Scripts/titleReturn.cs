using UnityEngine;
using TMPro;

public class AddTextToTextMeshPro : MonoBehaviour
{
    private string text = "RLC 102";

    // Public property to access the text variable
    public string Text
    {
        get { return text; }
    }

    void Start()
    {
        AddTextToTextMeshProP(text);
    }

    void AddTextToTextMeshProP(string textToAdd)
    {
        // Get all components attached to the same GameObject
        Component[] components = GetComponents<Component>();

        // Loop through all components to find TextMeshProUGUI
        foreach (Component component in components)
        {
            // Check if the component is a TextMeshProUGUI component
            if (component.GetType() == typeof(TextMeshProUGUI))
            {
                // If found, cast it to TextMeshProUGUI and append textToAdd
                TextMeshProUGUI textMeshPro = (TextMeshProUGUI)component;
                textMeshPro.text += textToAdd;
                return; // Exit the loop after finding TextMeshProUGUI
            }
        }

        // If TextMeshProUGUI component is not found, log a warning
        Debug.LogWarning("TextMeshProUGUI component not found on the GameObject.");
    }
}
