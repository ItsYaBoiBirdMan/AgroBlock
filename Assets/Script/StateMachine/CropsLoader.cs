using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

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
            string jsonFilePath = Path.Combine(Application.dataPath, "JSON", inputFilePath);

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
        
    }
}
