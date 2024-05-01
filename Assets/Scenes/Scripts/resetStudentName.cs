using TMPro;
using UnityEngine;

public class resetStudentName : MonoBehaviour
{
    public GameObject inputObject;
    public GameObject targetObject;
    public TMP_InputField inputField;
    public GameObject inputFieldMain;
    public string placeholderText = "Enter your name:";


    public void delete()
    {
        PlayerPrefs.DeleteKey("studentName");
        TextMeshProUGUI foundObject = targetObject.GetComponentInChildren<TextMeshProUGUI>();
        inputFieldMain.SetActive(true);
        inputObject.SetActive(false);
        inputField.text = "";
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = placeholderText;
    }
}