using System;
using System.Collections;
using System.Collections.Generic;
using Script.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CropsDisplayer : MonoBehaviour {
    [SerializeField] private StartScript startScript;
    [SerializeField] private ConditionManager ConditionManager;
    [SerializeField] private GameObject itemPrefab;  // Prefab for each item (a button or custom prefab)
    [SerializeField] private Transform contentPanel; // The Content object inside the ScrollView
    [SerializeField] private CropsLoader cropsLoader;
    [SerializeField] private Transform conditionsLoader;
    [SerializeField] private Transform SelectorMenu;
    [SerializeField] private Transform HomeMenu;
    [SerializeField] private Transform Notifications;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TextMeshProUGUI humidityText;
    [SerializeField] private TextMeshProUGUI temperatureText;
    [SerializeField] private TextMeshProUGUI lightsText;
    [SerializeField] private TextMeshProUGUI nText;
    [SerializeField] private TextMeshProUGUI pText;
    [SerializeField] private TextMeshProUGUI kText;
    [SerializeField] private Button startMonitoringButton;
    private bool isContentPanelActive = false;
    private CSVConverter.Crop selectedCrop;
    private int selectedSoil;

    private void Start()
    {
        // Add a listener to detect value changes in the dropdown
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        startMonitoringButton.onClick.AddListener(OnStartMonitoringButton);
    }


    private void Update() {
        if (contentPanel.gameObject.activeSelf && !isContentPanelActive)
        {
            isContentPanelActive = true;
            PopulateScrollView();
        }
        else if (!contentPanel.gameObject.activeSelf && isContentPanelActive)
        {
            isContentPanelActive = false;
        }
    }

    private void PopulateScrollView() {
        foreach (Transform child in contentPanel){
            Destroy(child.gameObject);
        }

        foreach (CSVConverter.Crop crop in cropsLoader.allCcrops) {
            string name = crop.Name;
            
            GameObject newItem = Instantiate(itemPrefab, contentPanel); // Instantiate prefab as a child of Content
            newItem.name = name; // Set the name of the GameObject for debugging purposes

            // Get the Text component (assuming the prefab has a child Text component)
            Image itemiImage = newItem.transform.Find("Image").GetComponent<Image>();
            if (itemiImage != null) {
                string imageName = name + "_selector"; // e.g., "item1_selector"
                string path = "Images/" + imageName;
                Texture2D texture = Resources.Load<Texture2D>(path); // Assuming the image is in Resources/Images/image.png

                if (texture != null){
                    // Apply the loaded texture to the UI Image component
                    itemiImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }

            // Add an OnClick event listener to the button
            Button button = newItem.GetComponent<Button>();
            if (button != null){
                button.onClick.AddListener(() => OnItemClick(crop));
            }
            
        }
    }

    private void OnItemClick(CSVConverter.Crop crop) {
        selectedCrop = crop;
        selectedSoil = 0;
        conditionsLoader.gameObject.SetActive(true);
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var soil in crop.Soils) {
            options.Add(soil.Type);
        }
        dropdown.AddOptions(options);
        dropdown.value = 0;
        dropdown.RefreshShownValue();
        CSVConverter.GrowthStage growthStage = crop.Soils[0].GrowthStages[0];
        
        humidityText.text = "Min: " + growthStage.Humidity.Min.ToString("0.0") + "%" + "     Max: " + growthStage.Humidity.Max.ToString("0.0") + "%";
        temperatureText.text = "Min: " + growthStage.Temperature.Day.Min.ToString("0.0") + "Cº" + "     Max: " + growthStage.Temperature.Day.Max.ToString("0.0") + "Cº";
        lightsText.text = "Min: " + growthStage.Light.Period.Min.ToString("0.0") + "H" + "     Max: " + growthStage.Light.Period.Max.ToString("0.0") + "H";
        nText.text = "Min: " + growthStage.Nutrients.Nitrogen.Min.ToString("0.0") + "mg/kg" + "     Max: " + growthStage.Nutrients.Nitrogen.Max.ToString("0.0") + "mg/kg";;
        pText.text = "Min: " + growthStage.Nutrients.Phosphorus.Min.ToString("0.0") + "mg/kg" + "     Max: " + growthStage.Nutrients.Phosphorus.Max.ToString("0.0") + "mg/kg";;
        kText.text = "Min: " + growthStage.Nutrients.Potassium.Min.ToString("0.0") + "mg/kg" + "     Max: " + growthStage.Nutrients.Potassium.Max.ToString("0.0") + "mg/kg";;
        
    }
    
    // This method will be called whenever the dropdown value changes
    private void OnDropdownValueChanged(int value){
        Debug.Log("Value: "+ value);
        selectedSoil = value;
        CSVConverter.GrowthStage growthStage = selectedCrop.Soils[value].GrowthStages[0];
        humidityText.text = "Min: " + growthStage.Humidity.Min.ToString("0.0") + "%" + "     Max: " + growthStage.Humidity.Max.ToString("0.0") + "%";
        temperatureText.text = "Min: " + growthStage.Temperature.Day.Min.ToString("0.0") + "Cº" + "     Max: " + growthStage.Temperature.Day.Max.ToString("0.0") + "Cº";
        lightsText.text = "Min: " + growthStage.Light.Period.Min.ToString("0.0") + "H" + "     Max: " + growthStage.Light.Period.Max.ToString("0.0") + "H";
        nText.text = "Min: " + growthStage.Nutrients.Nitrogen.Min.ToString("0.0") + "mg/kg" + "     Max: " + growthStage.Nutrients.Nitrogen.Max.ToString("0.0") + "mg/kg";;
        pText.text = "Min: " + growthStage.Nutrients.Phosphorus.Min.ToString("0.0") + "mg/kg" + "     Max: " + growthStage.Nutrients.Phosphorus.Max.ToString("0.0") + "mg/kg";;
        kText.text = "Min: " + growthStage.Nutrients.Potassium.Min.ToString("0.0") + "mg/kg" + "     Max: " + growthStage.Nutrients.Potassium.Max.ToString("0.0") + "mg/kg";;

    }

    private void OnStartMonitoringButton() {
        CSVConverter.Crop crop = new CSVConverter.Crop();
        crop.Name = selectedCrop.Name;
        List<CSVConverter.Soil> soils = new List<CSVConverter.Soil>();
        soils.Add(selectedCrop.Soils[selectedSoil]);
        crop.Soils = soils;
        startScript.SaveCropIntoFile(crop);
        startScript.SavetimeIntoFile();
        ConditionManager.SaveStageIntoFile(0);
        ConditionManager.StartMonotoring(crop);
        conditionsLoader.gameObject.SetActive(false);
        SelectorMenu.gameObject.SetActive(false);
        HomeMenu.gameObject.SetActive(true);
        Notifications.gameObject.SetActive(true);
    }
    
    private void OnDestroy(){
        // Make sure to remove the listener when the object is destroyed to avoid memory leaks
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
}
