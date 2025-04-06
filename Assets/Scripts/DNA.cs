using System.Collections.Generic; // Pour List<>
using UnityEngine;                // Pour Color

[System.Serializable]
public class DNA
{
    public float swimSpeed;
    public float turnSpeed;
    public float curiosity;
    public Color skinColor;
    public List<string> edibleTags;
    public List<string> toxicTags;
}
