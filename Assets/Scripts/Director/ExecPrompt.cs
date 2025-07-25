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
    }

    [ContextMenu("Get Scope")]
    public void GetScope()
    {
        correlatorResults = scopeController.EvaluateCorrelators();
    }

    [ContextMenu("Execute Prompt")]
    public void ExecutePrompt()
    {
        string prompt = agent.prePrompt + "\n\nExposedMethods" + allExposedMethods + "\n\nExposedGameObjects" + allExposedGameObjects + "\n\nCorrelatorResults" + JsonUtility.ToJson(correlatorResults);
        player2Npc.OnChatMessageSubmitted(prompt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
