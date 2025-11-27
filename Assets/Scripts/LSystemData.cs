using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewLSystem", menuName = "L-System/New L-System")]
// Class for managing and for creating new predefined L-Systems
public class LSystemData : ScriptableObject
{
    public string Name;
    public string Axiom;
    public int Iterations;
    public float Angle;
    public float Length;
    public enum LSystemMode { EdgeWriting, NodeWriting }
    public LSystemMode mode = LSystemMode.EdgeWriting;

    public List<LSystemRules> Rules;

}
