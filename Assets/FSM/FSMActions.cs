using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FSMActions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TestMessage;
    private void HighHumidityMessage()
    {
        TestMessage.text = "Humidity is too High!!";
        Debug.Log("Humidity is too High!!");
    }
    private void LowHumidityMessage()
    {
        TestMessage.text = "Humidity is too Low!!";
        Debug.Log("Humidity is too Low!!");
    }
    
    private void HighTemperatureMessage()
    {
        TestMessage.text = "Temperature is too High!!";
        Debug.Log("Temperature is too High!!");
    }
    
    private void LowTemperatureMessage()
    {
        TestMessage.text = "Temperature is too Low!!";
        Debug.Log("Temperature is too Low!!");
    }
    
    private void TurningLightOnMessage()
    {
        TestMessage.text = "Light has been turned on.";
        Debug.Log("Light has been turned on.");
    }
    
    private void TurningLightOffMessage()
    {
        TestMessage.text = "Light has been turned off.";
        Debug.Log("Light has been turned off.");
    }
    
    private void HighNitrogenMessage()
    {
        TestMessage.text = "Nitrogen is too High!!";
        Debug.Log("Nitrogen is too High!!");
    }
    
    private void LowNitrogenMessage()
    {
        TestMessage.text = "Nitrogen is too Low!!";
        Debug.Log("Nitrogen is too Low!!");
    }
    
    private void HighPhosphorousMessage()
    {
        TestMessage.text = "Phosphorous is too High!!";
        Debug.Log("Phosphorous is too High!!");
    }
    
    private void LowPhosphorousMessage()
    {
        TestMessage.text = "Phosphorous is too Low!!";
        Debug.Log("Phosphorous is too Low!!");
    }
    
    private void HighPotassiumMessage()
    {
        TestMessage.text = "Potassium is too High!!";
        Debug.Log("Potassium is too High!!");
    }
    
    private void LowPotassiumMessage()
    {
        TestMessage.text = "Potassium is too Low!!";
        Debug.Log("Potassium is too Low!!");
    }
}

