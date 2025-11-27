using System.Collections.Generic;
using UnityEngine;


public class LSystemGen : MonoBehaviour
{
    [Header("References")]
    public LineDrawer LineDrawer;
    [HideInInspector] public LSystemData CurrentLSystem;
    public LSystemManager LSystemManager;   

    [Header("Line adjustments")]
    public float lengthScaling = 0.5f;
    private float LineWidth = 1;

    private string CurrentString;
    private int Iterations = 0;
 
    private Stack<TransformInfo> TransformStack = new Stack<TransformInfo>();
    private List<int> GenerationMap = new List<int>();


    public void StartGeneration(LSystemData newLSystem, bool changedIter = false)
    {
        if (newLSystem == null)
        {
            Debug.LogError("No L-System selected!");
            return;
        }

        // cleaning
        TransformStack.Clear(); // Clear the transform stack
        LineDrawer.ClearLines(); // Clear the lines from the screen

        // Reset transforms
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        
        // Initialize variables
        CurrentLSystem = newLSystem;
        CurrentString = CurrentLSystem.Axiom;
        if(!changedIter)
            Iterations = CurrentLSystem.Iterations;

        // Clear and Initialize depth map
        GenerationMap.Clear();
        foreach (char c in CurrentString)
            GenerationMap.Add(0);

        GenerateLSystem();
    }

    public void modifyIterations(int iterations)
    {
        Iterations = iterations;
    }

    public void GenerateLSystem()
    {
        for (int i = 0; i < Iterations; i++) Generate(); // Calculate every iteration
        
        // After calculating the iterations --> Draw
        EdgeDrawing();
        
    }

    private void Generate()
    {
        // Initialize Vars
        string newString = "";
        List<int> newMap = new List<int>();
        char[] stringChars = CurrentString.ToCharArray();

        // Iterate through the current string: first iteration = Axiom
        for (int i = 0; i < CurrentString.Length; i++)
        {
            char c = stringChars[i];
            bool replaced = false;

            // Replace each variable with its replacement. Apply the rules
            foreach(var rule in CurrentLSystem.Rules)
            {
                if (c == rule.symbol)
                {
                    foreach (char rep in rule.replacement)
                        newMap.Add(GenerationMap[i] + 1);
                    newString += rule.replacement;
                    replaced = true;
                    break;
                }
            }
            // Keep all the constants without changes (+, -, [, ])
            if (!replaced)
            {
                newString += c.ToString();
                newMap.Add(GenerationMap[i]);
            }
        }
        CurrentString = newString;
        GenerationMap = newMap;

        Debug.Log(CurrentString);
    }

    // Action mapping and drawing
    private void EdgeDrawing()
    {
        // Iterate through the processed string
        for (int i = 0; i < CurrentString.Length; i++)
        {
            char currentChar = CurrentString[i];

            // Map each symbol with its action
            LSystemVarActions matchedAction = LSystemManager.globalActions.Find((a) => a.Symbol == currentChar);
            if (matchedAction != null)
            {
                switch (matchedAction.action)
                {
                        // F|X
                    case LSystemVarActions.ActionType.MoveForward:
                        Vector3 initialPos = transform.position;
                        float len = matchedAction.value * Mathf.Pow(lengthScaling, GenerationMap[i]); // Calculate the length of the lines
                        transform.Translate(Vector3.up * len);
                        
                        LineDrawer.DrawLine(initialPos, transform.position, Color.white, LineWidth); // Draw lines
                        break;
                        // +
                    case LSystemVarActions.ActionType.RotatePos:
                        transform.Rotate(Vector3.forward * matchedAction.value);
                        break;
                        // -
                    case LSystemVarActions.ActionType.RotateNeg:
                        transform.Rotate(Vector3.forward * matchedAction.value);
                        break;
                        // [
                    case LSystemVarActions.ActionType.PushState:
                        // Save tranforms into the stack
                        TransformInfo ti = new()
                        {
                            position = transform.position,
                            rotation = transform.rotation
                        };
                        TransformStack.Push(ti);
                        break;
                        // ]
                    case LSystemVarActions.ActionType.PopState:
                        // Get transforms from the stack
                        ti = TransformStack.Pop();
                        transform.position = ti.position;
                        transform.rotation = ti.rotation;
                        break;
                    case LSystemVarActions.ActionType.None:
                    default:
                        break;
                }

            }

        }
    }

    public void UpdateWidth(float witdth)
    {
        LineWidth = witdth;
    }
}
