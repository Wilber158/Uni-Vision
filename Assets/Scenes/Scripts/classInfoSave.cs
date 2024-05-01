using UnityEngine;
using TMPro;

public class TMP_DropdownManager : MonoBehaviour
{
    public TMP_Dropdown dropdown1;
    public TMP_Dropdown dropdown2;
    public GameObject button;
    public TextMeshProUGUI textToUpdate;
    public string classNumber;

    public void CheckAndSaveValues()
    {
        // Check if both dropdowns have a value selected
        if (dropdown1.value != 0 && dropdown2.value != 0)
        {
            // Get the selected values from the dropdowns
            string selectedValue1 = dropdown1.options[dropdown1.value].text;
            string selectedValue2 = dropdown2.options[dropdown2.value].text;

            // Perform your save operation here
            PlayerPrefs.SetString(classNumber, selectedValue1+selectedValue2);
            PlayerPrefs.Save();
            string savedValue = PlayerPrefs.GetString(classNumber);

            // Update the text object with the saved value
            textToUpdate.text = savedValue;

            // Set both dropdowns as inactive (not visible)
            dropdown1.gameObject.SetActive(false);
            dropdown2.gameObject.SetActive(false);
            button.SetActive(false);
        }
        else
        {
            // Show an error message if any of the dropdowns is not selected
            Debug.LogError("Please select values from both dropdowns.");
        }
    }
}
