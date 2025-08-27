using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, ICanBePaused
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup masterGroup;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("TestBackgroundMusic")]
    [SerializeField] private AudioClip audioClip;

    private AudioSource musicSource;
    private List<AudioSource> activeSFX = new List<AudioSource>();

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
    }
    private void OnDisable()
    {
        PauseManager.OnPauseChanged -= OnPausedChanged;
    }
    private void Start()
    {
        LoadSettings();
        PlayMusic(audioClip);
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

    public void PlaySFX(AudioClip clip, Vector3? position = null, float volume = 1f)
    {
        if (clip == null) return;

        GameObject go = new GameObject("SFX_" + clip.name);
        if (position.HasValue)
            go.transform.position = position.Value;
        else
            go.transform.parent = transform;

        AudioSource source = go.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = sfxGroup;
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = position.HasValue ? 1f : 0f;
        source.Play();

        activeSFX.Add(source);

        StartCoroutine(DestroyAfterPlaying(source));
    }

    private IEnumerator DestroyAfterPlaying(AudioSource source)
    {
        yield return new WaitUntil(() => !source.isPlaying && !isPaused);
        Debug.Log("DestroyAfterPlaying");
        Destroy(source.gameObject);
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

    public void OnPausedChanged(bool paused)
    {
        isPaused = paused;

        if (paused)
            PauseSFX();
        else 
            ResumeSFX();
    }
}
