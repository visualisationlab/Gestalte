using System;
using System.Collections.Generic;
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

    public void SendMessage(string message, Action<string> callback)
    {
        responseQueue.Enqueue(new OracleResponse { callback = callback });
        player2Npc.OnChatMessageSubmitted(message);
    }

    public void OnResponseReceived(NpcApiChatResponse response)
    {
        var item = responseQueue.Dequeue();
        item.callback(response.message);
    }
}
