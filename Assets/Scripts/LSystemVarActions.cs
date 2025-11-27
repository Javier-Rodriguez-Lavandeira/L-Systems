using UnityEngine;

[System.Serializable]

// Class for managing Actions
public class LSystemVarActions
{
    public char Symbol;

    public enum ActionType
    {
        MoveForward,
        RotatePos,
        RotateNeg,
        PushState,
        PopState,
        None
    }

    public ActionType action;
    public float value = 0f; // angle or lenght
}
