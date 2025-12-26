using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour, ICanBePaused
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Transform audioListenerTransform;
    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup masterGroup;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("TestBackgroundMusic")]
    [SerializeField] private AudioClip audioClip;

    [Header("AudioSourcePoolSize")]
    [SerializeField] private int sfxPoolSize = 20;

    private AudioSource[] sfxPool;
    private int poolIndex = 0;

    private List<AudioSource> activeSFX = new List<AudioSource>();

    private AudioSource musicSource;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);


        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
    }
    private void OnEnable()
    {
        PauseManager.OnPauseChanged += OnPausedChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        PauseManager.OnPauseChanged -= OnPausedChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void Start()
    {
        LoadSettings();
        CreatePositionalPool();
        PlayMusic(audioClip);
    }

    private void CreatePositionalPool()
    {
        sfxPool = new AudioSource[sfxPoolSize];
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 0f;
            src.outputAudioMixerGroup = sfxGroup;

            sfxPool[i] = src;
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume > 0 ? volume : 0.000001f) * 20);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume > 0 ? volume : 0.000001f) * 20);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume > 0 ? volume : 0.000001f) * 20);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }
    public void StopMusic() => musicSource.Stop();

    public void PlaySFX(AudioClip clip, Vector3? pos = null, float volume = 1f)
    {
        if (clip == null) return;

        poolIndex = (poolIndex + 1) % sfxPool.Length;
        AudioSource src = sfxPool[poolIndex];

        Vector3 listenerPos = audioListenerTransform.position;
        float distance = pos.HasValue ? Vector3.Distance(listenerPos, pos.Value) : 0f;

        float distanceVolume = 1f - Mathf.Clamp01(distance / 30f);

        src.spatialBlend = 0f;
        src.volume = volume * distanceVolume;

        activeSFX.Add(src);

        src.PlayOneShot(clip);
        StartCoroutine(TrackSFX(src, clip.length));
    }

    public void PlayCantPausedSFX(AudioClip clip, Vector3? pos = null, float volume = 1f)
    {
        if (clip == null) return;

        poolIndex = (poolIndex + 1) % sfxPool.Length;
        AudioSource src = sfxPool[poolIndex];

        Vector3 listenerPos = audioListenerTransform.position;
        float distance = pos.HasValue ? Vector3.Distance(listenerPos, pos.Value) : 0f;

        // curse
        float distanceVolume = 1f - Mathf.Clamp01(distance / 30f);

        src.spatialBlend = 0f;
        src.volume = volume * distanceVolume;

        src.PlayOneShot(clip);
    }


    private IEnumerator TrackSFX(AudioSource src, float duration)
    {
        float timer = 0f;

        while (timer < duration || isPaused)
        {
            if (!isPaused)
                timer += Time.deltaTime;

            yield return null;
        }

        activeSFX.Remove(src);
    }

    public void PauseSFX()
    {
        foreach (var s in activeSFX)
        {
            if (s != null && s.isPlaying) s.Pause();
        }
    }

    public void ResumeSFX()
    {
        foreach (var s in activeSFX)
        {
            if (s != null) s.UnPause();
        }
    }

    public void LowerMusicOnPause(float factor = 0.3f)
    {
        if (musicSource != null)
            musicSource.volume = factor;
    }

    public void RestoreMusicVolume()
    {
        if (musicSource != null)
            musicSource.volume = 1f;
    }

    // === SETTINGS ===
    public void LoadSettings()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        SetMasterVolume(master);
        SetMusicVolume(music);
        SetSFXVolume(sfx);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioListenerTransform = FindObjectOfType<PlayerMovement>().transform;
    }

    public void OnPausedChanged(bool paused)
    {
        isPaused = paused;

        if (paused)
            PauseSFX();
        else 
            ResumeSFX();
    }
}
