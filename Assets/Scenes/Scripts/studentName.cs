using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputName : MonoBehaviour
{
    public GameObject inputObject;
    public GameObject targetObject;
    public TMP_InputField inputField;
    public GameObject inputFieldMain;


    private const string textKey = "studentName";

    void Start()
    {
        // Check if the PlayerPrefs variable exists
        if (PlayerPrefs.HasKey(textKey))
        {
            // If the PlayerPrefs variable exists, load the text
            string savedText = PlayerPrefs.GetString(textKey);
            inputObject.SetActive(true);
            TextMeshProUGUI foundObject = targetObject.GetComponentInChildren<TextMeshProUGUI>();
            foundObject.text = savedText;

            // Enable the target object
            if (targetObject != null)
            {
                targetObject.SetActive(true);
            }
            inputFieldMain.SetActive(false);
        }
    }
    void Update()
    {
        string InputValue = inputField.text;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) && InputValue != "")
        {
            string inputVal = inputField.text;
            if (inputVal == "")
            {
                Debug.Log("");
            }
            else
            {
                // Find the GameObject named "Placeholder"
                inputObject.SetActive(true);

                // If the "Placeholder" object exists
                if (targetObject != null)
                {
                    // Find the TextMeshPro component within its children
                    TextMeshProUGUI foundObject = targetObject.GetComponentInChildren<TextMeshProUGUI>();

                    // If the TextMeshPro component is found
                    if (foundObject != null)
                        // Update the text area
                        foundObject.text = inputVal;

                    // Save the text to PlayerPrefs
                    PlayerPrefs.SetString(textKey, inputVal);
                    PlayerPrefs.Save();
                }
                inputFieldMain.SetActive(false);
            }
        }
    }
}
