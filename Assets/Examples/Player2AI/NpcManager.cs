using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class Parameters
{
    public string type;
    public Dictionary<string, object> properties;
    public List<string> required;
}

public class NpcManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] public string gameId = "your-game-id";
    
    private Player2NpcResponseListener _responseListener;
    
    private const string BaseUrl = "http://localhost:4315/v1";
    
    public static string GetBaseUrl() 
    {
        return BaseUrl;
    }

    private void Awake() 
    {
        // Add the component and configure it
        _responseListener = gameObject.AddComponent<Player2NpcResponseListener>();
        _responseListener.SetGameId(gameId);
        
        Debug.Log($"NpcManager initialized with gameId: {gameId}");
    }

    private void Start()
    {
        // Ensure listener starts after all components are initialized
        if (_responseListener != null && !_responseListener.IsListening)
        {
            Debug.Log("Starting response listener from NpcManager");
            _responseListener.StartListening();
        }
        
    }

    public void RegisterNpc(string id, UnityEvent<NpcApiChatResponse> onResponse)
    {
        if (_responseListener == null)
        {
            Debug.LogError("Response listener is null! Cannot register NPC.");
            return;
        }

        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("Cannot register NPC with empty ID");
            return;
        }
        
        _responseListener.RegisterNpc(id, onResponse);

        // Ensure listener is running after registering
        if (!_responseListener.IsListening)
        {
            Debug.Log("Listener was not running, starting it now");
            _responseListener.StartListening();
        }
    }

    public void UnregisterNpc(string id)
    {
        if (_responseListener != null)
        {
            _responseListener.UnregisterNpc(id);
        }
    }

    public bool IsListenerActive()
    {
        return _responseListener != null && _responseListener.IsListening;
    }

    public void StartListener()
    {
        if (_responseListener != null)
        {
            _responseListener.StartListening();
        }
    }

    public void StopListener()
    {
        if (_responseListener != null)
        {
            _responseListener.StopListening();
        }
    }

    private void OnDestroy()
    {
        if (_responseListener != null)
        {
            _responseListener.StopListening();
        }
    }

    // Add this method for debugging
    [ContextMenu("Debug Listener Status")]
    public void DebugListenerStatus()
    {
        if (_responseListener == null)
        {
            Debug.Log("Response listener is NULL");
        }
        else
        {
            Debug.Log($"Response listener status: IsListening={_responseListener.IsListening}, GameId={_responseListener.GameId}");
        }
    }
}