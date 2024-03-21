using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required for the EventSystem
using TMPro;

public class CustomDropdown : MonoBehaviour, IPointerDownHandler // Implement the interface
{
    public TMP_InputField searchInput;
    public GameObject optionsPanel;
    public Button[] optionButtons; // Assign your option buttons here

    void Start()
    {
        optionsPanel.SetActive(false); // Make sure the panel is hidden on start
        // Assign the click event to the buttons
        foreach (Button btn in optionButtons)
        {
            btn.onClick.AddListener(() => OnOptionSelected(btn));
        }
    }

    // Implement the IPointerDownHandler method
    public void OnPointerDown(PointerEventData eventData)
    {
        // Show the options panel when the InputField is clicked
        optionsPanel.SetActive(true);
    }

    private void OnOptionSelected(Button selectedButton)
    {
        searchInput.text = selectedButton.GetComponentInChildren<TextMeshProUGUI>().text; // Set text from button to input field
        optionsPanel.SetActive(false); // Hide options panel
        /*
        searchInput.ActivateInputField(); // Optional: To refocus on the InputField
        searchInput.Select(); // Optional: Highlight the text
        */
    }
}
