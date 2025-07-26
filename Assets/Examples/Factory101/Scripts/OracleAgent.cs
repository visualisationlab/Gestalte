using System;
using System.Collections.Generic;
using Examples.Factory101.Scripts;
using UnityEditor;
using UnityEngine;

public class OracleAgent : MonoBehaviour
{
    [TextArea(3, 50)] public string preprompt;
    public Player2Npc player2Npc;
    private Queue<OracleResponse> responseQueue = new();
    private void Start()
    {
        var composedPreprompt = preprompt;
        _ = player2Npc.SpawnNpcAsync(composedPreprompt);
    }

    [ContextMenu("Send Message")]
    public void MockMessage()
    {
        SendMessage("Hello How are You?", s => Debug.Log("Nothin"));
    }
    public void SendMessage(string message, Action<string> callback)
    {
        Debug.Log($"Sending message to Oracle: {message}");
        responseQueue.Enqueue(new OracleResponse { guid = "test", callback = callback });
        player2Npc.OnChatMessageSubmitted(message);
    }
    
    public void OnResponseReceived(NpcApiChatResponse response)
    {
        Debug.Log($"Oracle Response Received: {response}");
        var item = responseQueue.Dequeue();
        item.callback(response.message);
    }
}
