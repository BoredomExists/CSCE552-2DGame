using UnityEngine;

/// <summary>
/// Audio Controller to handle sfx sounds for the Final Boss
/// </summary> 
public class FinalBossAudioController : MonoBehaviour
{
    [Header("References")]
    public AudioSource bossAudioSource;             // Audio Source for the final boss

    [Header("Audio Clips")]
    public AudioClip shield;
    public float shieldVolume = 0.5f;
    public AudioClip slam;
    public float slamVolume = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossAudioSource = GetComponent<AudioSource>();
    }

    public void PlayShieldAudio()
    {
        bossAudioSource.volume = shieldVolume;
        bossAudioSource.PlayOneShot(shield);
    }

    public void PlaySlamAudio()
    {
        bossAudioSource.volume = slamVolume;
        bossAudioSource.PlayOneShot(slam);
    }
}
