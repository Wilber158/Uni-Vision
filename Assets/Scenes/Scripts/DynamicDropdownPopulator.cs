using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DynamicDropdownPopulator : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    [SerializeField]
    private GameObject targetParent; // GameObject with targets as children

    // Public list to be accessed by the navigation script
    public List<Target> navigationTargetObjects = new List<Target>();

    // Singleton pattern for accessing instance from other scripts
    public static DynamicDropdownPopulator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

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
            // Create new Target instance with more detailed initialization as needed
            Target newTarget = new Target(child.name, child); // Initialize additional properties as needed
            navigationTargetObjects.Add(newTarget); // Add to list
        }

        dropdown.AddOptions(options);
    }

    // Method to access navigation target objects
    public List<Target> GetNavigationTargets()
    {
        return navigationTargetObjects;
    }
}
