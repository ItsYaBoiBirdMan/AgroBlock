using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.StateMachine;
using Script.WifiConnection;
using UnityEngine;

public class ConditionManager : MonoBehaviour { 
   [SerializeField] private Esp32SocketClient socketClient;
   private Animator animator;
   private float waterLevel;      // Current water level (update this based on your game logic)
   private float thresholdTooMuchWater = 0.0f; // Threshold for StateA
   private float thresholdNotEnoughWater = 0.0f; // Threshold for StateB
   private float thresholdTooWarm = 0.0f; // Threshold for StateA
   private float thresholdNotWarmEnough = 0.0f; // Threshold for StateB
   private long lightTimer = 0; // Threshold for StateB
   [SerializeField] private CropsLoader cropsLoader;
   private CSVConverter.Crop Crop;
   private CSVConverter.Soil Soil;
   private CSVConverter.GrowthStage growthStage;
   private int currentStage = 0;
   private float thresholdTooMuchNitrogen;
   private float thresholdNotEnoughNitrogen;
   private float thresholdTooMuchPhosphorous;
   private float thresholdNotEnoughPhosphorous;
   private float thresholdTooMuchPotassium;
   private float thresholdNotEnoughPotassium;


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
    
    
    private void CheckLightState(bool light, long lightOnTime) {
        if (light && lightOnTime >= lightTimer) {
            TriggerState("TurnLightOff");
        } else if (!light && lightOnTime < lightTimer) {
            TriggerState("TurnLightOn");
        } 
        
    }
    
    
    private void CheckNitrogenState(float nitrogen){
        if (nitrogen > thresholdTooMuchNitrogen) {
            TriggerState("HighNitrogen");
        } else if (nitrogen < thresholdNotEnoughNitrogen) {
            TriggerState("LowNitrogen");
        }
    }
    
    private void CheckPhosphorousState(float phosphorous){
        if (phosphorous > thresholdTooMuchPhosphorous) {
            TriggerState("HighPhosphorous");
        } else if (phosphorous < thresholdNotEnoughPhosphorous) {
            TriggerState("LowPhosphorous");
        }
    }
    
    private void CheckPotassiumState(float potassium){
        if (potassium > thresholdTooMuchPotassium) {
            TriggerState("HighPotassium");
        } else if (potassium < thresholdNotEnoughPotassium) {
            TriggerState("LowPotassium");
        }
    }
    
    private void TriggerState(string triggerName){
        // Reset all other triggers to avoid conflicting transitions
        ResetAllTriggers();

        // Set the trigger for the desired state
        animator.SetTrigger(triggerName);
    }
    private void ResetAllTriggers() {
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

    public void StartMonotoring(){
        Crop = cropsLoader.currentCrop;
        Soil = Crop.Soils.First();
        currentStage = 0;
        growthStage = Soil.GrowthStages[currentStage];
        thresholdNotEnoughWater = growthStage.Humidity.Min;
        thresholdTooMuchWater = growthStage.Humidity.Max;
        thresholdNotWarmEnough = growthStage.Temperature.Day.Min;
        thresholdTooWarm = growthStage.Temperature.Day.Max; 
        long averageHours = (growthStage.Light.Period.Max + growthStage.Light.Period.Min) / 2;
        lightTimer = averageHours * 60 * 60;
        thresholdTooMuchNitrogen = growthStage.Nutrients.Nitrogen.Max;
        thresholdNotEnoughNitrogen = growthStage.Nutrients.Nitrogen.Min;
        thresholdTooMuchPhosphorous = growthStage.Nutrients.Phosphorus.Max;
        thresholdNotEnoughPhosphorous = growthStage.Nutrients.Phosphorus.Min;
        thresholdTooMuchPotassium = growthStage.Nutrients.Potassium.Max;
        thresholdNotEnoughPotassium = growthStage.Nutrients.Potassium.Min;
        
        socketClient.SendMessageToEsp32("Lights Timer 0");
        socketClient.SendMessageToEsp32("Lights ON 0");
        
    }

}
