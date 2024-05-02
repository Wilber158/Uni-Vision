using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class loaderStudentInfo : MonoBehaviour
{
    public GameObject inputObject; // inputfield
    public GameObject targetObject; //studentName
    public TextMeshProUGUI inputField; // placeholder
    public GameObject button;
    public GameObject inputObject2; // inputfield
    public GameObject targetObject2; //studentName
    public TextMeshProUGUI inputField2; // placeholder

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("studentName"))
        {
            targetObject.SetActive(true);
            inputField.text = PlayerPrefs.GetString("studentName");
            inputObject.SetActive(false);
        }
        if (PlayerPrefs.HasKey("studentID"))
        {
            targetObject2.SetActive(true);
            inputField2.text = PlayerPrefs.GetString("studentID");
            inputObject2.SetActive(false);
            button.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
