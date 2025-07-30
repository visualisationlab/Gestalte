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
    public bool mixing;
    
    private Vector3 tinyRandom;

    protected override void RegisterLua()
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
        if (sensorOne.onDetect && sensorTwo.onDetect && !mixing)
        {
            mixing = true;
            var mcGibbleOne = sensorOne.detectedGameObject.GetComponent<McGibble>();
            var mcGibbleTwo = sensorTwo.detectedGameObject.GetComponent<McGibble>();
            RecipeTracker.Instance.GetRecipe(mcGibbleOne.description, mcGibbleTwo.description, Eject);

            McGibbleTracker.Instance.Remove(mcGibbleOne);
            McGibbleTracker.Instance.Remove(mcGibbleTwo);
        }
    }

    private void Eject(RecipeTracker.Recipe recipe)
    {
        mixing = false;
        tinyRandom = new Vector3(Random.value-0.5f, 0f, 0f);
        var mcGibble = Instantiate(mcGibbleTemplate, outputPort.transform.position + tinyRandom, Quaternion.identity).GetComponent<McGibble>();
        McGibbleTracker.Instance.Add(mcGibble);
        mcGibble.description = recipe.result;
        Debug.Log($"EJECT: {recipe.result.icon}");
    }
}
