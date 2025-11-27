using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotKeysInput : MonoBehaviour
{
    public LSystemManager manager;
    
    void Update()
    {
        // If input field selected --> dont get other inputs
        if (EventSystem.current.currentSelectedGameObject != null &&
            EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null)
        {
            return;
        }
        // Hotkeys 1-8
        if (Input.GetKeyDown(KeyCode.Alpha1)) Display(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Display(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Display(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Display(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) Display(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) Display(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) Display(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) Display(7);

        // Step through iterations
        if (Input.GetKeyDown(KeyCode.W)) manager.OnStartSteppingIterations();
        if (Input.GetKeyDown(KeyCode.E)) manager.OnAugmentIterations();
        if (Input.GetKeyDown(KeyCode.Q)) manager.OnDecrementIterations();
       
    }

    private void Display(int index)
    {
        if (index < 0 || index >= manager.allLSystems.Count)
        {
            Debug.LogWarning("LSystem does not exist in that position");
            return;
        }

        manager.StartFromHotKey(index);
    }
}
