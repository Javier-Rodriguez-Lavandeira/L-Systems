using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LSystemUICreator : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField axiomInput;
    public TMP_InputField iterInput;
    public TMP_InputField angleInput;
    public TMP_InputField lengthInput;

    public Transform rulesContainer;
    public GameObject rulePrefab;

    public LSystemManager manager;

    private List<RuleUI> ruleUIs = new List<RuleUI>();
    public string defaultName = "NEW";

    // Funtion called when Add rule button pressed
    public void AddRuleUI()
    {
        GameObject ruleInstantiate = Instantiate(rulePrefab, rulesContainer);
        RuleUI ruleUI = ruleInstantiate.GetComponent<RuleUI>();
        ruleUIs.Add(ruleUI);
    }

    // Function called when Create New System button pressed
    public void CreateNewLSystem()
    {
        LSystemData newLSystem = LSystemData.CreateInstance<LSystemData>();

        if (newLSystem.Rules == null)
            newLSystem.Rules = new List<LSystemRules>();

        // If the name is equal to one of the predefined L-Systems --> rename it as "defaultName"
        if ("ABCDEFGH".Contains(nameInput.text) && nameInput.text.Length == 1) newLSystem.Name = defaultName;
        else newLSystem.Name = nameInput.text;

        newLSystem.Axiom = axiomInput.text;
        newLSystem.Iterations = int.Parse(iterInput.text);
        newLSystem.Angle = float.Parse(angleInput.text);
        newLSystem.Length = float.Parse(lengthInput.text);

        foreach (var r in ruleUIs)
        {
            if (r.symbolInput.text != "" || r.replacementInput.text != "")
            {
                LSystemRules rule = new LSystemRules();
                rule.symbol = r.symbolInput.text[0];
                rule.replacement = r.replacementInput.text;

                newLSystem.Rules.Add(rule);
            }
        }

        manager.RegisterNewSystem(newLSystem);

    }

}
