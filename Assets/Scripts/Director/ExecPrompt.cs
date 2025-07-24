using Director;
using UnityEngine;
using System.Collections.Generic;
using Agent;

public class ExecPrompt : MonoBehaviour
{
    public ScopeController scopeController;
    public AgentController agent;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
