using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class LSystemManager : MonoBehaviour
{
    [Header("References")]
    public LSystemGen generator;        
    public LineDrawer lineDrawer;
    [SerializeField] 
    private TMP_Dropdown dropdown;

    //public Slider angleSlider;
    //public Slider lengthSlider;


    [Header("L-Systems")]
    public List<LSystemData> allLSystems = new List<LSystemData>();

    [Header("Global Actions")]
    public List<LSystemVarActions> globalActions;

    

    public float Angle = 60f;
    public float Length = 100f;

    private int IterationsGen = 0;
    private int LSystemIndex = 0;

    private bool IterationMode = false;

    void Awake()
    {
        UpdateAngleInActions(Angle);
        UpdateLengthInActions(Length);
    }

    void Start()
    {
        if (generator == null || lineDrawer == null || dropdown == null)
        {
            Debug.LogError("LSystemManager: null references for generator, lineDrawer or dropdown.");
            return;
        }

        // Load all the predefined L-Systems
        if (allLSystems.Count == 0)
        {
            allLSystems.AddRange(Resources.LoadAll<LSystemData>("LSystems"));
        }

        RefreshDropdown();

    }

    public void UpdateAngleInActions(float angle)
    {
        foreach (var a in globalActions)
        {
            if (a.action == LSystemVarActions.ActionType.RotatePos)
                a.value = angle;

            if (a.action == LSystemVarActions.ActionType.RotateNeg)
                a.value = -angle;
        }
    }

    public void UpdateLengthInActions(float length)
    {
        foreach (var a in globalActions)
        {
            if (a.action == LSystemVarActions.ActionType.MoveForward)
                a.value = length;
        }
    }

    // Adds new System to the list of L-Systems, if theres one with the same name --> Overwrite it
    public void RegisterNewSystem(LSystemData data)
    {
        bool newLSystem = true;
        for(int i = 0; i < allLSystems.Count; i++)  
        {
            if (allLSystems[i].Name == data.Name)
            {
                allLSystems[i] = data;
                newLSystem = false;
                break;
            }
        }
        if(newLSystem) 
            allLSystems.Add(data);

        RefreshDropdown();
    }

    // Reloads the dropdown menu
    private void RefreshDropdown()
    {
        if (dropdown != null)
        {
            dropdown.ClearOptions();
            List<string> names = new List<string>();

            foreach (var lSystem in allLSystems)
            {
                names.Add(lSystem.Name);
                //Debug.Log(lSystem.Name);
            }
            dropdown.AddOptions(names);
            dropdown.onValueChanged.AddListener(OnDropdownChanged);
        }
    }

    // When width slider changed
    public void OnWidthSliderValueChanged(float value)
    {
        generator.UpdateWidth(value);
        StartGeneration(LSystemIndex);
    }

    // When Length slider changed
    public void OnLengthSliderValueChanged(float value)
    {
        Length = value;
        UpdateLengthInActions(value);
        StartGeneration(LSystemIndex);
    }
    
    // When Angle slider changed
    public void OnAngleSliderValueChanged(float value)
    {
        Angle = value;
        
        UpdateAngleInActions(value);
        StartGeneration(LSystemIndex);
    }

    // When stepping through iterations
    public void OnIterationChanged()
    {
        IterationMode = true;
        generator.modifyIterations(IterationsGen);
        StartGeneration(LSystemIndex);
    }

    public void OnAugmentIterations()
    {
     
        IterationsGen++;
        OnIterationChanged();
            
    }
    public void OnDecrementIterations()
    {
        
        IterationsGen = Mathf.Max(0, IterationsGen - 1);
        OnIterationChanged();

    }
    public void OnStartSteppingIterations()
    {
        IterationsGen = 0;
        OnIterationChanged();
    }

    // When L-System selected from dropdown
    private void OnDropdownChanged(int index)
    {
        if (index >= 0 && index < allLSystems.Count)
        {
            LSystemIndex = index;
            
            //generator.StartGeneration(allLSystems[index]);
        }
    }

    // When GEN button pressed
    public void OnGenButtonPressed()
    {
        IterationMode = false;
        if (LSystemIndex >= 0 && LSystemIndex < allLSystems.Count)
        {
            UpdateAngleInActions(allLSystems[LSystemIndex].Angle);
            UpdateLengthInActions(allLSystems[LSystemIndex].Length);
            StartGeneration(LSystemIndex);
        }
        
    }

    // Manages the display of L-Systems from hotkeys
    public void StartFromHotKey(int index)
    {
        IterationMode = false;
        LSystemIndex = index;
        UpdateAngleInActions(allLSystems[index].Angle);
        UpdateLengthInActions(allLSystems[index].Length);
        StartGeneration(index);
    }

    // Send L-System to Generate
    public void StartGeneration(int system)
    {
        if (!IterationMode)
        {
            IterationsGen = allLSystems[system].Iterations;
        }
        dropdown.SetValueWithoutNotify(system);
        generator.StartGeneration(allLSystems[system], IterationMode);
    }

    public void closeApp()
    {
        Application.Quit();
    }




}
