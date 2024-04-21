using UnityEngine;

[System.Serializable]
public class Target
{
    public string Name;
    public Transform PositionObject; // Use Transform to hold the reference
    public int FloorNumber;
    public bool IsStair;

    // Constructor for easy creation
    public Target(string name, Transform positionObject)
    {
        Name = name;
        PositionObject = positionObject;

        // Determine if this target is a stairwell
        IsStair = Name.StartsWith("Stairwell");
        
        // Extract floor number based on whether it's a stairwell or not
        if (IsStair)
        {
            // Assumes stairwell name is in format "Stairwell_2_A"
            string[] parts = Name.Split('_');
            if (parts.Length > 1 && int.TryParse(parts[1], out int floor))
            {
                FloorNumber = floor;
            }
            else
            {
                Debug.LogError("Failed to parse floor number from stairwell name: " + Name);
            }
        }
        else
        {
            // Extract the 4th character for non-stairwell names (e.g., "RLC102")
            if (Name.Length >= 4 && int.TryParse(Name[3].ToString(), out int floor))
            {
                FloorNumber = floor;
            }
            else
            {
                Debug.LogError("Failed to parse floor number from name: " + Name);
            }
        }
    }
}
