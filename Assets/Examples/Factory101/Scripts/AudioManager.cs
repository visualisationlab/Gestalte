using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource sfxSource;        // for one-shots like chime/etc
    [SerializeField] private AudioSource foliageSource;    // looping foliage
    [SerializeField] private AudioSource musicSource;      // non-looping music

    [Header("AudioFX")]
    [SerializeField] private AudioClip chime;
    [SerializeField] private AudioClip lowClick;
    [SerializeField] private AudioClip pling;
    [SerializeField] private AudioClip pluck;
    [SerializeField] private AudioClip shake;
    [SerializeField] private AudioClip tok;

    [Header("MusicFX")]
    [SerializeField] private AudioClip foliage;
    [SerializeField] private AudioClip music;

    [Header("Settings")]
    [Tooltip("Min delay after music ends before replaying music (seconds)")]
    [SerializeField] private float minFoliageOnlyDuration = 300f; // 5 minutes
    [Tooltip("Max delay after music ends before replaying music (seconds)")]
    [SerializeField] private float maxFoliageOnlyDuration = 600f; // 10 minutes

    private Dictionary<AudioClip, float> _lastPlayed = new();
    [SerializeField, Tooltip("Minimum seconds between playing the same SFX clip to avoid stacking")] 
    private float minRepeatInterval = 0.1f; // adjust as needed
    
    
    private Coroutine musicCycleCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate AudioManager detected. Destroying new instance.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Safety: ensure sources are assigned
        if (foliageSource == null || musicSource == null || sfxSource == null)
        {
            Debug.LogError("One or more AudioSources are not assigned in AudioManager.");
        }

        // Configure foliage to loop
        if (foliageSource != null)
        {
            foliageSource.loop = true;
        }

        if (musicSource != null)
        {
            musicSource.loop = false; // music plays once each time
        }
    }

    private void Start()
    {
        PlayFoliage();
        musicCycleCoroutine = StartCoroutine(MusicCycleRoutine());
    }

    public void Chime() => PlayOneShotSafe(chime);
    public void LowClick() => PlayOneShotSafe(lowClick);
    public void Pling() => PlayOneShotSafe(pling);
    public void Pluck() => PlayOneShotSafe(pluck);
    public void Shake() => PlayOneShotSafe(shake);
    public void Tok() => PlayOneShotSafe(tok);


    private void PlayOneShotSafe(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;

        float now = Time.unscaledTime;
        if (_lastPlayed.TryGetValue(clip, out float last) && now - last < minRepeatInterval)
            return; // skip because it was just played

        _lastPlayed[clip] = now;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayFoliage()
    {
        if (foliageSource == null || foliage == null) return;
        if (foliageSource.clip != foliage)
            foliageSource.clip = foliage;
        if (!foliageSource.isPlaying)
            foliageSource.Play();
    }

    public void PlayMusicOnce()
    {
        if (musicSource == null || music == null) return;
        musicSource.clip = music;
        musicSource.Play();
    }

    private IEnumerator MusicCycleRoutine()
    {
        // initial foliage-only delay before first music
        float initialDelay = Random.Range(minFoliageOnlyDuration, maxFoliageOnlyDuration);
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            PlayMusicOnce();

            // wait until music finishes
            yield return new WaitUntil(() => musicSource != null && !musicSource.isPlaying);

            // random foliage-only duration between min and max before next music
            float delay = Random.Range(minFoliageOnlyDuration, maxFoliageOnlyDuration);
            yield return new WaitForSeconds(delay);
        }
    }

    public void StopMusicCycle()
    {
        if (musicCycleCoroutine != null)
        {
            StopCoroutine(musicCycleCoroutine);
            musicCycleCoroutine = null;
        }

        if (musicSource != null)
            musicSource.Stop();
    }

    public void RestartMusicCycle()
    {
        StopMusicCycle();
        musicCycleCoroutine = StartCoroutine(MusicCycleRoutine());
    }
}
