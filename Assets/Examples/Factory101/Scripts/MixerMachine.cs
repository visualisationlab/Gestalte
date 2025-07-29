using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;

public class MixerMachine : Machine
{
    public SimpleSensor sensorOne;
    public SimpleSensor sensorTwo;
    public GameObject mcGibbleTemplate;
    public Transform outputPort;
    private Script luaScript;
    private string script;    
    private Vector3 tinyRandom;
    
    private void Start()
    {
        UserData.RegisterType<MixerMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
        StartCoroutine(ExecuteEverySecond());
    }

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            if(!string.IsNullOrWhiteSpace(script)){
                luaScript.DoString(script);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    
    [ExposeMethod("Mix the items together")]
    public void Mix()
    {
        if (sensorOne.onDetect && sensorTwo.onDetect)
        {
            var mcGibbleOne = sensorOne.detectedGameObject.GetComponent<McGibble>();
            var mcGibbleTwo = sensorTwo.detectedGameObject.GetComponent<McGibble>();
            RecipeTracker.Instance.GetRecipe(mcGibbleOne, mcGibbleTwo, Eject);
            
            McGibbleTracker.Instance.Remove(mcGibbleOne);
            McGibbleTracker.Instance.Remove(mcGibbleTwo);
        }
    }

    private void Eject(RecipeTracker.Recipe recipe)
    {
        tinyRandom = new Vector3(Random.value-0.5f, Random.value, 0f);
        var mcGibble = Instantiate(mcGibbleTemplate, outputPort.transform.position + tinyRandom, Quaternion.identity).GetComponent<McGibble>();
        McGibbleTracker.Instance.Add(mcGibble);
        mcGibble.description = recipe.result;
        Debug.Log($"EJECT: {recipe.result.gibbleType}");
    }
    
    public override void SetScript(string code)
    {
        script = code;
    }
}
