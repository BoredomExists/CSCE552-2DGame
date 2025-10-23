using UnityEngine;


/// <summary>
/// Audio Controller for the player's actions
/// </summary>
public class PlayerAudioController : MonoBehaviour
{
    [Header("References")]
    public AudioSource playerLoopSource;                    // Audio Source for Looping Audio Clips
    public AudioSource playerSFXSource;                     // Audio Source for One Shot Audio Clips

    [Header("Audio Clips")]
    public AudioClip walking;                               // Audio for player walking
    public float sprintingPitch = 1.5f;                     // Audio Pitch Change to represent player sprinting
    public AudioClip jumping;                               // Audio for player Jumping
    public AudioClip landing;                               // Audio for player landing
    public AudioClip swordAttack;                           // Audio for attacking with sword
    public AudioClip projectileAttack;                      // Audio for firing projectiles
    public AudioClip gravityLaunch;                         // Audio for Gravity Launch Ability
    public AudioClip gravityLaunchLand;                     // Audio for Gravity Launch Ability Landing
    public AudioClip repulsorWave;                          // Audio for Repuslor Wave ability
    public AudioClip playerDeath;                           // Audio for player being defeated
    public AudioClip playerHit;                             // Audio for player getting hit

    void Awake()
    {
        // Gets Audio Sources and preset some values
        playerLoopSource = GetComponents<AudioSource>()[0];
        playerSFXSource = GetComponents<AudioSource>()[1];

        playerLoopSource.playOnAwake = false;
        playerSFXSource.playOnAwake = false;

        playerLoopSource.loop = true;
        playerSFXSource.loop = false;
    }

    /// <summary>
    /// Functions to call for player sfx audio
    /// </summary>
    public void PlayWalking()
    {
        playerLoopSource.pitch = 1f;
        playerLoopSource.clip = walking;
        if (!playerLoopSource.isPlaying) playerLoopSource.Play();
    }
    public void PlaySprinting()
    {
        playerLoopSource.pitch = sprintingPitch;
        playerLoopSource.clip = walking;
        if (!playerLoopSource.isPlaying) playerLoopSource.Play();
    }

    public void StopPlayingWalkingClips()
    {
        if (playerLoopSource.isPlaying && playerLoopSource.clip == walking)
        {
            playerLoopSource.Stop();
            playerLoopSource.loop = false;
        }
    }

    public void PlayJump()
    {
        playerSFXSource.PlayOneShot(jumping);
    }

    public void PlayLanding()
    {
        playerSFXSource.PlayOneShot(landing);
    }

    public void PlaySwordAttack()
    {
        playerSFXSource.PlayOneShot(swordAttack);
    }

    public void PlayProjectileAttack()
    {
        playerSFXSource.PlayOneShot(projectileAttack);
    }

    public void PlayGravityLaunch()
    {
        playerSFXSource.PlayOneShot(gravityLaunch);
    }

    public void PlayGravityLaunchLand()
    {
        playerSFXSource.PlayOneShot(gravityLaunchLand);
    }

    public void PlayRepulsorWave()
    {
        playerSFXSource.PlayOneShot(repulsorWave);
    }

    public void PlayPlayerDeath()
    {
        playerSFXSource.PlayOneShot(playerDeath);
    }

    public void PlayPlayerHit()
    {
        playerSFXSource.PlayOneShot(playerHit);
    }

    public void SetSFXVolume(float vol)
    {
        playerSFXSource.volume = Mathf.Clamp01(vol);
    }

    public void PausePlayerAudio()
    {
        playerSFXSource.Pause();
    }

    public void UnPausePlayerAudio()
    {
        playerSFXSource.UnPause();
    }
}
