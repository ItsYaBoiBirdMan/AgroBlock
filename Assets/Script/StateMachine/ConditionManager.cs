using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using Script.WifiConnection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.StateMachine {
    public class ConditionManager : MonoBehaviour { 
        [SerializeField] private Esp32SocketClient socketClient;
        [SerializeField] private StartScript startScript;
        [SerializeField] private Animator animator;
        private float waterLevel;      // Current water level (update this based on your game logic)
        private float thresholdTooMuchWater; // Threshold for StateA
        private float thresholdNotEnoughWater; // Threshold for StateB
        private float thresholdTooWarm ; // Threshold for StateA
        private float thresholdNotWarmEnough ; // Threshold for StateB
        private long lightTimer; // Threshold for StateB
        // ReSharper disable once InconsistentNaming
        private CSVConverter.Crop Crop;
        // ReSharper disable once InconsistentNaming
        private CSVConverter.Soil Soil;
        private CSVConverter.GrowthStage growthStage;
        private int currentStage ;
        private float thresholdTooMuchNitrogen;
        private float thresholdNotEnoughNitrogen;
        private float thresholdTooMuchPhosphorous;
        private float thresholdNotEnoughPhosphorous;
        private float thresholdTooMuchPotassium;
        private float thresholdNotEnoughPotassium;
        private bool monotoringStarted;


        void Start() {
            StartCoroutine(Waiter());
        
        }
        IEnumerator Waiter() {
            while (!monotoringStarted) {
                yield return new WaitForSecondsRealtime(6);
            }
       
            socketClient.OnFullTemperatureDataReceived += CheckTemperatureState;
            socketClient.OnHumidityDataReceived += CheckHumidityState;
            socketClient.LightStateDataReceived += CheckLightState;
            socketClient.OnNitrogenDataReceived += CheckNitrogenState;
            socketClient.OnPhosphorousDataReceived += CheckPhosphorousState;
            socketClient.OnPotassiumDataReceived += CheckPotassiumState;
            socketClient.DayTimerReceived += CheckStageDayState;
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

        public void CheckTemperatureState(float temperature,long timer) {
            if (timer >= growthStage.Light.Period.Max * 60 * 60) {
                SaveDayNightIntoFile(false);
                thresholdNotWarmEnough = growthStage.Temperature.Night.Min; 
                thresholdTooWarm = growthStage.Temperature.Night.Max;
            } else {
                SaveDayNightIntoFile(true);
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
    
    
        public void CheckLightState(bool lights, long lightOnTime) {
            if (lights && lightOnTime >= lightTimer) {
                TriggerState("TurnLightOff");
            } else if (!lights && lightOnTime < lightTimer) {
                TriggerState("TurnLightOn");
            } else {
                TriggerState("Idle");
            }
        
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
    
        public void CheckStageDayState(long days){
            if (growthStage.Time.Min > days/86.400) {
                //TODO fire event
            }
            else if (growthStage.Time.Max >= days/86.400) {
                currentStage++;
                if (Soil.GrowthStages.Count <= currentStage) {
                    currentStage = -1;
                }
                SaveStageIntoFile(currentStage);
            }
        }
        public void TriggerState(string triggerName){
      
            // Set the trigger for the desired state
            animator.Play(triggerName);
        }
    
        public void StartMonotoring(CSVConverter.Crop crop){
            Crop = crop;
            Soil = Crop.Soils[0];
            string inputFilePath = "CurrentStage.json"; // Ensure the correct JSON filename
            string jsonFilePath = Path.Combine(Application.persistentDataPath, "JSON", inputFilePath);
        
        
        
            // Create the JSON file if it doesn't exist
            if (!File.Exists(jsonFilePath)){
                Debug.Log($"JSON file not found at path: {jsonFilePath}. Creating a new file with default content.");

                // Default stage value (for example, 0)
                currentStage = 0;

                // Save the default value to the JSON file
                SaveStageIntoFile(currentStage); 
            }
            try {
                currentStage = JsonConvert.DeserializeObject<int>(File.ReadAllText(jsonFilePath));
                Debug.Log($"Loaded stage index:{currentStage}");
                if (currentStage == -1) {
                    currentStage = 0;
                    SaveStageIntoFile(currentStage);
                }
            } catch (Exception ex) {
                Debug.LogError($"Failed to load crops: {ex.Message}");
            }
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
            monotoringStarted = true;

            //socketClient.SendMessageToEsp32("Lights Timer 0");
            //socketClient.SendMessageToEsp32("Lights ON 0");

        }

        public void SaveStageIntoFile(int stage) {
            string json = JsonConvert.SerializeObject(stage, Formatting.Indented);
            string folderPath = Path.Combine(Application.persistentDataPath, "JSON");
            // Ensure the folder exists
            if (!Directory.Exists(folderPath)){
                Directory.CreateDirectory(folderPath);
            }
            // Define the full file path
            string filePath = Path.Combine(folderPath, "CurrentStage.json");
            // Write the JSON to the file
            File.WriteAllText(filePath, json);
        }
        public void SaveDayNightIntoFile(bool day) {
            string json = JsonConvert.SerializeObject(day, Formatting.Indented);
            string folderPath = Path.Combine(Application.persistentDataPath, "JSON");
            // Ensure the folder exists
            if (!Directory.Exists(folderPath)){
                Directory.CreateDirectory(folderPath);
            }
            // Define the full file path
            string filePath = Path.Combine(folderPath, "CropDayNight.json");
            // Write the JSON to the file
            File.WriteAllText(filePath, json);
        }
/*
    private void Update()
    {
        CheckTemperatureState(20, 864000);
    }*/
    }
}
