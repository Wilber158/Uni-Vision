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
    [SerializeField]
    private TMP_Text errorMessageUI; // Reference to the TextMeshPro UI element for error messages

    private void Start()
    {
        // Check if login details are saved and adjust UI visibility accordingly
        loginUI.SetActive(!saveLoginInfo.AreCredentialsSaved());
        errorMessageUI.gameObject.SetActive(false); // Ensure the error message is not visible at start
    }

    public void SubmitLoginInformation()
    {
        // Retrieve input data
        string userId = userIdInputField.text;
        string password = passwordInputField.text;

        // Get the login status using a local variable for confirmation
        bool confirmation = SeleniumExample.loginStatus; // Use the correct class name here

        // Check the confirmation status before saving
        if (!confirmation) 
        {
            // Display error message if confirmation is false
            errorMessageUI.gameObject.SetActive(true); // Show the error message
            return; // Exit the function if confirmation is false
        }
        
        // Hide the error message in case it was previously shown
        errorMessageUI.gameObject.SetActive(false);

        // If confirmation is true, proceed with saving the login info
        saveLoginInfo.UserId = userId;
        saveLoginInfo.Password = password;

        // Hide the login UI
        loginUI.SetActive(false);

        Debug.Log("Login information submitted and saved securely.");
    }
}


/*
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
        bool confirmation = scrapper.loginStatus;
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
*/