using Mediator;
using UnityEngine;

public class ExampleActions : MonoBehaviour
{
    [ExposeFunction("This is an Example Function")]
    public void ExampleAction1(string extra)
    {
        Debug.Log("Example Action 1 executed");
    }
    
    [ExposeFunction("This is the Example TWO Function")]
    public void ExampleAction2(int count, string name)
    {
        Debug.Log("Example Action 2 executed");
    }
}
