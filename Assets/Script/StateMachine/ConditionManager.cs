
using System;
using System.Collections;
using System.Linq;
using Script.StateMachine;
using Script.WifiConnection;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConditionManager : MonoBehaviour { 
   [SerializeField] private Esp32SocketClient socketClient;
   [SerializeField] private Animator animator;
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
       StartCoroutine(waiter());
        
    }
   IEnumerator waiter()
   {
       yield return new WaitForSecondsRealtime(6);
       socketClient.OnFullTemperatureDataReceived += CheckTemperatureState;
       socketClient.OnHumidityDataReceived += CheckHumidityState;
       socketClient.LightStateDataReceived += CheckLightState;
       socketClient.OnNitrogenDataReceived += CheckNitrogenState;
       socketClient.OnPhosphorousDataReceived += CheckPhosphorousState;
       socketClient.OnPotassiumDataReceived += CheckPotassiumState;
       StartMonotoring();
       /*
       InvokeRepeating(nameof(GetTemperature), 2.0f, 180.0f);
       InvokeRepeating(nameof(GetHumidity), 3.0f, 180.0f);
       InvokeRepeating(nameof(GetLight), 4.0f, 600.0f);
       InvokeRepeating(nameof(GetFertilizer), 5.0f, 3600.0f);
       */
       InvokeRepeating(nameof(GetTemperature), 2.0f, 5f);
       InvokeRepeating(nameof(GetHumidity), 3.0f, 10f);
       InvokeRepeating(nameof(GetLight), 4.0f, 15f);
       InvokeRepeating(nameof(GetFertilizer), 5.0f, 20f);
        
   }

    public void GetTemperature()
    {
        CheckTemperatureState(Random.Range(0, 30), 10);
        //socketClient.SendMessageToEsp32("Temperature esp 0");
    }
    public void GetHumidity() {
        CheckHumidityState(Random.Range(0, 60));
        //socketClient.SendMessageToEsp32("Humidity esp 0");
    }
    public void GetLight()
    {
        int test = Random.Range(0, 1);
        bool testbool;
        if (test == 0) testbool = false;
        else testbool = true;
        CheckLightState( testbool, 10);
        //socketClient.SendMessageToEsp32("Lights state 0");
    }
    public void GetFertilizer() {
        CheckNitrogenState(Random.Range(0, 50));
        CheckPotassiumState(Random.Range(0, 50));
        CheckPhosphorousState(Random.Range(0, 50));
        //socketClient.SendMessageToEsp32("NPK esp 0");
    }

    public void CheckTemperatureState(float temperature,long timer){
        if (timer >= growthStage.Light.Period.Max * 60 * 60) {
            thresholdNotWarmEnough = growthStage.Temperature.Night.Min; 
            thresholdTooWarm = growthStage.Temperature.Night.Max;
        } else {
            thresholdNotWarmEnough = growthStage.Temperature.Day.Min; 
            thresholdTooWarm = growthStage.Temperature.Day.Max;
        }
        
        if (temperature > thresholdTooWarm) {
            TriggerState("HighTemperature");
        } else if (temperature < thresholdNotWarmEnough) {
            TriggerState("LowTemperature");
        }
    }
    
    public void CheckHumidityState(float humidity){
        if (humidity > thresholdTooMuchWater) {
            TriggerState("HighHumidity");
        } else if (humidity < thresholdNotEnoughWater) {
            TriggerState("LowHumidity");
        }
    }
    
    
    public void CheckLightState(bool light, long lightOnTime) {
        if (light && lightOnTime >= lightTimer) {
            TriggerState("TurnLightOff");
        } else if (!light && lightOnTime < lightTimer) {
            TriggerState("TurnLightOn");
        } 
        TriggerState("idle");
    }
    
    
    public void CheckNitrogenState(float nitrogen){
        if (nitrogen > thresholdTooMuchNitrogen) {
            TriggerState("HighNitrogen");
        } else if (nitrogen < thresholdNotEnoughNitrogen) {
            TriggerState("LowNitrogen");
        }
    }
    
    public void CheckPhosphorousState(float phosphorous){
        if (phosphorous > thresholdTooMuchPhosphorous) {
            TriggerState("HighPhosphorous");
        } else if (phosphorous < thresholdNotEnoughPhosphorous) {
            TriggerState("LowPhosphorous");
        }
    }
    
    public void CheckPotassiumState(float potassium){
        if (potassium > thresholdTooMuchPotassium) {
            TriggerState("HighPotassium");
        } else if (potassium < thresholdNotEnoughPotassium) {
            TriggerState("LowPotassium");
        }
    }
    
    public void TriggerState(string triggerName){
      
        // Set the trigger for the desired state
        animator.Play(triggerName);
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
        Debug.Log("monotoring started");
        
        //socketClient.SendMessageToEsp32("Lights Timer 0");
        //socketClient.SendMessageToEsp32("Lights ON 0");
        
    }
/*
    private void Update()
    {
        CheckTemperatureState(20, 864000);
    }*/
}
