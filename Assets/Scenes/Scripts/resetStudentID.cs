using TMPro;
using UnityEngine;

public class resetStudentID : MonoBehaviour
{
    public GameObject inputObject;
    public GameObject targetObject;
    public TMP_InputField inputField;
    public GameObject inputFieldMain;
    public string placeholderText = "Enter your student ID:";


    public void delete()
    {
        PlayerPrefs.DeleteKey("studentID");
        TextMeshProUGUI foundObject = targetObject.GetComponentInChildren<TextMeshProUGUI>();
        inputFieldMain.SetActive(true);
        inputObject.SetActive(false);
        inputField.text = "";
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = placeholderText;
    }
}