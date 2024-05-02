using UnityEngine;
using TMPro;

public class DropdownManager : MonoBehaviour
{
    public TMP_Dropdown dropdown1;
    public TMP_Dropdown dropdown2;
    public GameObject button;
    public TextMeshProUGUI textToUpdate;

    public string referenceText;

    void Start()
    {
        // Check if the PlayerPrefs has the reference text value
        if (PlayerPrefs.HasKey(referenceText))
        {
            // Get the saved value from PlayerPrefs
            string savedValue = PlayerPrefs.GetString(referenceText);

            // Update the text object with the saved value
            textToUpdate.text = savedValue;

            // Set both dropdowns as inactive (not visible)
            dropdown1.gameObject.SetActive(false);
            dropdown2.gameObject.SetActive(false);
            button.SetActive(false);
        }
        else
        {
            Debug.LogWarning("PlayerPrefs does not contain a value for the reference text: " + referenceText);
        }
    }
}
