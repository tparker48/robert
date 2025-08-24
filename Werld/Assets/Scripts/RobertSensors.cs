using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RobertSensors : MonoBehaviour
{
    public Sensor left, middle, right;

    void Start()
    {
        if (left == null || middle == null || right == null)
        {
            Debug.Log("ERROR: 'Left', 'Middle' and 'Right' sensors must be linked to RobertSensors objects.");
        }
    }
    void Update() { }

    public bool[] CheckSensors()
    {
        return new bool[3] { left.isTriggered(), middle.isTriggered(), right.isTriggered() };
    }

    public bool CheckForObjectType<T>()
    {
        foreach (GameObject obj in middle.GetSensedObjects())
        {
            T comp = obj.GetComponent<T>();
            T compInChildren = obj.GetComponentInChildren<T>();
            if (comp != null || compInChildren != null)
            {
                return true;
            }
        }
        return false;
    }

    public T GetObjectType<T>() {
        foreach (GameObject obj in middle.GetSensedObjects())
        {
            T comp = obj.GetComponent<T>();
            T compInChildren = obj.GetComponentInChildren<T>();
            if (comp != null)
            {
                return comp;
            }
            if (compInChildren != null)
            {
                return compInChildren;
            }
        }
        return default;
    }
}
