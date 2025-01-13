using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Script.StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
    private bool hasCrop;
    private bool firstTime;
    [SerializeField] private ConditionManager ConditionManager;
    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject SelectorMenu;
    [SerializeField] private GameObject HomeMenu;
    [SerializeField] private GameObject BackButton;
    [SerializeField] private Button StartButton;
    [SerializeField] private BarManager barManager1;
    [SerializeField] private BarManager barManager2;
    [SerializeField] private BarManager barManager3;
    [SerializeField] private BarManager barManager4;
    [SerializeField] private BarManager barManager5;
    [SerializeField] private BarManager barManager6;

    internal int CurrentStage;
    // Start is called before the first frame update
    public CSVConverter.Crop Crop{ private set; get; }
    void Start() {
        string inputFilePath = "CurrentCrop.json"; // Ensure the correct JSON filename
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "JSON", inputFilePath);
        if (!File.Exists(jsonFilePath)){
            hasCrop = false;
        } else {
            try {
                Crop = JsonConvert.DeserializeObject<CSVConverter.Crop>(File.ReadAllText(jsonFilePath));
  //              
                hasCrop = true;
            } catch (Exception ex) {
                Debug.LogError($"Failed to load crops: {ex.Message}");
            } 
        }
        
        inputFilePath = "FirstTime.json"; // Ensure the correct JSON filename
        jsonFilePath = Path.Combine(Application.persistentDataPath, "JSON", inputFilePath);
        firstTime = !File.Exists(jsonFilePath);
        StartButton.onClick.AddListener(OnStartButtonClick);
    }
    private void OnStartButtonClick() {
        if (firstTime) {
            
        } /*else*/ if (!hasCrop) {
            StartMenu.SetActive(false);
            SelectorMenu.SetActive(true);
            BackButton.SetActive(false);
            ConditionManager.StartMonotoring(Crop);
        } else {
            ConditionManager.StartMonotoring(Crop);
            updateBars();
            StartMenu.SetActive(false);
            HomeMenu.SetActive(true);
            
            //TODO
        }
    }

    public void SaveCropIntoFile(CSVConverter.Crop crop) {
        Crop = crop;
        string json = JsonConvert.SerializeObject(crop, Formatting.Indented);
        string folderPath = Path.Combine(Application.persistentDataPath, "JSON");
        // Ensure the folder exists
        if (!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
        }
        // Define the full file path
        string filePath = Path.Combine(folderPath, "CurrentCrop.json");
        // Write the JSON to the file
        File.WriteAllText(filePath, json);
    }
    public void SavetimeIntoFile() {
        // The value inside is irrelevant
        string json = JsonConvert.SerializeObject(false, Formatting.Indented);
        string folderPath = Path.Combine(Application.persistentDataPath, "JSON");
        // Ensure the folder exists
        if (!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
        }
        string filePath = Path.Combine(folderPath, "FirstTime.json");
        // Write the JSON to the file
        File.WriteAllText(filePath, json);
    }


    public void updateBars() {
        bool day = true;
        string inputFilePath = "CurrentStage.json"; // Ensure the correct JSON filename
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "JSON", inputFilePath);
        // Create the JSON file if it doesn't exist
        if (!File.Exists(jsonFilePath)){
            Debug.Log($"JSON file not found at path: {jsonFilePath}. Creating a new file with default content.");
            // Default stage value (for example, 0)
            CurrentStage = 0;
        }
        try {
            CurrentStage = JsonConvert.DeserializeObject<int>(File.ReadAllText(jsonFilePath));
            Debug.Log($"Loaded stage index:{CurrentStage}");
            if (CurrentStage == -1) {
                CurrentStage = 0;
            }
        } catch (Exception ex) {
            Debug.LogError($"Failed to load crops: {ex.Message}");
        }
        
        inputFilePath = "CropDayNight.json"; // Ensure the correct JSON filename
        jsonFilePath = Path.Combine(Application.persistentDataPath, "JSON", inputFilePath);
        if (!File.Exists(jsonFilePath)){
            Debug.Log($"JSON file not found at path: {jsonFilePath}. Creating a new file with default content.");
            // Default stage value (for example, 0)
            day = true;
        }
        try {
            day = JsonConvert.DeserializeObject<bool>(File.ReadAllText(jsonFilePath));
            Debug.Log($"Loaded day index:{day}");
        } catch (Exception ex) {
            Debug.LogError($"Failed to load crops: {ex.Message}");
        }
        Debug.Log("IDK WHATS GOING ON");
        Debug.Log(Crop);
        Debug.Log(CurrentStage);
        Debug.Log(day);
        barManager1.StartBar(Crop, CurrentStage, day);
        barManager2.StartBar(Crop, CurrentStage, day);
        barManager3.StartBar(Crop, CurrentStage, day);
        barManager4.StartBar(Crop, CurrentStage, day);
        barManager5.StartBar(Crop, CurrentStage, day);
        barManager6.StartBar(Crop, CurrentStage, day);
        
    }
}
