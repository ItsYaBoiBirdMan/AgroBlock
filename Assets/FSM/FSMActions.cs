using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMActions : MonoBehaviour
{
    [SerializeField] private Animator FSM;

    private void WaterStateHandler()
    {
        switch (FSM.GetFloat("Water Level"))
        {
            case > 61:
                 Debug.Log("Too much Water");
                 //FSM.SetFloat("Water Level", 55);
                 //Debug.Log("Water Level: " + FSM.GetFloat("Water Level"));
                break;
            case < 50:
                Debug.Log("Low Water");
                //FSM.SetFloat("Water Level", 55);
                //Debug.Log("Water Level: " + FSM.GetFloat("Water Level"));
                break;
            default:
                break;  
        }
    }

    private void TemperatureStateHandler()
    {
        switch (FSM.GetFloat("Temperature"))
        {
            case > 31:
                Debug.Log("Temperature too high");
                break;
            case < 20:
                Debug.Log("Temperature too low");
                break;
            default:
                break;
        }
    }
    
    private void LightStateHandler()
    {
        switch (FSM.GetFloat("Light Level"))
        {
            case > 91:
                Debug.Log("Light too bright");
                break;
            case < 70:
                Debug.Log("Light too dim");
                break;
            default:
                break;
        }
    }

    private void Update()
    {
       
    }
}

