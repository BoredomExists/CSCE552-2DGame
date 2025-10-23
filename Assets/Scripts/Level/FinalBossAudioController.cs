using UnityEngine;

/// <summary>
/// Audio Controller to handle sfx sounds for the Final Boss
/// </summary> 
public class FinalBossAudioController : MonoBehaviour
{
    [Header("References")]
    public AudioSource bossAudioSource;             // Audio Source for the final boss

    [Header("Audio Clips")]
    public AudioClip shield;                        // Audio Clip for Shield Animation
    public AudioClip slam;                          // Audio Clip for Slam Animation

    void Awake()
    {
        bossAudioSource = GetComponent<AudioSource>();          // Gets the Audio Source
    }

    /// <summary>
    /// Functions to call to play, pause, resume boss sfx audio
    /// </summary>
    public void PlayShieldAudio()
    {
        bossAudioSource.PlayOneShot(shield);
    }

    public void PlaySlamAudio()
    {
        bossAudioSource.PlayOneShot(slam);
    }

    public void SetSFXVolume(float vol)
    {
        bossAudioSource.volume = Mathf.Clamp01(vol);
    }

    public void PauseBossAudio()
    {
        bossAudioSource.Pause();
    }
    public void UnPauseBossAudio()
    {
        bossAudioSource.UnPause();
    }
}
