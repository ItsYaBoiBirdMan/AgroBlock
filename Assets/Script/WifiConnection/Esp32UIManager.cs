using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.WifiConnection {
    public class Esp32UIManager : MonoBehaviour {
        [SerializeField] private Esp32SocketClient socketClient;
        [SerializeField] private Text temperatureText;
        [SerializeField] private Text humidityText;
        [SerializeField] private Text lightText;
        [SerializeField] private String messageText;
        [SerializeField] private Text nitrogenText;
        [SerializeField] private Text phosphorousText;
        [SerializeField] private Text potassiumText;


        void Start() {
            // Subscribe to events
            if(temperatureText){temperatureText.text = null; socketClient.OnTemperatureDataReceived += UpdateTemperatureUI;}
            if(humidityText){humidityText.text = null; socketClient.OnHumidityDataReceived += UpdateHumidityUI;}
            if(lightText){lightText.text = null; socketClient.OnLightDataReceived += UpdateLightUI;}
            if(nitrogenText){nitrogenText.text = null; socketClient.OnNitrogenDataReceived += UpdateNitrogenUI;}
            if(phosphorousText){phosphorousText.text = null; socketClient.OnPhosphorousDataReceived += UpdatePhosphorousUI;}
            if(potassiumText){potassiumText.text = null; socketClient.OnPotassiumDataReceived += UpdatePotassiumUI;}
            
            if (socketClient.isConnected){
                socketClient.SendMessageToEsp32(messageText);
                socketClient.SendMessageToEsp32("NPK esp 0");
            } else {
                Debug.Log("Not Connected");
            }
            InvokeRepeating(nameof(SendMessagePeriodically), 0.5f, 0.5f);
        }
        void SendMessagePeriodically() {
            if (socketClient.isConnected) {
                socketClient.SendMessageToEsp32(messageText);  // Sends the same message each time
                socketClient.SendMessageToEsp32("NPK esp 0");
            } else {
                Debug.Log("Not Connected");
            }
        }

        void Update()
        {
            if ((!temperatureText || temperatureText.text != null) && (!humidityText || humidityText.text != null) &&
                (!lightText || lightText.text != null)) return;
            if (socketClient.isConnected) {
                socketClient.SendMessageToEsp32(messageText);
            } else {
                socketClient.StartUDPDiscovery();
            }
            
        }

        // Button handlers
        public void OnSendStatusRequest() {
            socketClient.SendMessageToEsp32("{\"request\":\"status\"}");
        }

        public void OnSendSensorDataRequest() {
            socketClient.SendMessageToEsp32("{\"request\":\"sensor_data\"}");
        }

        // Event handlers
        private void UpdateTemperatureUI(float temperature) {
            temperatureText.text = "Temperature:" + temperature + "Cº";
        }
        
        private void UpdateLightUI(float lightData) {
            lightText.text = "Light: " + lightData + "%";
        }

        private void UpdateHumidityUI(float humidity) {
            humidityText.text = "humidity: " + humidity + "%";
        }

        private void UpdateNitrogenUI(float nitrogen) {
            nitrogenText.text = "Nitrogen:" + nitrogen + "mg/kg";
        }
        
        private void UpdatePhosphorousUI(float phosphorous) {
            phosphorousText.text = "Phosphorous: " + phosphorous + "mg/kg";
        }

        private void UpdatePotassiumUI(float potassium) {
            potassiumText.text = "Potassium: " + potassium + "mg/kg";
        }

        

        void OnDestroy() {
            // Unsubscribe from events to avoid memory leaks
            socketClient.OnTemperatureDataReceived -= UpdateLightUI;
            socketClient.OnHumidityDataReceived -= UpdateHumidityUI;
            socketClient.OnLightDataReceived -= UpdateTemperatureUI;
        }
    }
}