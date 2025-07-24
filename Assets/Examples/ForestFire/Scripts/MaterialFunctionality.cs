using UnityEngine;
using Mediator;

/// <summary>
/// MaterialFunctionality: A reusable component that references a TemperatureSensor
/// and provides a method to ignite the material by setting its on-fire state
/// and bumping its temperature above a given threshold.
/// </summary>
public class MaterialFunctionality : MonoBehaviour
{

    public string materialDescription = "";
    private bool isOnFire = false;
    // Change material to Assets/Examples/ForestFire/Mats/Red.mat when set ablaze
    public Material fireEffectMat;
    private MeshRenderer materialRenderer = null;
    public TemperatureValue intrinsicTemperature;
    public TemperatureValue highestNeighboringTemperature;

    void Awake()
    {
        // Get material renderer from this GameObject
        materialRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        UpdateEnvironmentalTemperature();
    }

    /// <summary>
    /// Ignites this material: sets on-fire state, bumps temperature,
    /// and spawns a fire VFX if provided.
    /// </summary>
    [ExposeMethod("This sets the material ablaze")]
    public void SetAblaze()
    {
        if (isOnFire)
            return;

        Debug.Log($"Setting material '{materialDescription}' ablaze", this);
        isOnFire = true;

        // Set material to fire effect
        if (fireEffectMat != null)
        {
            if (materialRenderer != null)
            {
                materialRenderer.material = fireEffectMat;
            }
            else
            {
                Debug.LogWarning("No Renderer found on this GameObject to apply fire effect material.", this);
            }
        }
    }

    // Function to change the intrinsic temperature of the material
    [ExposeMethod("This changes the intrinsic temperature of the material based on the amount provided, can be negative or positive")]
    public void AdjustIntrinsicTemperature(float amount)
    {
        intrinsicTemperature.temperature += amount;
    }

    // Function that uses the sphere collider of the game object to check if other objects have a sensorprobe, if so get average the environmentalTemperature to the average of all intrinsic temperatures of the objects in the sphere collider
    public void UpdateEnvironmentalTemperature()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            Debug.LogWarning("No SphereCollider attached to this GameObject.", this);
            return;
        }
        Collider[] colliders = Physics.OverlapSphere(sphereCollider.transform.position, sphereCollider.radius * Mathf.Max(sphereCollider.transform.lossyScale.x, sphereCollider.transform.lossyScale.y, sphereCollider.transform.lossyScale.z));

        float highestTemp = 0.0f;
        foreach (Collider collider in colliders)
        {
            SensorProbe sensorProbe = collider.GetComponent<SensorProbe>();
            if (sensorProbe != null && sensorProbe != this) // Avoid self
            {
                float temperature = sensorProbe.Evaluate();
                if (temperature > highestTemp) highestNeighboringTemperature.temperature = temperature;
            }
        }
    }

    /// <summary>
    /// Query if this material is currently on fire.
    /// </summary>
    public bool IsOnFire()
    {
        return isOnFire;
    }
}
