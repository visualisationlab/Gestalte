using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DissolveSelfPlay : MonoBehaviour
{
    public float destroyAfter = 0.7f;
    void Start()
    {
        var ps = GetComponent<ParticleSystem>();
        ps.Play();

        Destroy(gameObject, destroyAfter);
    }
}
