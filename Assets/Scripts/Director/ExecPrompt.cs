using Director;
using UnityEngine;
using System.Collections.Generic;
using Agent;

public class ExecPrompt : MonoBehaviour
{
    public ScopeController scopeController;
    public AgentController agent;
    public Player2Npc player2Npc;

    private string allExposedMethods;
    private string allExposedGameObjects;
    private List<CorrelatorResult> correlatorResults = new List<CorrelatorResult>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allExposedGameObjects = agent.InterpretExposedGameObjects();
        allExposedMethods = agent.InterpretExposedMethods();
        PrePrompt();
    }

    [ContextMenu("Get Scope")]
    public void GetScope()
    {
        correlatorResults = scopeController.EvaluateCorrelators();
    }

    [ContextMenu("Test Message")]
    public void TestMessage()
    {
        string message = "This is a test message to the NPC.";
        player2Npc.OnChatMessageSubmitted(message);
    }

    public void OnResponseReceived(NpcApiChatResponse response)
    {
        Debug.Log("Response received: " + response.message);
    }

    private void PrePrompt()
    {
        string prePrompt = agent.prePrompt + "\n" + agent.systemDescription + "\n\nExposedMethods" + allExposedMethods + "\n\nExposedGameObjects" + allExposedGameObjects;
        player2Npc.SpawnNpcAsync(prePrompt);
    }

    [ContextMenu("Execute Prompt")]
    public void ExecutePrompt()
    {
        string prompt = "CorrelatorResults: " + JsonUtility.ToJson(correlatorResults);
        player2Npc.OnChatMessageSubmitted(prompt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
