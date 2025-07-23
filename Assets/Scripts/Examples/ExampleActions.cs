using Mediator;
using UnityEngine;

public class ExampleActions : MonoBehaviour
{
    [ExposeFunction("This makes the animal fly to a room")]
    public void Fly(string roomName, int height, int speed)
    {
        Debug.Log("Example Action 1 executed");
    }
    
    [ExposeFunction("This makes the animal dig to a room")]
    public void Dig(string roomName, int speed, int depth)
    {
        Debug.Log("Example Action 2 executed");
    }
    
    [ExposeFunction("This makes the animal walk to a room")]
    public void Walk(string roomName, int speed)
    {
        Debug.Log("Example Action 2 executed");
    }
}
