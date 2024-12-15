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
    [SerializeField] private GameObject BackButton;
    [SerializeField] private Button StartButton;
    // Start is called before the first frame update
    public CSVConverter.Crop Crop{ private set; get; }
    void Start()
    {
        string inputFilePath = "CurrentCrop.json"; // Ensure the correct JSON filename
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "JSON", inputFilePath);
        if (!File.Exists(jsonFilePath)){
            hasCrop = false;
        } else {
            try {
                Crop = JsonConvert.DeserializeObject<CSVConverter.Crop>(File.ReadAllText(jsonFilePath));
                ConditionManager.StartMonotoring(Crop);
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
        } else {
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
        string json = JsonConvert.SerializeObject(true, Formatting.Indented);
        string folderPath = Path.Combine(Application.persistentDataPath, "JSON");
        // Ensure the folder exists
        if (!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
        }
        // Define the full file path
        string filePath = Path.Combine(folderPath, "FirstTime.json");
        // Write the JSON to the file
        File.WriteAllText(filePath, json);
    }
}
