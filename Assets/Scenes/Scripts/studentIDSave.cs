using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    public GameObject inputObject;
    public GameObject targetObject;
    public TMP_InputField inputField;
    public GameObject inputFieldMain;

    public GameObject inputObject2;
    public GameObject targetObject2;
    public TMP_InputField inputField2;
    public GameObject inputFieldMain2;

    private const string textKey = "studentName";
    private const string textKey2 = "studentID";

    //void Start()
    //{
    /* Check if the PlayerPrefs variable exists
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
    }*/
    // }
    public void saver()
    {

        string inputValue = inputField.text;
        string inputValue2 = inputField2.text;
        if (inputValue == "" || inputValue2 == "")
        {
            Debug.Log("");
        }
        else
        {
            if (inputValue == "")
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
                        foundObject.text = inputValue;

                    // Save the text to PlayerPrefs
                    PlayerPrefs.SetString(textKey, inputValue);
                    PlayerPrefs.Save();
                }
                inputFieldMain.SetActive(false);
            }
            if (inputValue2 == "")
            {
                Debug.Log("");
            }
            else
            {
                // Find the GameObject named "Placeholder"
                inputObject2.SetActive(true);

                // If the "Placeholder" object exists
                if (targetObject2 != null)
                {
                    // Find the TextMeshPro component within its children
                    TextMeshProUGUI foundObject = targetObject2.GetComponentInChildren<TextMeshProUGUI>();

                    // If the TextMeshPro component is found
                    if (foundObject != null)
                        // Update the text area
                        foundObject.text = inputValue2;

                    // Save the text to PlayerPrefs
                    PlayerPrefs.SetString(textKey2, inputValue2);
                    PlayerPrefs.Save();
                }
                inputFieldMain2.SetActive(false);
            }
        }
    }
}