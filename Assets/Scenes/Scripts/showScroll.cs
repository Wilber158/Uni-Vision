using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    public GameObject panelToToggle; // Assign the panel you want to toggle in the inspector

    // You will link this function to the button's OnClick event in the Inspector
    public void TogglePanelVisibility()
    {
        // This will toggle the active state of the panel
        if (panelToToggle != null)
            panelToToggle.SetActive(!panelToToggle.activeSelf);
    }
}
