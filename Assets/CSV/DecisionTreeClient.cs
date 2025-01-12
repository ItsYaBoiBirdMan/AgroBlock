using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Script.StateMachine;

public class DecisionTreeClient : MonoBehaviour
{
    // URL of your Flask API endpoint
    private string apiUrl = "http://127.0.0.1:5000/predictDecisionTree";
    private string apiUrl2 = "http://127.0.0.1:5000/predictFertilezerlessTree";
    // Example features to send to the API
    private float[] features = { 72.0f, 19.0f, 8.0f, 120.0f, 165.0f, 300.0f }; // Replace with actual input features
    private float[] features2 = { 72.0f, 19.0f, 8.0f };
    [SerializeField] private ConditionManager manager;
    public void UpdateFeatures(){
        float humidity = manager.Humidity;
        float temperature = manager.Temperature;
        float nitrogen = manager.Nitrogen;
        float phosphorous = manager.Phosphorous;
        float potassium = manager.Potassium;
        features = new { humidity , temperature , 8.0f, nitrogen, phosphorous, potassium};
        features2 = new { humidity, temperature, 8.0f};
    }
    // Start is called before the first frame update
    public void StartButton()
    {
        StartCoroutine(SendPredictionRequest(features));
    }
    public void StartButton2()
    {
        StartCoroutine(SendPredictionRequest2(features2));
    }

    // Coroutine to send a POST request
    IEnumerator SendPredictionRequest(float[] features) {
        // Convert the payload to JSON
        string jsonPayload = JsonUtility.ToJson(new Wrapper(features));

        // Create a UnityWebRequest for a POST request
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(apiUrl, ""))
        {
            // Set the request headers and body
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for a response
            yield return request.SendWebRequest();

            // Handle the response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
    
    IEnumerator SendPredictionRequest2(float[] features)
    {
        // Convert the payload to JSON
        string jsonPayload = JsonUtility.ToJson(new Wrapper(features));

        // Create a UnityWebRequest for a POST request
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(apiUrl2, ""))
        {
            // Set the request headers and body
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            Debug.Log(apiUrl2);
            // Send the request and wait for a response
            yield return request.SendWebRequest();

            // Handle the response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response 2: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }

    

    // Helper class for serializing the dictionary to JSON
    [System.Serializable]
    private class Wrapper
    {
        public float[] features;

        public Wrapper(float[] features)
        {
            this.features = features;
        }
    }

}
