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
        [SerializeField] private Text lightStateText;
        [SerializeField] private Text valveStateText;
        
        [SerializeField] private Button lightStateButton;
        [SerializeField] private Button valveStateButton;

        private bool lightState;
        private bool valveState;

        void Start() {
            // Subscribe to events
            if(temperatureText){temperatureText.text = null; socketClient.OnFullTemperatureDataReceived += UpdateTemperatureUI;}
            if(humidityText){humidityText.text = null; socketClient.OnHumidityDataReceived += UpdateHumidityUI;}
            if(lightText){lightText.text = null; socketClient.OnLightDataReceived += UpdateLightUI;}
            if(nitrogenText){nitrogenText.text = null; socketClient.OnNitrogenDataReceived += UpdateNitrogenUI;}
            if(phosphorousText){phosphorousText.text = null; socketClient.OnPhosphorousDataReceived += UpdatePhosphorousUI;}
            if(potassiumText){potassiumText.text = null; socketClient.OnPotassiumDataReceived += UpdatePotassiumUI;}
            if(lightStateText){lightStateText.text = "Lights: " + "Unknown"; socketClient.LightStateDataReceived += UpdateLightStateUI;}
            if(valveStateText){valveStateText.text = "Valve: " + "Unknown"; socketClient.ValveStateReceived += UpdateValveStateUI;}
            if(lightStateButton){lightStateButton.onClick.AddListener(OnLightButtonClick);}
            if(valveStateButton){valveStateButton.onClick.AddListener(OnValveButtonClick);}

            
            if (socketClient.isConnected){
                socketClient.SendMessageToEsp32(messageText);
                socketClient.SendMessageToEsp32("NPK esp 0");
            } else {
                Debug.Log("Not Connected");
            }
            InvokeRepeating(nameof(SendMessagePeriodically), 1f, 1.1f);
            InvokeRepeating(nameof(GetlightPeriodically), 2.0f, 2.2f);
            InvokeRepeating(nameof(GetValvePeriodically), 2.5f, 2.3f);
        }
        void SendMessagePeriodically() {
            if (socketClient.isConnected) {
                socketClient.SendMessageToEsp32(messageText);  // Sends the same message each time
                socketClient.SendMessageToEsp32("NPK esp 0");
            } else {
                Debug.Log("Not Connected");
            }
        }
        void GetlightPeriodically() {
            if (socketClient.isConnected) {
                socketClient.SendMessageToEsp32("Valve state 0");
            } else {
                Debug.Log("Not Connected");
            }
        }
        void GetValvePeriodically() {
            if (socketClient.isConnected) {
                socketClient.SendMessageToEsp32("Lights state 0");
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
        public void OnLightButtonClick(){
            lightState = !lightState;
            socketClient.SendMessageToEsp32(lightState ? "Lights ON 1" : "Lights OFF 0");
            UpdateLightStateUI(lightState, 0);
            
        }

        public void OnValveButtonClick(){
            valveState = !valveState;
            socketClient.SendMessageToEsp32(valveState ? "Valve ON 1" : "Valve OFF 0");
            UpdateValveStateUI(valveState);
        }

        // Event handlers
        private void UpdateTemperatureUI(float temperature, long timer) {
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
        
        private void UpdateLightStateUI(bool lights, long lightOnTime) {
            if(lights){lightStateText.text = "Lights: " + "ON";lightState = true;}
            else{lightStateText.text = "Lights: " + "OFF";lightState = false;}
        }

        private void UpdateValveStateUI(bool valve) {
            if(valve){valveStateText.text = "Valve: " + "ON";valveState = true;}
            else{valveStateText.text = "Valve: " + "OFF";valveState = false;}
        }
        

        void OnDestroy() {
            // Unsubscribe from events to avoid memory leaks
            socketClient.OnLightDataReceived -= UpdateLightUI;
            socketClient.OnHumidityDataReceived -= UpdateHumidityUI;
            socketClient.OnFullTemperatureDataReceived -= UpdateTemperatureUI;
        }
    }
}