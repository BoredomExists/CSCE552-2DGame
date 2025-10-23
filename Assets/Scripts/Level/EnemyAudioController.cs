using UnityEngine;

/// <summary>
/// Audio Controller to manage sfx sounds for the enemies
/// </summary>
public class EnemyAudioController : MonoBehaviour
{
    [Header("References")]
    public AudioSource enemyLoopAudioSource;      // Audio Source for looping clips
    public AudioSource enemySFXAudioSource;       // Audio Source for SFX clips

    [Header("Audio Clips")]
    public AudioClip enemyHit;
    public float enemyHitVolume = 0.5f;
    public AudioClip enemyGrunt;
    public float enemyGruntVolume = 0.5f;
    public AudioClip enemyIdle;
    public float enemyIdleVolume = 0.5f;
    public AudioClip enemyMove;
    public float enemyMoveVolume = 0.5f;
    public AudioClip enemyAttack;
    public float enemyAttackVolume = 0.5f;
    public AudioClip enemyDeath;
    public float enemyDeathVolume = 0.5f;
    public AudioClip enemyProjectile;
    public float enemyProjectileVolume = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyLoopAudioSource = GetComponents<AudioSource>()[0];
        enemySFXAudioSource = GetComponents<AudioSource>()[1];

        enemyLoopAudioSource.playOnAwake = false;
        enemySFXAudioSource.playOnAwake = false;

        enemyLoopAudioSource.loop = true;
        enemySFXAudioSource.loop = false;
    }

    public void PlayMoving()
    {
        enemyLoopAudioSource.volume = enemyMoveVolume;
        enemyLoopAudioSource.clip = enemyMove;
        if (!enemyLoopAudioSource.isPlaying) enemyLoopAudioSource.Play();
    }

    public void StopPlayingWalkingClip()
    {
        if (enemyLoopAudioSource.isPlaying && enemyLoopAudioSource.clip == enemyMove)
        {
            enemyLoopAudioSource.Stop();
            enemyLoopAudioSource.loop = false;
        }
    }

    public void PlayEnemyHit()
    {
        enemySFXAudioSource.volume = enemyHitVolume;
        enemySFXAudioSource.PlayOneShot(enemyHit);
    }

    public void PlayEnemyGrunt()
    {
        enemySFXAudioSource.volume = enemyGruntVolume;
        enemySFXAudioSource.PlayOneShot(enemyGrunt);
    }

    public void PlayEnemyIdle()
    {
        enemySFXAudioSource.volume = enemyIdleVolume;
        enemySFXAudioSource.PlayOneShot(enemyIdle);
    }

    public void PlayEnemyAttack()
    {
        enemySFXAudioSource.volume = enemyAttackVolume;
        enemySFXAudioSource.PlayOneShot(enemyAttack);
    }

    public void PlayEnemyProjectile()
    {
        enemySFXAudioSource.volume = enemyProjectileVolume;
        enemySFXAudioSource.PlayOneShot(enemyProjectile);
    }

    public void PlayEnemyDeath()
    {
        enemySFXAudioSource.volume = enemyDeathVolume;
        enemySFXAudioSource.PlayOneShot(enemyDeath);
    }
}
