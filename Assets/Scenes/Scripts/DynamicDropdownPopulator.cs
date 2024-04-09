using UnityEngine;
using TMPro; // Make sure to have the TextMesh Pro package
using System.Collections.Generic;

public class DynamicDropdownPopulator : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    [SerializeField]
    private GameObject targetParent; // GameObject with targets as children

    // Public static list to be accessed by the navigation script
    public static List<Target> navigationTargetObjects = new List<Target>();

    void Start()
    {
        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        if (dropdown == null || targetParent == null) return;

        dropdown.ClearOptions();
        navigationTargetObjects.Clear(); // Clear the existing list

        List<string> options = new List<string>();
        foreach (Transform child in targetParent.transform)
        {
            options.Add(child.name);
            navigationTargetObjects.Add(new Target(child.name, child)); // Add to static list
        }

        dropdown.AddOptions(options);
    }
}