using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobertSensors : MonoBehaviour
{
    public Sensor left, middle, right;

    void Start() {
        if (left == null || middle == null || right == null) {
            Debug.Log("ERROR: 'Left', 'Middle' and 'Right' sensors must be linked to RobertSensors objects.");
        }
    }
    void Update() {}

    public bool[] CheckSensors(){
        return new bool[3]{ left.isTriggered(), middle.isTriggered(), right.isTriggered() };
    }
}
