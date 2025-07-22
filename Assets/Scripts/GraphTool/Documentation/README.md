## Subscribing Your Float Values to the Graph
To plot any runtime float in the **Graph Tool** window (Window → Graph Tool), follow these simple steps:

1. Import the runtime API
In your game scripts, add:
```csharp
using GraphTool.Runtime;
```
2. Subscribe your float provider
Call GraphSubscription.Subscribe once (e.g. in Start() or Awake()):
```csharp
void Start()
{
    // “MyValue” is the series ID shown in the legend
    // () => MyComponent.SomeValue must return 0–100 (it will be clamped)
    // Color.yellow is the curve color
    GraphSubscription.Subscribe(
        "MyValue",
        () => myComponent.SomeValue,
        Color.yellow
    );
}
```
3. Unsubscribe when done
To clean up (for example, on object destruction), call:
```csharp
void OnDestroy()
{
    GraphSubscription.Unsubscribe("MyValue");
}
```