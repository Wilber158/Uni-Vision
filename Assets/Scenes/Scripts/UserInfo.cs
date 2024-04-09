using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInfoManager : MonoBehaviour
{
    public TMP_InputField studentID;
    public TMP_InputField studentName;
    public TMP_InputField class1;
    public TMP_InputField class2;
    public TMP_InputField class3;
    public TMP_InputField class4;
    public TMP_InputField class5;
    public TMP_InputField class6;
    public Button saveButton;

    void Start()
    {
        LoadUserInfo();

        // Remove the listener if it was previously added to avoid duplicates
        saveButton.onClick.RemoveListener(SaveUserInfo);
        saveButton.onClick.AddListener(SaveUserInfo);
    }

    // Load user info from PlayerPrefs
    void LoadUserInfo()
    {
        studentID.text = PlayerPrefs.GetString("StudentId", "");
        studentName.text = PlayerPrefs.GetString("StudentName", "");
        class1.text = PlayerPrefs.GetString("Class1", "");
        class2.text = PlayerPrefs.GetString("Class2", "");
        class3.text = PlayerPrefs.GetString("Class3", "");
        class4.text = PlayerPrefs.GetString("Class4", "");
        class5.text = PlayerPrefs.GetString("Class5", "");
        class6.text = PlayerPrefs.GetString("Class6", "");
    }

    // Save user info to PlayerPrefs
    void SaveUserInfo()
    {
        PlayerPrefs.SetString("StudentId", studentID.text);
        PlayerPrefs.SetString("StudentName", studentName.text);
        PlayerPrefs.SetString("Class1", class1.text);
        PlayerPrefs.SetString("Class2", class2.text);
        PlayerPrefs.SetString("Class3", class3.text);
        PlayerPrefs.SetString("Class4", class4.text);
        PlayerPrefs.SetString("Class5", class5.text);
        PlayerPrefs.SetString("Class6", class6.text);
        PlayerPrefs.Save(); // Save the changes
    }
}

    /*
    public void ClearUserInfo()
    {
        PlayerPrefs.DeleteKey("UserName");
        PlayerPrefs.DeleteKey("UserInfo");
        PlayerPrefs.Save();
        
        // Clear input fields
        userNameInputField.text = "";
        userInfoInputField.text = "";
    }
    */


