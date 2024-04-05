using System;
using UnityEngine;

public class Target
{
    public string Name { get; set; }
    public GameObject PositionObject { get; set; }

    public Target(string name, GameObject positionObject)
    {
        Name = name;
        PositionObject = positionObject;
    }
}