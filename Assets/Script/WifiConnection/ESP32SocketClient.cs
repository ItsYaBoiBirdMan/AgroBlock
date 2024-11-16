using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Script.WifiConnection {
    public class Esp32SocketClient : MonoBehaviour{
        private TcpClient client;
        private NetworkStream stream;
        private string ipAddress = "";       // IP will be set by UDP discovery
        private int tcpPort;             // Port will be set by UDP discovery
        internal bool isConnected;
        private UdpClient udpClient;
        private readonly int udpPort = 54131;         // UDP port for IP discovery
        private string ssid = "Default";
        

        // Specific events for different types of messages
        public event Action<float> OnTemperatureDataReceived;
        public event Action<float> OnHumidityDataReceived;
        public event Action<float> OnLightDataReceived;
        public event Action<float> OnNitrogenDataReceived;
        public event Action<float> OnPhosphorousDataReceived;
        public event Action<float> OnPotassiumDataReceived;

        void Start() {
            StartUDPDiscovery();
        }

        internal void StartUDPDiscovery() {
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, udpPort));
            udpClient.Client.ReceiveBufferSize = 8192;
            udpClient.Client.SendBufferSize = 8192;
            udpClient.BeginReceive(OnReceiveUDP, udpClient);
            Debug.Log("UDP discovery started, waiting for ESP32 broadcast...");
        }

        void OnReceiveUDP(IAsyncResult ar) {
            UdpClient u = (UdpClient)ar.AsyncState;
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, udpPort);
            byte[] receivedBytes = u.EndReceive(ar, ref endPoint);
            Debug.Log("I reached here");
            string receivedMessage = Encoding.ASCII.GetString(receivedBytes);

            // Parse the JSON message
            var jsonObj = JObject.Parse(receivedMessage);

            ipAddress = jsonObj["ip"]?.ToString();
            tcpPort = int.Parse(jsonObj["port"]?.ToString() ?? string.Empty);
            ssid = jsonObj["ssid"]?.ToString();
        

            Debug.Log($"Discovered IP: {ipAddress}, Port: {tcpPort}, SSID: {ssid}");

            ConnectToESP32();
        }

        void ConnectToESP32() {
            try {
                client = new TcpClient();
                client.BeginConnect(ipAddress, tcpPort, OnConnect, client);
            }
            catch (Exception e)
            {
                Debug.LogError("Connection attempt failed: " + e.Message);
                isConnected = false;
            }
        }

        private void OnConnect(IAsyncResult ar) {
            try {
                client.EndConnect(ar);
                if (client.Connected){
                    isConnected = true;
                    stream = client.GetStream();
                    Debug.Log("Connected to ESP32 server!");
                }else {
                    Debug.LogWarning("Failed to connect to ESP32.");
                    isConnected = false;
                }
            }catch (Exception e) {
                Debug.LogError("Connection error: " + e.Message);
                isConnected = false;
            }
        }

        public JObject SendMessageToEsp32(string message) {
            JObject jsonObj = new JObject();
            if (isConnected && client.Connected) {
                // Send message to ESP32
                byte[] messageBytes = Encoding.ASCII.GetBytes(message + "\n");
                stream.Write(messageBytes, 0, messageBytes.Length);
                Debug.Log("Sent to ESP32: " + message);
                // Now read the response
                try {
                    if (stream.DataAvailable) {
                        byte[] receivedBuffer = new byte[client.ReceiveBufferSize];
                        int bytesRead = stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                        string messageReceived = Encoding.ASCII.GetString(receivedBuffer, 0, bytesRead);
                        Debug.Log("Received from ESP32: " + messageReceived);
                        // Handle the JSON response based on its structure
                        jsonObj = JObject.Parse(messageReceived);
                        if (jsonObj.ContainsKey("message_type")) {
                            string messageType = jsonObj["message_type"]?.ToString();
                            if (messageType == "humidity" && jsonObj.ContainsKey("humidity")){
                                float humidity = (float)jsonObj["humidity"];
                                OnHumidityDataReceived?.Invoke(humidity);
                            } else if (messageType == "values" && jsonObj.ContainsKey("humidity")) {
                                if (jsonObj.ContainsKey("temperature")) {
                                    float temperature = (float)jsonObj["temperature"];
                                    OnTemperatureDataReceived?.Invoke(temperature);
                                }
                                if (jsonObj.ContainsKey("humidity")) {
                                    float humidity = (float)jsonObj["humidity"];
                                    OnHumidityDataReceived?.Invoke(humidity);
                                }
                                if (jsonObj.ContainsKey("light")) {
                                    // ReSharper disable once InconsistentNaming
                                    float Light = (float)jsonObj["light"];
                                    OnLightDataReceived?.Invoke(Light);
                                }
                            } else if (messageType == "npk" && jsonObj.ContainsKey("nitrogen")) {
                                if (jsonObj.ContainsKey("nitrogen")) {
                                    float nitrogen = (float)jsonObj["nitrogen"];
                                    OnNitrogenDataReceived?.Invoke(nitrogen);
                                }
                                if (jsonObj.ContainsKey("phosphorous")) {
                                    float phosphorous = (float)jsonObj["phosphorous"];
                                    OnPhosphorousDataReceived?.Invoke(phosphorous);
                                }
                                if (jsonObj.ContainsKey("potassium")) {
                                    // ReSharper disable once InconsistentNaming
                                    float potassium = (float)jsonObj["potassium"];
                                    OnPotassiumDataReceived?.Invoke(potassium);
                                }
                            }
                        }

                        return jsonObj;
                    }
                } catch (Exception e) {
                    Debug.LogError("Error receiving or parsing response: " + e.Message);
                }
            } else {
                StartUDPDiscovery();
                return SendMessageToEsp32(message);
            }
            return jsonObj;
        }
        
        

        private void OnApplicationQuit() {
            if (stream != null) stream.Close();
            if (client != null) client.Close();
            if (udpClient != null) udpClient.Close();
        }

        private void OnDisable() {
            if (stream != null) stream.Close();
            if (client != null) client.Close();
            if (udpClient != null) udpClient.Close();
        }
    
        void CheckCurrentNetwork(string expectedSsid) {
            var wifiInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && nic.OperationalStatus == OperationalStatus.Up);
            bool networkFound = false;
            foreach (var wifiInterface in wifiInterfaces) {
                var currentSsid = wifiInterface.Description; // This gets the SSID

                if (currentSsid.Equals(expectedSsid, StringComparison.OrdinalIgnoreCase)) {
                    networkFound = true; // Set flag to true
                    break; // Exit the loop after finding the network
                } 
            }
            if (!networkFound) {
                Debug.Log("The target network was not found.");
            }
        }
    }
}
