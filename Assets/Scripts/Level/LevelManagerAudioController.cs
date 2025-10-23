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
    public float backgroundMusicVolume = 0.1f;
    public AudioClip bossMusic;
    public float bossMusicVolume = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lmAudioSource = GetComponent<AudioSource>();
        musicSource = GetComponentInChildren<AudioSource>();
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic();
        }
    }

    public void PlayUIButtonPress()
    {
        lmAudioSource.PlayOneShot(UIBleep);
    }

    public void PlayBackgroundMusic()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = backgroundMusicVolume;
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
        musicSource.volume = bossMusicVolume;
        musicSource.Play();
    }
}
