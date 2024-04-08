using TMPro;
using UnityEngine;

public class userLogin : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField userIdInputField;
    [SerializeField]
    private TMP_InputField passwordInputField;
    [SerializeField]
    private GameObject loginUI; // Assign the parent GameObject of the login UI in the Unity Inspector

    private void Start()
    {
        // Check if login details are saved and adjust UI visibility accordingly
        loginUI.SetActive(!saveLoginInfo.AreCredentialsSaved());
    }

    public void SubmitLoginInformation()
    {
        // Retrieve input data
        string userId = userIdInputField.text;
        string password = passwordInputField.text;

        // Securely save the data using SecureDataManager
        saveLoginInfo.UserId = userId;
        saveLoginInfo.Password = password;

        loginUI.SetActive(false);

        Debug.Log("Login information submitted and saved securely.");
    }
}