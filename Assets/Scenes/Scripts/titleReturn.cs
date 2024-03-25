using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using MyNamespace;

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
