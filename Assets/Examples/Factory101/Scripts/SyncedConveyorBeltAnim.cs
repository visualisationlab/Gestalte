using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SyncedConveyorBeltAnim : MonoBehaviour
{
    [Tooltip("Name of the state that contains the looping belt animation")]
    public string stateName = "Conveyorv2Anim"; // adjust to your actual state
    [Tooltip("Duration in seconds of one full loop of the animation clip")]
    public float cycleDuration = 0.5f; // set to match the length of the looped animation

    private Animator animator;
    private int stateHash;

    void Awake()
    {
        animator = GetComponent<Animator>();
        stateHash = Animator.StringToHash(stateName);
        // Freeze automatic progression; we manually drive normalized time.
        animator.speed = 0f;
    }

    void Update()
    {
        // Compute how far into the cycle we are, globally.
        float elapsed = (Time.time - ConveyorSync.GlobalStartTime) * ConveyorSync.GlobalSpeed;
        float normalizedTime = (elapsed / cycleDuration) % 1f;

        // Force the animator to that frame of the looping state.
        animator.Play(stateHash, 0, normalizedTime);
        // Note: Keep speed=0 so it doesn't advance on its own.
    }
}
