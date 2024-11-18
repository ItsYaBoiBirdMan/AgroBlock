using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMActions : MonoBehaviour
{
    [SerializeField] private Animator FSM;
    [SerializeField] private float waterLevel;

    private void WaterStateHandler()
    {
        switch (FSM.GetFloat("Water Level"))
        {
            case > 60:
                 Debug.Log("Too much Water");
                 FSM.SetFloat("Water Level", 55);
                //Debug.Log("Water Level: " + FSM.GetFloat("Water Level"));
                break;
            case < 50:
                Debug.Log("Low Water");
                FSM.SetFloat("Water Level", 55);
                //Debug.Log("Water Level: " + FSM.GetFloat("Water Level"));
                break;
            default:
                Debug.Log("Water Normal");
                break;
        }
    }

    private void Update()
    {
        FSM.SetFloat("Water Level", waterLevel);
    }
}

