using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class DirectAPIChatRequest
{
    public List<DirectAPIMessage> messages;
    public bool stream = false;
}

[Serializable]
public class DirectAPIMessage
{
    public string content;
    public string role;
    public DirectAPIMessage(string content, string role)
    {
        this.content = content;
        this.role = role;
    }
}

public class DirectAPI : MonoBehaviour
{
    [SerializeField] private string apiUrl = "http://localhost:4315/v1/chat/completions";

    [ContextMenu("Test Request")]
    async void TestRequest()
    {
        string response = await SendMessageAsync(
            "You are an entertaining assistant who tries to make jokes.",
            "Hello, how are you doing today?"
        );

        Debug.Log("Response: " + response);
    }

    public async Task<string> SendMessageAsync(string systemMessage, string userMessage)
    {
        var payload = new DirectAPIChatRequest
        {
            messages = new List<DirectAPIMessage>
            {
                new(systemMessage, "system"),
                new(userMessage,   "user")
            },
        };

        string jsonPayload = JsonConvert.SerializeObject(payload);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

            var asyncOp = request.SendWebRequest();
            while (!asyncOp.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Network error: {request.error} (Code: {request.responseCode})");
            }

            string json = request.downloadHandler.text;

            APIResponse parsed;
            try
            {
                parsed = JsonConvert.DeserializeObject<APIResponse>(json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse API response JSON: {ex.Message} | Raw: {json}");
            }

            string assistantReply = parsed?.choices?[0]?.message?.content;
            if (string.IsNullOrEmpty(assistantReply))
                throw new Exception("Assistant reply was missing or empty.");

            Debug.Log("✅ Assistant says: " + assistantReply);
            return assistantReply;
        }
    }

    private string EscapeJson(string input)
    {
        return input.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}