using System.Collections;
using Examples.Factory101.Scripts;
using Examples.Factory101.Scripts.Input;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using Coroutine = UnityEngine.Coroutine;

public class MixerMachine : Machine, IBuyable, IBlockPlacement
{
    public SimpleSensor sensorOne;
    public SimpleSensor sensorTwo;
    public GameObject mcGibbleTemplate;
    public Transform outputPort;
    public bool mixing;
    private Vector3 tinyRandom;
    public UnityEvent onStartMixing;
    public UnityEvent onStopMixing;
    
    //Mix rate
    [SerializeField] private float mixRate;
    private float maxMixRate = 1f;

    private Coroutine loopRoutine;

    [SerializeField] private string extraComment = "";
    [SerializeField] private string incantation = "";

    protected override void RegisterLua()
    {
        UserData.RegisterType<MixerMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
        loopRoutine = StartCoroutine(ExecuteEverySecond());
    }

    public override string GetStatus()
    {
        string result = "";
        result += $"Level: {upgradeLevel}/{maxUpgradeLevel} \n";
        result += $"Rate: {GetDigRate()} item(s)/s (max {GetMaxDigRate()})\n";
        return result;
    }

    public override void UpgradeMachine()
    {
        upgradeLevel++;
        maxMixRate = upgradeLevel switch
        {
            2 => 3f,
            3 => 5f,
            4 => 6f,
            _ => 1f // default case
        };
        
        extraComment = upgradeLevel switch
        {
            2 => "give a slightly higher chance to a medium normalizedRarity",
            3 => "give a medium chance to a high normalizedRarity",
            4 => "give a very high chance to a high normalizedRarity",
            5 => "takes incantation instead",
            _ => ""
        };
        Debug.Log(extraComment);
    }

    protected override void AfterSetScript()
    {
        ExecuteScript();
        RestartCoroutine();
    }

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            Mix();
            yield return new WaitForSeconds(1f / mixRate);
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

            var extra = extraComment;
            if (upgradeLevel == 5) extra = incantation;
            
            RecipeTracker.Instance.GetRecipe(mcGibbleOne.description, mcGibbleTwo.description, Eject, extra);

            McGibbleTracker.Instance.Remove(mcGibbleOne);
            McGibbleTracker.Instance.Remove(mcGibbleTwo);
            Debug.Log("Mixing");
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
    }
    
    [ExposeMethod("Sets how many items this machine mixes up per second.")]
    public void SetMixRate(float rate)
    {
        mixRate = Mathf.Min(rate, maxMixRate);
    }
    
    [ExposeMethod("Add an incantation while mixing")]
    public void SetIncantation(string comment)
    {
        incantation = comment;
    }

    public int GetPrice()
    {
        return basePrice;
    }

    public string GetDescription()
    {
        return description;
    }
    
    private string GetDigRate()
    {
        return mixRate.ToString("0.0");
    }

    private string GetMaxDigRate()
    {
        return maxMixRate.ToString("0.0");
    }
    
    private void RestartCoroutine()
    {
        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
        }

        loopRoutine = StartCoroutine(ExecuteEverySecond());
    }
}
