using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Examples.Factory101.Scripts
{
    [Serializable]
    public class LogPayload
    {
        public string playTime;
        public string message;
    }

    public class StatApi : MonoBehaviour
    {
        public static StatApi Instance { get; private set; }

        private const string ApiUrl   = "https://api.joeyvanderkaaij.com:8005/api/mcgibble/log";
        private const string ApiToken = "a4f87a62d60f4eae44b1456b4c2166a4";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Duplicate StatApi detected. Destroying new instance.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartLogging(LogPayload payload)
        {
            StartCoroutine(SendLogRequest(payload));
        }
        
        private IEnumerator SendLogRequest(LogPayload payload)
        {
            
            string jsonPayload = JsonUtility.ToJson(payload);
            Debug.Log($"Payload JSON ▶ {jsonPayload}");

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

            // Build the request
            var request = new UnityWebRequest(ApiUrl, "POST");
            var upload  = new UploadHandlerRaw(bodyRaw) {
                contentType = "application/json"
            };
            request.uploadHandler   = upload;
            request.downloadHandler = new DownloadHandlerBuffer();

            // Set auth header
            request.SetRequestHeader("Authorization", $"Bearer {ApiToken}");

            // Fire it off
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"API response: {request.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"API error ({request.responseCode}): {request.error}");
            }
        }
    }
}
