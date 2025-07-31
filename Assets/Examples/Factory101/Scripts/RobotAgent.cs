using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Agent;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class RobotAgent : MonoBehaviour
{
    [TextArea(3, 50)] public string preprompt;
    [TextArea] public string gameObjectInstructions;
    public Player2Npc player2Npc;
    public UnityEvent onResponseReceived;

    [Header("Retry Settings")]
    [SerializeField] private int   _maxRetries     = 3;
    [SerializeField] private float _timeoutSeconds = 5f;

    // internal queue of pending requests
    private Queue<RobotRequest> _pending = new();
    private float               _timeoutDeadline;

    private void Start()
    {
        var composedPreprompt = preprompt + RobotAgentResponse.Format();
        _ = player2Npc.SpawnNpcAsync(composedPreprompt);
    }

    /// <summary>
    /// Enqueue a new code‑generation request for the given machine.
    /// </summary>
    [ContextMenu("Send Message")]
    public void SendMessage(ExposeMachine machine)
    {
        var instructions = BuildInstructions(machine);
        var req = new RobotRequest {
            machine      = machine,
            instructions = instructions,
            retryCount   = 0
        };
        _pending.Enqueue(req);

        // if this is the only pending request, fire it off immediately
        if (_pending.Count == 1)
            SendCurrent();
    }

    private void Update()
    {
        if (_pending.Count == 0) return;

        // timed out waiting for a response?
        if (Time.time > _timeoutDeadline)
        {
            var current = _pending.Peek();
            if (current.retryCount < _maxRetries)
            {
                Debug.LogWarning($"[RobotAgent] Timeout—resending (try {current.retryCount+1}/{_maxRetries})");
                SendCurrent();
            }
            else
            {
                Debug.LogError($"[RobotAgent] Giving up on machine “{current.machine.name}” after {current.retryCount} attempts");
                // still invoke event so listeners know it completed (with no script)
                onResponseReceived?.Invoke();
                _pending.Dequeue();
                if (_pending.Count > 0) SendCurrent();
            }
        }
    }

    public void OnResponseReceived(NpcApiChatResponse response)
    {
        if (_pending.Count == 0)
        {
            Debug.LogWarning("[RobotAgent] Stray response—no pending request.");
            return;
        }

        var completed = _pending.Dequeue();
        Debug.Log($"[RobotAgent] Response for “{completed.machine.name}”: {response.message}");

        // extract & apply
        string json = ExtractJson(response.message);
        if (!string.IsNullOrEmpty(json))
        {
            var resp = JsonConvert.DeserializeObject<RobotAgentResponse>(json);
            completed.machine.SetScript(resp.Lua);
        }
        else
        {
            Debug.LogError("[RobotAgent] Failed to parse JSON from response.");
        }

        // notify listeners
        onResponseReceived?.Invoke();

        // kick off next request
        if (_pending.Count > 0)
            SendCurrent();
    }

    private void SendCurrent()
    {
        var current = _pending.Peek();
        current.retryCount++;
        Debug.Log($"[RobotAgent] Sending to “{current.machine.name}” (try {current.retryCount}):\n{current.instructions}");
        player2Npc.OnChatMessageSubmitted(current.instructions);

        _timeoutDeadline = Time.time + _timeoutSeconds;
    }

    private string BuildInstructions(ExposeMachine machine)
    {
        var instr = gameObjectInstructions
                  + machine.description
                  + ". It contains the following functions you can reference: "
                  + machine.GetExposedMethodsNames()
                  + ". Write code that does the following: "
                  + machine.instructionPrompt;
        return instr;
    }

    private string ExtractJson(string input)
    {
        var match = Regex.Match(input, @"```json\s*(\{[\s\S]*?\})\s*```");
        return match.Success ? match.Groups[1].Value : null;
    }

    // holder for each outgoing request
    private class RobotRequest
    {
        public ExposeMachine machine;
        public string        instructions;
        public int           retryCount;
    }
}
