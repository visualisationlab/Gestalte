Goal
---
#### Keep context window small

##### Prioritize current events
- **Prioritize Information:** create importance hierarchies for reacting to more prominent events
- **Event Buffers**: only pass N suggestions
##### Memory
- **Short‑Term Buffers**: only pass a summary of older events
- **Vector‑Memory Retrieval**: embed critical past decisions and pull in the top‑k relevant ones


Design
---
### Sensors
Combinations of data points

### Correlators
Sensor values into a relevance function (linear/non-linear). Also a semantic description.

### Nudges
Suggested actions and **relevant relational data**

### Example
Sensor: A *house* is on *fire* with 5 *persons*
Rules: *persons* shouldn't be near *fire*
Nudges: Douse the fire (fire-truck, bucket, weather)

**Ideal world**
The LLM would know the house is on fire with 5 people inside and people not dying is of the highest priority. It knows about the fire truck nearby and it's capabilities to douse the fire.

**Problem of scale**
There are exponentially more sensors all wanting something. How to prioritize the house fire? How to collect relevant systems for solutions to meet the needs? It's a sliding scale; define too much and emergent behaviour disappears, define too little and the LLM floods with information. Where do we sit? How do we provide tools for the designers to decide themselves where to sit?


Architecture
---
After finding sensor results that have the highest priority I want to be able to construct semantically relevant information in the Translator. This means the Translator needs access to the original sensor type, even if it's a recursive sensor I want to be able to point to the relevant game objects and the base sensor type.


Sensors
---
The most important part is keeping track of the source of data. This is done through the tracking of `SensorResult`. This is necessary for the **Translator** to construct a semantic narrative. On any failure the `SensorResult` will contain a `float.NaN` value and should expose a `LogError` on what failed.

**Sensors** can evaluate multiple float inputs and always output a float value that represents the reading. Writing sensors means inheriting the **Sensor** abstract class, implementing the **Evaluate** function. For any arithmetic try and use the `EvaluationFunction` Scriptable Objects. If an evaluation fails it should return ``float.NaN``

Every Sensor somehow needs to pass along a **Min** and **Max** value that can be used in the **Correlator** to normalize the value.

**Sensor Operators**

**Some example Sensor Operators:**
**SubtractFunction**: Subtracts all inputs sequentially.
**DivideFunction**: Divides all inputs sequentially.
**MaxFunction**: Returns the maximum value from the inputs.
**MinFunction**: Returns the minimum value from the inputs.
**AverageFunction**: Calculates the average of all inputs.
**WeightedSumFunction**: Computes a weighted sum of inputs, requiring an additional weight array.
**ClampFunction**: Clamps the result of an evaluation between a minimum and maximum value.
**ExponentialFunction**: Applies an exponential operation to the inputs.
**CustomCurveFunction**: Evaluates inputs using a predefined AnimationCurve for non-linear transformations.
**ThresholdFunction**: Returns 1 if the input exceeds a threshold, otherwise 0.


Correlators
---
Sensor Data readings are normalized by the Min and Max value in the **SensorResult
**Correlators** are **Agent** specific; you can assign different **Correlators** to different Agents. 

