using UnityEngine;


/// <summary>
/// Audio Controller for the player's actions
/// </summary>
public class PlayerAudioController : MonoBehaviour
{
    [Header("References")]
    public AudioSource playerLoopSource;
    public AudioSource playerSFXSource;

    [Header("Audio Clips")]
    public AudioClip walking;
    public float walkingVolume = 0.5f;
    public float sprintingPitch = 1f;
    public AudioClip jumping;
    public float jumpingVolume = 0.5f;
    public AudioClip landing;
    public float landingVolume = 0.5f;
    public AudioClip swordAttack;
    public float swordAttackVolume = 0.5f;
    public AudioClip projectileAttack;
    public float projectileAttackVolume = 0.5f;
    public AudioClip gravityLaunch;
    public float gravityLaunchVolume = 0.5f;
    public AudioClip gravityLaunchLand;
    public float gravityLaunchLandVolume = 0.5f;
    public AudioClip repulsorWave;
    public float repulsorWaveVolume = 0.5f;

    void Awake()
    {
        playerLoopSource = GetComponents<AudioSource>()[0];
        playerSFXSource = GetComponents<AudioSource>()[1];

        playerLoopSource.playOnAwake = false;
        playerSFXSource.playOnAwake = false;

        playerLoopSource.loop = true;
        playerSFXSource.loop = false;
    }

    public void DisableLoopAudio()
    {
        if (playerLoopSource.loop)
        {
            playerLoopSource.loop = false;
            playerLoopSource.Stop();
            playerLoopSource.clip = null;
        }
    }

    public void PlayWalking()
    {
        playerLoopSource.volume = walkingVolume;
        playerLoopSource.pitch = 1f;
        playerLoopSource.clip = walking;
        if (!playerLoopSource.isPlaying) playerLoopSource.Play();
    }
    public void PlaySprinting()
    {
        playerLoopSource.volume = walkingVolume;
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
        playerSFXSource.volume = jumpingVolume;
        playerSFXSource.PlayOneShot(jumping);
    }

    public void PlayLanding()
    {
        playerSFXSource.volume = landingVolume;
        playerSFXSource.PlayOneShot(landing);
    }

    public void PlaySwordAttack()
    {
        playerSFXSource.volume = swordAttackVolume;
        playerSFXSource.PlayOneShot(swordAttack);
    }

    public void PlayProjectileAttack()
    {
        playerSFXSource.volume = projectileAttackVolume;
        playerSFXSource.PlayOneShot(projectileAttack);
    }

    public void PlayGravityLaunch()
    {
        playerSFXSource.volume = gravityLaunchVolume;
        playerSFXSource.PlayOneShot(gravityLaunch);
    }

    public void PlayGravityLaunchLand()
    {
        playerSFXSource.volume = gravityLaunchLandVolume;
        playerSFXSource.PlayOneShot(gravityLaunchLand);
    }

    public void PlayRepulsorWave()
    {
        playerSFXSource.volume = repulsorWaveVolume;
        playerSFXSource.PlayOneShot(repulsorWave);
    }
}
