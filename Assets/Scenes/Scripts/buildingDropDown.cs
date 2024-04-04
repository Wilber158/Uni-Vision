using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro;

public class CustomDropdown : MonoBehaviour, IPointerDownHandler 
{
    public TMP_InputField searchInput;
    public GameObject optionsPanel;
    public Button[] optionButtons; 

    void Start()
    {
        optionsPanel.SetActive(false); 
        foreach (Button btn in optionButtons)
        {
            btn.onClick.AddListener(() => OnOptionSelected(btn));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Show the options panel when the InputField is clicked
        optionsPanel.SetActive(true);
    }

    private void OnOptionSelected(Button selectedButton)
    {
        searchInput.text = selectedButton.GetComponentInChildren<TextMeshProUGUI>().text; // Send text from button to input field for user to see
        optionsPanel.SetActive(false); // Hide panel
    }
}
