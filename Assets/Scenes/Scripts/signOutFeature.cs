using UnityEngine;
using UnityEngine.UI; 

public class signOutFeature : MonoBehaviour
{
    public Button signOutButton;
    public GameObject loginPanel;

    void Start()
    {
        if (signOutButton != null)
        {
            signOutButton.onClick.AddListener(OnSignOutClicked);
        }
    }

    private void OnSignOutClicked()
    {
        saveLoginInfo.ResetCredentials(); // Clear the credentials
        if (loginPanel != null)
        {
            loginPanel.SetActive(true); // Make the login page visible
        }
        Debug.Log("User signed out, credentials reset.");
    }

    void OnDestroy()
    {
        if (signOutButton != null)
        {
            signOutButton.onClick.RemoveListener(OnSignOutClicked);
        }
    }
}

