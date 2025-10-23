using UnityEngine;

/// <summary>
/// Audio Controller that will manage UI sfx sounds
/// </summary>
public class LevelManagerAudioController : MonoBehaviour
{
    [Header("References")]
    public AudioSource lmAudioSource;           // Gets the audio source that is attached to the level manager
    public AudioSource musicSource;               // Music source child to not interfere with the Level Manager UI Audio

    [Header("Audio Clips")]
    public AudioClip UIBleep;
    public AudioClip backgroundMusic;
    public AudioClip bossMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        lmAudioSource = GetComponent<AudioSource>();
        AudioSource[] sources = GetComponentsInChildren<AudioSource>(true);
        foreach (var s in sources)
        {
            if (s != lmAudioSource)
            {
                musicSource = s;
                break;
            }
        }
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic();
        }
    }


    /// <summary>
    /// Functions to call to play, pause, unpause, and set volumes for music and sfx audio
    /// </summary>
    public void PlayUIButtonPress()
    {
        lmAudioSource.PlayOneShot(UIBleep);
    }

    public void PlayBackgroundMusic()
    {
        musicSource.volume = 0.1f;
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusicSource()
    {
        musicSource.Stop();
    }

    public void PlayBossMusic()
    {
        musicSource.clip = bossMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void SetSFXVolume(float vol)
    {
        lmAudioSource.volume = Mathf.Clamp01(vol);
    }

    public void SetMusicVolume(float vol)
    {
        musicSource.volume = Mathf.Clamp01(vol);
    }

    public void PauseAllAudio()
    {
        lmAudioSource.Pause();
        musicSource.Pause();
    }

    public void UnPauseAllAudio()
    {
        lmAudioSource.UnPause();
        musicSource.UnPause();
        if (!musicSource.isPlaying)
            musicSource.Play();
    }
}
