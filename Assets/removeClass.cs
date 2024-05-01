using TMPro;
using UnityEngine;

public class PlayerPrefsReset : MonoBehaviour
{
    public GameObject save;
    public string keyToReset;
    public TMP_Dropdown dropdown1;
    public TMP_Dropdown dropdown2;
    public TextMeshProUGUI textToUpdate;


    public void ResetPlayerPref()
    {
        // Check if the PlayerPrefs key exists
        if (PlayerPrefs.HasKey(keyToReset))
        {
            // Remove the PlayerPrefs key
            PlayerPrefs.DeleteKey(keyToReset);
            PlayerPrefs.Save(); // Save changes
            save.SetActive(true);
            dropdown1.gameObject.SetActive(true);
            dropdown2.gameObject.SetActive(true);
            textToUpdate.text = "";
        }

    }
}
