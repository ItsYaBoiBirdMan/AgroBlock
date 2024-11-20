using System;
using System.Collections;
using System.Collections.Generic;
using Script.WifiConnection;
using UnityEngine;

public class ConditionManager : MonoBehaviour { 
   [SerializeField] private Esp32SocketClient socketClient;
   private Animator animator;
   private float waterLevel;      // Current water level (update this based on your game logic)
   private float thresholdTooMuchWater = 50.0f; // Threshold for StateA
   private float thresholdNotEnoughWater = 70.0f; // Threshold for StateB
   private float thresholdTooWarm = 50.0f; // Threshold for StateA
   private float thresholdNotWarmEnough = 70.0f; // Threshold for StateB

    void Start() {
        socketClient.OnTemperatureDataReceived += CheckTemperatureState;
        socketClient.OnHumidityDataReceived += CheckHumidityState;
        socketClient.LightStateDataReceived += CheckLightState;
        socketClient.OnNitrogenDataReceived += CheckNitrogenState;
        socketClient.OnPhosphorousDataReceived += CheckPhosphorousState;
        socketClient.OnPotassiumDataReceived += CheckPotassiumState;
        InvokeRepeating(nameof(GetTemperature), 2.0f, 180.0f);
        InvokeRepeating(nameof(GetHumidity), 3.0f, 180.0f);
        InvokeRepeating(nameof(GetLight), 4.0f, 600.0f);
        InvokeRepeating(nameof(GetFertilizer), 5.0f, 3600.0f);
    }

    public void GetTemperature() {
        socketClient.SendMessageToEsp32("Temperature esp 0");
    }
    public void GetHumidity() {
        socketClient.SendMessageToEsp32("Humidity esp 0");
    }
    public void GetLight() {
        socketClient.SendMessageToEsp32("Lights state 0");
    }
    public void GetFertilizer() {
        socketClient.SendMessageToEsp32("NPK esp 0");
    }

    private void CheckTemperatureState(float temperature){
        if (temperature > thresholdTooWarm) {
            TriggerState("HighTemperature");
        } else if (temperature < thresholdNotWarmEnough) {
            TriggerState("LowTemperature");
        }
    }
    
    private void CheckHumidityState(float humidity){
        if (humidity > thresholdTooMuchWater) {
            TriggerState("HighHumidity");
        } else if (humidity < thresholdNotEnoughWater) {
            TriggerState("LowHumidity");
        }
    }
    
    
    private void CheckLightState(bool light){
       //TODO
    }
    
    
    private void CheckNitrogenState(float nitrogen){
        if (nitrogen > thresholdTooMuchWater) {
            TriggerState("HighNitrogen");
        } else if (nitrogen < thresholdNotEnoughWater) {
            TriggerState("LowNitrogen");
        }
    }
    
    private void CheckPhosphorousState(float phosphorous){
        if (phosphorous > thresholdTooMuchWater) {
            TriggerState("HighPhosphorous");
        } else if (phosphorous < thresholdNotEnoughWater) {
            TriggerState("LowPhosphorous");
        }
    }
    
    private void CheckPotassiumState(float potassium){
        if (potassium > thresholdTooMuchWater) {
            TriggerState("HighPotassium");
        } else if (potassium < thresholdNotEnoughWater) {
            TriggerState("LowPotassium");
        }
    }
    
    private void TriggerState(string triggerName){
        // Reset all other triggers to avoid conflicting transitions
        ResetAllTriggers();

        // Set the trigger for the desired state
        animator.SetTrigger(triggerName);
    }
    private void ResetAllTriggers()
    {
        // List all triggers used in your Animator
        animator.ResetTrigger("HighTemperature");
        animator.ResetTrigger("LowTemperature");
        
        animator.ResetTrigger("HighHumidity");
        animator.ResetTrigger("LowHumidity");
        
        animator.ResetTrigger("HighNitrogen");
        animator.ResetTrigger("LowNitrogen");
        
        animator.ResetTrigger("HighPhosphorous");
        animator.ResetTrigger("LowPhosphorous");
        
        animator.ResetTrigger("HighPotassium");
        animator.ResetTrigger("LowPotassium");
        
        
        
        
    }

}
