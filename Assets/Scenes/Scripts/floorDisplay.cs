using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueToText : MonoBehaviour
{
    public Slider slider; // Assign your Slider component here
    public TextMeshProUGUI textDisplay; // Assign your TextMeshProUGUI component here

    private void Start()
    {
        // Add a listener to the slider's value changed event
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // This method will be called whenever the slider's value changes
    public void ValueChangeCheck()
    {
        // Update the TextMeshProUGUI component to display the slider's value
        // Assuming the slider's values are whole numbers
        textDisplay.text = slider.value.ToString();
    }
}

/*
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
*/