using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace Script.StateMachine {
    public class CropsLoader : MonoBehaviour {
        internal List<CSVConverter.Crop> allCcrops { get; private set; }
        internal List<CSVConverter.Crop> CropsWithSoilType { get; private set; }
        private string currentSoil;
        public CSVConverter.Crop currentCrop { get; private set; }
        internal bool cropSelected { get; private set; }
        internal bool cropsLoaded { get; private set; }

        // Start is called before the first frame update
        void Start() {
            string inputFilePath = "Crops.json"; // Ensure the correct JSON filename
            string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "JSON", inputFilePath);

            if (!File.Exists(jsonFilePath)) {
                Debug.LogError($"JSON file not found at path: {jsonFilePath}");
                return;
            }
            try {
                allCcrops = JsonConvert.DeserializeObject<List<CSVConverter.Crop>>(File.ReadAllText(jsonFilePath));
                cropsLoaded = true;
                Debug.Log($"Loaded {allCcrops.Count} crops from JSON.");
            } catch (Exception ex) {
                Debug.LogError($"Failed to load crops: {ex.Message}");
            }
        }

        public List<CSVConverter.Crop> SelectSoilType(string soilType) {
            if (allCcrops == null || allCcrops.Count == 0) {
                Debug.LogWarning("No crops available to search.");
                return new List<CSVConverter.Crop>();
            }

            // Find all crops with matching soil types
            var matchingCrops = allCcrops
                .Where(crop => crop.Soils.Any(soil => soil.Type.Equals(soilType, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (matchingCrops.Count == 0) {
                Debug.LogWarning($"No crops found with soil type '{soilType}'.");
            } else {
                Debug.Log($"Found {matchingCrops.Count} crop(s) with soil type '{soilType}'.");
            }

            CropsWithSoilType = matchingCrops;
            currentSoil = soilType;
            Debug.Log("GOT HERE");
            return matchingCrops;
        }
        
       

        public CSVConverter.Crop SelectCrop(string cropName) {
            if (CropsWithSoilType == null || CropsWithSoilType.Count == 0) {
                Debug.LogWarning("No crops available for the current soil type. Select a soil type first.");
                return null;
            }

            // Find the crop with the specified name
            var matchingCrop = CropsWithSoilType
                .FirstOrDefault(crop => crop.Name.Equals(cropName, StringComparison.OrdinalIgnoreCase));

            if (matchingCrop == null) {
                Debug.LogWarning($"No crop found with name '{cropName}' for soil type '{currentSoil}'.");
                return null;
            }

            // Filter soils for the selected crop to match the current soil type
            matchingCrop.Soils = matchingCrop.Soils
                .Where(soil => soil.Type.Equals(currentSoil, StringComparison.OrdinalIgnoreCase))
                .ToList();

            currentCrop = matchingCrop;
            cropSelected = true;
            Debug.Log($"Selected crop '{cropName}' with soil type '{currentSoil}'.");
            return currentCrop;
        }

        public void GenerateTestValues(){
           List<TestCrop> examples= new List<TestCrop>();
           Stopwatch stopwatch = new Stopwatch();
           stopwatch.Start();

           foreach (CSVConverter.Crop crop in allCcrops) {
               foreach (var soil in crop.Soils) {
                   foreach (var growthStage in soil.GrowthStages){
                       for (int i = 0; i < 300; i++) {
                           int soilHumidityMin = Math.Min(growthStage.Humidity.Min, growthStage.Humidity.Max);
                           int soilHumidityMax = Math.Max(growthStage.Humidity.Min, growthStage.Humidity.Max);
                           if (soilHumidityMin > soilHumidityMax) {
                               Debug.LogError($"Error in data for Crop: {crop.Name}, Soil: {soil.Type}, Stage: {growthStage.Stage} - MinHumidity: {soilHumidityMin}, MaxHumidity: {soilHumidityMax}");
                           }
                           int dayTempMin = growthStage.Temperature.Day.Min;
                           int dayTempMax = growthStage.Temperature.Day.Max; 
                           int nightTempMin = growthStage.Temperature.Night.Min;
                           int nightTempMax = growthStage.Temperature.Night.Max; 
                           int lightMax = growthStage.Light.Period.Max;
                           int lightMin = growthStage.Light.Period.Min;
                           int nitrogenMax = growthStage.Nutrients.Nitrogen.Max;
                           int nitrogenMin = growthStage.Nutrients.Nitrogen.Min;
                           int phosphorusMax = growthStage.Nutrients.Phosphorus.Max;
                           int phosphorusMin = growthStage.Nutrients.Phosphorus.Min;
                           int potassiumMax = growthStage.Nutrients.Potassium.Max;
                           int potassiumMin = growthStage.Nutrients.Potassium.Min;
                           int timeMax = growthStage.Time.Max;
                           int timeMin = growthStage.Time.Min;
                           int time = GenerateValue(timeMin, timeMax);
                           int soilHumidity = GenerateValue(soilHumidityMin, soilHumidityMax);
                           int dayTemp = GenerateValue(dayTempMin, dayTempMax);
                           int nightTemp = GenerateValue(nightTempMin, nightTempMax);
                           int lights = GenerateValue(lightMin, lightMax);
                           int nitrogen = GenerateValue(nitrogenMin, nitrogenMax);
                           int phosphorus = GenerateValue(phosphorusMin, phosphorusMax);
                           int potassium = GenerateValue(potassiumMin, potassiumMax);
                           
                           Temperature temp = new Temperature {
                               Day = dayTemp,
                               Night = nightTemp
                           };
                           Nutrients nut = new Nutrients {
                               Nitrogen = nitrogen,
                               Phosphorus = phosphorus,
                               Potassium = potassium
                           };
                           GrowthStage gs = new GrowthStage {
                               TestStage = growthStage.Stage, // Unique growth stage
                               TestTime = time,
                               TestHumidity = soilHumidity,
                               TestTemperature = temp,
                               TestLight = lights,
                               TestNutrients = nut
                           };
                           Soil sl = new Soil{
                               TestType = soil.Type, // A unique soil type (e.g., "Clay" instead of "Loamy")
                               TestGrowthStages = new List<GrowthStage> { gs } // Add the growth stage to the list
                           };
                           
                           TestCrop testCrop = new TestCrop {
                               TestName = crop.Name, // A unique crop name (e.g., "Tomato Crop")
                               TestSoils = new List<Soil> { sl } // Add the soil to the list
                           };
                           examples.Add(testCrop);
                       }
                   }
               }
           }
           // Stop the stopwatch
           stopwatch.Stop();

           // Print the elapsed time
           Debug.Log($"Time taken to generate test values: {stopwatch.ElapsedMilliseconds} ms");
           Debug.Log($"Number of examples = : {examples.Count} ");
           string json = JsonConvert.SerializeObject(examples, Formatting.Indented);
           string folderPath = Path.Combine(Application.dataPath, "JSON");
           
        
           // Ensure the folder exists
           if (!Directory.Exists(folderPath))
           {
               Directory.CreateDirectory(folderPath);
           }

           // Define the full file path
           string filePath = Path.Combine(folderPath, "SampleCrops.json");
           

           // Write the JSON to the file
           File.WriteAllText(filePath, json);

           string csv = ConvertToCsv(examples);
           
            folderPath = Path.Combine(Application.dataPath, "CSV");
            // Define the full file path
             filePath = Path.Combine(folderPath, "SampleCrops.csv");
             File.WriteAllText(filePath, csv);
           
            Debug.Log(" it is done ms");

        }
        
        // Method to generate random values within or slightly out of range
        static int GenerateValue(int min, int max, double outlierChance = 0.05, double outlierMultiplier = 0.2) {
            if (min > max) {
                Debug.LogError($"Invalid range: min ({min}) > max ({max}). Check the dataset or logic.");
                throw new ArgumentOutOfRangeException("minValue", min, "Min cannot be greater than max.");
            }
            Random random = new Random();
            bool isOutlier = random.NextDouble() < outlierChance;
            if (isOutlier) {
                // Generate an out-of-range value slightly above or below the range
                double direction = random.NextDouble() < 0.5 ? -1 : 1; // Randomly add or subtract
                return (int)(random.Next(min, max + 1) + direction * (max - min) * outlierMultiplier);
            }
            // Generate a value within the range
            return random.Next(min, max + 1);
        }
        
        public string ConvertToCsv(List<TestCrop> testCrops)
        {
            // Create a StringBuilder for the CSV data
            StringBuilder csvBuilder = new StringBuilder();

            // Add the header row
            csvBuilder.AppendLine("TestName,TestType,TestStage,TestTime,TestHumidity,DayTemperature,NightTemperature,TestLight,Nitrogen,Phosphorus,Potassium");

            // Iterate through the test crops to build CSV rows
            foreach (var crop in testCrops)
            {
                foreach (var soil in crop.TestSoils)
                {
                    foreach (var stage in soil.TestGrowthStages)
                    {
                        // Create a CSV row
                        string row = string.Join(",",
                            crop.TestName,                     // TestName
                            soil.TestType,                     // TestType
                            stage.TestStage,                   // TestStage
                            stage.TestTime,                    // TestTime
                            stage.TestHumidity,                // TestHumidity
                            stage.TestTemperature.Day,         // DayTemperature
                            stage.TestTemperature.Night,       // NightTemperature
                            stage.TestLight,                   // TestLight
                            stage.TestNutrients.Nitrogen,      // Nitrogen
                            stage.TestNutrients.Phosphorus,    // Phosphorus
                            stage.TestNutrients.Potassium      // Potassium
                        );

                        csvBuilder.AppendLine(row);
                    }
                }
            }

            return csvBuilder.ToString();
        }
        
        
        
        public class TestCrop
        {
            public string TestName;
            public List<Soil> TestSoils;
        }

        public class Soil
        {
            public string TestType;
            public List<GrowthStage> TestGrowthStages;
        }

        public class GrowthStage
        {
            public string TestStage;
            public int TestTime;
            public int TestHumidity;
            public Temperature TestTemperature;
            public int TestLight;
            public Nutrients TestNutrients;
        }

        

        public class Temperature
        {
            public int Day;
            public int Night;
        }
        
        public class Nutrients
        {
            public int Nitrogen;
            public int Phosphorus;
            public int Potassium;
        }
        
    
    }
}
