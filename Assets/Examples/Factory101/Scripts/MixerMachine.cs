using System;
using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;

public class MixerMachine : Machine
{
    public SimpleSensor sensorOne;
    public SimpleSensor sensorTwo;
    public GameObject mcGibbleTemplate;
    public Transform outputPort;
    public bool mixing;
    [SerializeField] private int tickRate;
    private Vector3 tinyRandom;
    public UnityEvent onStartMixing;
    public UnityEvent onStopMixing;
    protected override void RegisterLua()
    {
        UserData.RegisterType<MixerMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
        StartCoroutine(ExecuteEverySecond());
    }

    public override string GetStatus()
    {
        return "Mixer Status";
    }

    public override string UpgradeMachine()
    {
        throw new NotImplementedException();
    }

    protected override void AfterSetScript()
    {
        ExecuteScript();
    }

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            Mix();
            yield return new WaitForSeconds(tickRate);
        }
    }
    
    private void Mix()
    {
        if (sensorOne.onDetect && sensorTwo.onDetect && !mixing)
        {
            mixing = true;
            onStartMixing?.Invoke();

            var mcGibbleOne = sensorOne.detectedGameObject.GetComponent<McGibble>();
            var mcGibbleTwo = sensorTwo.detectedGameObject.GetComponent<McGibble>();
            RecipeTracker.Instance.GetRecipe(mcGibbleOne.description, mcGibbleTwo.description, Eject);

            McGibbleTracker.Instance.Remove(mcGibbleOne);
            McGibbleTracker.Instance.Remove(mcGibbleTwo);
        }
    }

    private void Eject(Recipe recipe)
    {
        mixing = false;
        onStopMixing?.Invoke();

        tinyRandom = new Vector3(Random.value-0.5f, 0f, 0f);
        var mcGibble = Instantiate(mcGibbleTemplate, outputPort.transform.position + tinyRandom, Quaternion.identity).GetComponent<McGibble>();
        McGibbleTracker.Instance.Add(mcGibble);
        mcGibble.description = recipe.result;
        Debug.Log($"EJECT: {recipe.result.singleEmoji}");
    }
    
    [ExposeMethod("Sets the speed this machine mixes items at")]
    public void SetSpeed(int rate)
    {
        tickRate = rate;
        if (tickRate == 0) tickRate = Int32.MaxValue;
    }

}
