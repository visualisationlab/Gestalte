using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class OracleAgent : MonoBehaviour
{
    [TextArea(3, 50)] public string preprompt;

    public Player2Npc player2Npc;

    // max number of times to retry before erroring out
    [SerializeField] private int _maxRetries = 3;

    // how long to wait between each try (in seconds)
    [SerializeField] private float _timeoutSeconds = 5f;
    [SerializeField] private DirectAPI directAPI;

    private Queue<OracleRequest> _pending = new();
    private float _timeOutDeadline;

    private void Start()
    {
        var composedPreprompt = preprompt;
        _ = player2Npc.SpawnNpcAsync(composedPreprompt);
    }
    
    public async Task<string> SendMessageDirect(string systemMessage, string message)
    {
        string response = await directAPI.SendMessageAsync(systemMessage, message);
        return response;
    }

    public void SendMessage(string message, Action<string> callback)
    {
        var req = new OracleRequest
        {
            message = message,
            callback = callback,
            retryCount = 0
        };
        _pending.Enqueue(req);

        // if this was the only request, immediately kick it off:
        if (_pending.Count == 1)
            SendCurrent();
    }

    private void SendCurrent()
    {
        if (_pending.Count == 0) return;

        var current = _pending.Peek();
        current.retryCount++;
        player2Npc.OnChatMessageSubmitted(current.message);

        // reset timeout deadline
        _timeOutDeadline = Time.time + _timeoutSeconds;
        Debug.Log($"[OracleAgent] Sending \"{current.message}\" (try #{current.retryCount})");
    }

    private void Update()
    {
        if (_pending.Count == 0)
            return;

        // timed out before OnResponseReceived fired?
        if (Time.time > _timeOutDeadline)
        {
            var current = _pending.Peek();

            if (current.retryCount < _maxRetries)
            {
                Debug.LogWarning($"[OracleAgent] Timeout—retrying ({current.retryCount + 1}/{_maxRetries})");
                SendCurrent();
            }
            else
            {
                Debug.LogError($"[OracleAgent] Giving up after {current.retryCount} attempts");
                _pending.Dequeue();
                current.callback?.Invoke(null); // or pass an error string
                // next message in queue (if any):
                if (_pending.Count > 0) SendCurrent();
            }
        }
    }

    public void OnResponseReceived(NpcApiChatResponse response)
    {
        if (_pending.Count == 0)
        {
            Debug.LogWarning("[OracleAgent] Got a stray response with no pending request.");
            return;
        }

        var completed = _pending.Dequeue();
        Debug.Log($"[OracleAgent] Got reply: “{response.message}” -- invoking callback");
        completed.callback?.Invoke(response.message);

        // kick off the next waiting message, if any
        if (_pending.Count > 0)
            SendCurrent();
    }

    // simple holder for each chatRequest
    private class OracleRequest
    {
        public string message;
        public Action<string> callback;
        public int retryCount;
    }
}