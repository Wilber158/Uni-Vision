using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpdateTextWithSliderValue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay; // Assign your TextMeshProUGUI component here
    public Slider slider; // Assign your Slider component here

    private void Start()
    {
        UpdateText(slider.value); // Set the initial text
    }

    public void UpdateText(float sliderValue)
    {
        // Log the received value for debugging
        Debug.Log("Slider Value Received: " + sliderValue);
        
        // Update the TextMeshProUGUI component to display the slider's value
        textDisplay.text = sliderValue.ToString("0");
    }
}
