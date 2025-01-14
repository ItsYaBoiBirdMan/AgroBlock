using System;
using System.Collections;
using System.Collections.Generic;
using Script.WifiConnection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI minThreshold;
    [SerializeField] private TextMeshProUGUI maxThreshold;
    [SerializeField] private TextMeshProUGUI refreshText;
    [SerializeField] private DataType datatype;
    [SerializeField] private Esp32SocketClient socketClient;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button lightButton;
    [SerializeField] private GameObject TemperatureIcon;
    [SerializeField] private StartScript StartScript;
    private float min;
    private float max;
    private bool firstTime = true;
    private bool lightOnOff;
    private int cropStage;
    private bool day;
    

    private void SetPercent(float value) {
        Debug.Log("Value: " + value) ;
        float percentage = (value - min) / (max - min);
        fill.fillAmount = Mathf.Clamp01(percentage);
    }
    private void SetPercentLights (bool lights , long value ) {
        Debug.Log("Lights: " + lights + " value: " + value) ;
        float percentage = (value - min) / (max - min);
        fill.fillAmount = Mathf.Clamp01(percentage);
        lightOnOff = lights;
        setLights(lightOnOff);
    }
    
    private void SetPercentTemperature (float temperature , long dayNightTimer ) {
        Debug.Log("temperature: " + temperature) ;
        if (dayNightTimer < StartScript.Crop.Soils[0].GrowthStages[cropStage].Light.Period.Max*3600) {
            min = StartScript.Crop.Soils[0].GrowthStages[cropStage].Temperature.Day.Min;
            max = StartScript.Crop.Soils[0].GrowthStages[cropStage].Temperature.Day.Max;
            day = true;
        } else {
            min = StartScript.Crop.Soils[0].GrowthStages[cropStage].Temperature.Night.Min;
            max = StartScript.Crop.Soils[0].GrowthStages[cropStage].Temperature.Night.Max;
            day = false;
        }
        SetMin(min);
        SetMax(max);
        float percentage = (temperature - min) / (max - min);
        fill.fillAmount = Mathf.Clamp01(percentage);
        setTemperatureIcon(day);
    }

    

    public void SetMin(float min)
    {
        minThreshold.text = "Min: " + min;
    }
    
    public void SetMax(float max)
    {
        maxThreshold.text = "Max: " + max;
    }

    private void CheckBarStatus()
    {
        if (fill.fillAmount  >= 1)
        {
            fill.color = Color.red;
        }
        else if (fill.fillAmount <= 0)
        {
            background.color = Color.blue;
        }
        else {
            fill.color = new Color(0.3f, 0.9f, 0.47f);
            background.color = new Color(.7f, .7f, .7f);
        }
        
    }
    private void Update()
    {
        CheckBarStatus();
    }

    public void SetFillAmountValue(float value)
    {
        fill.fillAmount = value;
    }
    
    private enum DataType{
            Humidity,
            Temperature,
            Light,
            Nitrogen,
            Phosphorus,
            Potassium
    }

    public void StartBar(CSVConverter.Crop crop, int stage, bool day) {
        cropStage = stage;
        switch (datatype) {
            case DataType.Temperature:
                if (firstTime){
                    socketClient.OnFullTemperatureDataReceived += SetPercentTemperature;
                    socketClient.OnFullTemperatureDataReceived += setRefreshText3;
                }
                if (day){
                    min = crop.Soils[0].GrowthStages[stage].Temperature.Day.Min;
                    max = crop.Soils[0].GrowthStages[stage].Temperature.Day.Max;
                } else {
                    min = crop.Soils[0].GrowthStages[stage].Temperature.Night.Min;
                    max = crop.Soils[0].GrowthStages[stage].Temperature.Night.Max;
                }
                break;
            case DataType.Humidity:
                if (firstTime){
                    socketClient.OnHumidityDataReceived += SetPercent;
                    socketClient.OnHumidityDataReceived += setRefreshText;
                }
                min = crop.Soils[0].GrowthStages[stage].Humidity.Min;
                max = crop.Soils[0].GrowthStages[stage].Humidity.Max;
                break;
            case DataType.Light:
                if (firstTime){
                    socketClient.LightStateDataReceived += SetPercentLights;
                    socketClient.LightStateDataReceived += setRefreshText2;
                    lightButton.onClick.AddListener(ToggleLights);
                }
                min = crop.Soils[0].GrowthStages[stage].Light.Period.Min;
                max = crop.Soils[0].GrowthStages[stage].Light.Period.Max;
                break;
            case DataType.Nitrogen:
                if (firstTime){
                    socketClient.OnNitrogenDataReceived += SetPercent;
                    socketClient.OnNitrogenDataReceived += setRefreshText;
                }
                min = crop.Soils[0].GrowthStages[stage].Nutrients.Nitrogen.Min;
                max = crop.Soils[0].GrowthStages[stage].Nutrients.Nitrogen.Max;
                break;
            case DataType.Phosphorus:
                if (firstTime){
                    socketClient.OnPhosphorousDataReceived += SetPercent;
                    socketClient.OnPhosphorousDataReceived += setRefreshText;
                }
                min = crop.Soils[0].GrowthStages[stage].Nutrients.Phosphorus.Min;
                max = crop.Soils[0].GrowthStages[stage].Nutrients.Phosphorus.Max;
                break;
            case DataType.Potassium:
                if (firstTime){
                    socketClient.OnPotassiumDataReceived += SetPercent;
                    socketClient.OnPotassiumDataReceived += setRefreshText;
                }
                min = crop.Soils[0].GrowthStages[stage].Nutrients.Potassium.Min;
                max = crop.Soils[0].GrowthStages[stage].Nutrients.Potassium.Max;
                break;
        }
        if (firstTime){
            refreshButton.onClick.AddListener(RefreshInfo);
        }
        firstTime = false;
        SetMin(min);
        SetMax(max);
    }

    private void ToggleLights() {
        lightOnOff = !lightOnOff;
        setLights(lightOnOff);
        if (lightOnOff){
            socketClient.SendMessageToEsp32("Lights ON 0");
        } else {
            socketClient.SendMessageToEsp32("Lights OFF 0");
        }
    }
    private void setRefreshText(float value){
        DateTime now = DateTime.Now;
        refreshText.text = "Last Checked: " + now.ToString("HH:mm") + ".";
    }
    private void setRefreshText2(bool value, long value2){
        DateTime now = DateTime.Now;
        refreshText.text = "Last Checked: " + now.ToString("HH:mm") + ".";
    }
    private void setRefreshText3(float value, long value2)
    {
        DateTime now = DateTime.Now;
        refreshText.text = "Last Checked: " + now.ToString("HH:mm") + ".";
    }
    private void RefreshInfo() {
        Debug.Log("PRessed");
        switch (datatype){
         case DataType.Temperature:
             socketClient.SendMessageToEsp32("Temperature esp 0");
                break;
         case DataType.Humidity: 
             socketClient.SendMessageToEsp32("Humidity esp 0");
                break;
         case DataType.Light:
             socketClient.SendMessageToEsp32("Lights esp 0");
                break;
         case DataType.Nitrogen:
             socketClient.SendMessageToEsp32("NPK esp 0");  
                break;
         case DataType.Phosphorus:
             socketClient.SendMessageToEsp32("NPK esp 0");  
                break;
         case DataType.Potassium:
             socketClient.SendMessageToEsp32("NPK esp 0");    
                break;
        }
    }
    private void setLights(bool lights) {
        Debug.Log("IGOTHERE");
        string imageName;
        if (lights){
             imageName = "Lights_on"; // e.g., "item1_selector"
        } else {
             imageName = "Lights_off"; // e.g., "item1_selector"
        }
        
        Image itemiImage = lightButton.GetComponent<Image>();
        if (itemiImage != null){
            
            string path = "Images/" + imageName;
            Texture2D texture = Resources.Load<Texture2D>(path); // Assuming the image is in Resources/Images/image.png

            if (texture != null){
                // Apply the loaded texture to the UI Image component
                itemiImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }
    private void setTemperatureIcon(bool day) {
        string imageName;
        if (day){
            imageName = "Temperature_Day"; // e.g., "item1_selector"
        } else {
            imageName = "Temperature_Night"; // e.g., "item1_selector"
        }
        
        Image itemiImage = TemperatureIcon.GetComponent<Image>();
        if (itemiImage != null){
            
            string path = "Images/" + imageName;
            Texture2D texture = Resources.Load<Texture2D>(path); // Assuming the image is in Resources/Images/image.png

            if (texture != null){
                // Apply the loaded texture to the UI Image component
                itemiImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }
}
