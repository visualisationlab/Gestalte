The Director systems function is to reduce the amount of data flowing into an **Agent** and keep the context window small. It does this by combining **Sensors** readouts, sorting them on relevance and correlate this data with a meaningful context. For instance a system tracking aggression levels, hunger and proximity to a prey animal might be combined under the **Correlator** with the context "lion is hungry, aggressive and close to a prey". The overall system is designed to give the game developer enough room to control the relative importance of the Sensors to each other for a correlation to be relevant  but also to give enough space to the **Agent** to come up with it's own solutions on how to deal with the events given the possibilities within the system. The idea is to give the designer the right tools to facilitate for emergent behaviour.
### Sensors
A Sensor tracks data points and expected minimum and maximum values. The values are passed along to a single or multiple **Correlators**. For instance, a Sensor can be measuring the distance between a **GameObject** and itself.
### Correlators
Sensor values flow into a relevance function (linear/non-linear). This normalizes the Sensor values, weights them according to the function and averages all the values together to get a normalized relevance score. The Correlators also carry a description describing the meaning of this correlator. All Correlators and their relevance score are later sorted and the first n values are passed along to the Agent to determine what Correlators are important enough to be taken into account in the next decision making step.

Sensor Data readings are normalized by the Min and Max value in the ``SensorResult``. Correlators are Agent specific; you can assign different Correlators to different Agents. The Correlator is used to tie different Sensors together in a contextually meaningful manner and their relevance is dynamically calculated by the Sensor output and the functions assigned to the Sensors. This means that different Sensors can have different effects on the actual relevance. Some might increase the relevance rapidly, or some Sensors might actually reduce the relevance on a high output.


### Contextual Representation
---
One of the important parts is knowing what the Sensor and Correlator systems represent. This is done through the tracking of `SensorResult`. This is necessary for the LLM to construct a narrative and take action accordingly. On any failure the `SensorResult` will contain a `float.NaN` value and should expose a `LogError` on what failed.

**Sensors** can evaluate one or multiple float inputs and always output a float value. Designing a sensor means inheriting the ``Sensor`` abstract class and implementing the ``Evaluate`` function.

Every **Sensor** needs to pass along a **Min** and **Max** value that can be used in the **Correlator** to normalize the value. It's up to the developer to decide on the reasonable values.


### Example
---
Sensors: The temperature inside a house is very high with 5 persons inside
Relevance: 0.9

Sensors: The temperature inside a house is very high with 0 persons inside
Relevance: 0.6

The Agent can now determine how to prioritize the dousing of the fires.
