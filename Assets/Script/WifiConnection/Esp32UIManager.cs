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


        void Start() {
            // Subscribe to events
            if(temperatureText){temperatureText.text = null; socketClient.OnTemperatureDataReceived += UpdateLightUI;}
            if(humidityText){humidityText.text = null; socketClient.OnHumidityDataReceived += UpdateHumidityUI;}
            if(lightText){lightText.text = null; socketClient.OnLightDataReceived += UpdateTemperatureUI;}

            if (socketClient.isConnected){
                socketClient.SendMessage(messageText);
            } else {
                socketClient.StartUDPDiscovery();
            }
            InvokeRepeating(nameof(SendMessagePeriodically), 0.5f, 0.5f);
        }
        void SendMessagePeriodically() {
            if (socketClient.isConnected) {
                socketClient.SendMessage(messageText);  // Sends the same message each time
            } else {
                socketClient.StartUDPDiscovery();
            }
        }

        void Update()
        {
            if ((!temperatureText || temperatureText.text != null) && (!humidityText || humidityText.text != null) &&
                (!lightText || lightText.text != null)) return;
            if (socketClient.isConnected) {
                socketClient.SendMessage(messageText);
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
        private void UpdateLightUI(float lightData) {
            lightText.text = "humidity: " + lightData + "%";
        }

        private void UpdateHumidityUI(float humidity) {
            humidityText.text = "humidity: " + humidity + "%";
        }

        private void UpdateTemperatureUI(float temperature) {
            temperatureText.text = $"Temperature: {temperature}" + "Cº";
        }

        void OnDestroy() {
            // Unsubscribe from events to avoid memory leaks
            socketClient.OnTemperatureDataReceived -= UpdateLightUI;
            socketClient.OnHumidityDataReceived -= UpdateHumidityUI;
            socketClient.OnLightDataReceived -= UpdateTemperatureUI;
        }
    }
}