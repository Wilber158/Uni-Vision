using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DynamicDropdownPopulator : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    [SerializeField]
    private GameObject targetParent; // GameObject with targets as children

    void Start()
    {
        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        // Ensure the dropdown is not null
        if (dropdown == null) return;

        // Clear existing options
        dropdown.ClearOptions();

        // Find all children of the targetParent GameObject
        List<string> options = new List<string>();

        // Add each child's name to the options list
        foreach (Transform child in targetParent.transform)
        {
            options.Add(child.name);
        }

        // Add the options to the dropdown
        dropdown.AddOptions(options);
    }
}