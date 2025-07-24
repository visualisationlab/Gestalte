using Mediator;
using UnityEngine;

public class ExampleActions : MonoBehaviour
{
    [ExposeMethod("This makes the animal fly to a room")]
    public void Fly(string roomName, int height, int speed)
    {
        Debug.Log("Example Action 1 executed");
    }
    
    [ExposeMethod("This makes the animal dig to a room")]
    public void Dig(string roomName, int speed, int depth)
    {
        Debug.Log($"Mole Dug into {roomName} with a speed of {speed} from a depth of {depth}");
    }
    
    [ExposeMethod("This makes the animal walk to a room")]
    public void Walk(string roomName, int speed, string gameObjectGUID)
    {
        Debug.Log("Example Action 2 executed");
    }
}
