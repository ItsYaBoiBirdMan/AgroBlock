using System;
using System.Collections;
using System.Collections.Generic;
using Script.WifiConnection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI minThreshold;
    [SerializeField] private TextMeshProUGUI maxThreshold;
    [SerializeField] private DataType datatype;
    [SerializeField] private Esp32SocketClient socketClient;
    private float min;
    private float max;
    

    private void SetPercent(float value) {
        float percentage = (value - min) / (max - min);
        fill.fillAmount = Mathf.Clamp01(percentage);
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
        else
        {
            fill.color = new Color(0.3f, 0.9f, 0.47f);
            background.color = new Color(.7f, .7f, .7f);
        }
        
    }
    private void Update()
    {
        CheckBarStatus();
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
        switch (datatype) {
            case DataType.Temperature:
                socketClient.OnTemperatureDataReceived -= SetPercent;
                socketClient.OnTemperatureDataReceived += SetPercent;
                if (day) {
                    min = crop.Soils[0].GrowthStages[stage].Temperature.Day.Min;
                    max = crop.Soils[0].GrowthStages[stage].Temperature.Day.Max;

                }
                else {
                    min = crop.Soils[0].GrowthStages[stage].Temperature.Night.Min;
                    max = crop.Soils[0].GrowthStages[stage].Temperature.Night.Max;
                }
                break;
            case DataType.Humidity:
                socketClient.OnHumidityDataReceived -= SetPercent;
                socketClient.OnHumidityDataReceived += SetPercent;
                min = crop.Soils[0].GrowthStages[stage].Humidity.Min;
                max = crop.Soils[0].GrowthStages[stage].Humidity.Max;
                break;
            case DataType.Light:
                socketClient.OnLightDataReceived -= SetPercent;
                socketClient.OnLightDataReceived += SetPercent;
                min = crop.Soils[0].GrowthStages[stage].Light.Period.Min;
                max = crop.Soils[0].GrowthStages[stage].Light.Period.Max;
                break;
            case DataType.Nitrogen:
                socketClient.OnNitrogenDataReceived -= SetPercent;
                socketClient.OnNitrogenDataReceived += SetPercent;
                min = crop.Soils[0].GrowthStages[stage].Nutrients.Nitrogen.Min;
                max = crop.Soils[0].GrowthStages[stage].Nutrients.Nitrogen.Max;
                break;
            case DataType.Phosphorus:
                socketClient.OnPhosphorousDataReceived -= SetPercent;
                socketClient.OnPhosphorousDataReceived += SetPercent;
                min = crop.Soils[0].GrowthStages[stage].Nutrients.Phosphorus.Min;
                max = crop.Soils[0].GrowthStages[stage].Nutrients.Phosphorus.Max;
                break;
            case DataType.Potassium:
                socketClient.OnPotassiumDataReceived -= SetPercent;
                socketClient.OnPotassiumDataReceived += SetPercent;
                min = crop.Soils[0].GrowthStages[stage].Nutrients.Potassium.Min;
                max = crop.Soils[0].GrowthStages[stage].Nutrients.Potassium.Max;
                break;
        }
        SetMin(min);
        SetMax(max);
    }

}
