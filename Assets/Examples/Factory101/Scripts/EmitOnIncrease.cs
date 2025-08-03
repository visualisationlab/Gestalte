using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EmitOnIncrease : MonoBehaviour
{
    private ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // This method can be hooked into the UnityEvent<int>
    public void EmitParticles(int delta)
    {
        if (delta > 0)
            ps.Emit(delta);
    }

    [ContextMenu("Test Emit")]
    public void TestEmit()
    {
        ps = GetComponent<ParticleSystem>();
        EmitParticles(10); // Test emit 10 particles
    }
}
