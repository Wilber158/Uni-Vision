using UnityEngine;


[System.Serializable]
public class Target
{
    public string Name;
    public Transform PositionObject; // Use Transform to hold the reference

    // Constructor for easy creation
    public Target(string name, Transform positionObject)
    {
        Name = name;
        PositionObject = positionObject;
    }
}
