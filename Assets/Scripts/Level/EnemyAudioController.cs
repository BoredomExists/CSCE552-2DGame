using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Audio Controller to manage sfx sounds for the enemies
/// </summary>
public class EnemyAudioController : MonoBehaviour
{
    public static readonly List<EnemyAudioController> All = new List<EnemyAudioController>();
    [Header("References")]
    public AudioSource enemyLoopAudioSource;      // Audio Source for looping clips
    public AudioSource enemySFXAudioSource;       // Audio Source for SFX clips

    [Header("Audio Clips")]
    public AudioClip enemyHit;                    // Enemy Getting Hit
    public AudioClip enemyGrunt;                  // Enemy Making Noise
    public AudioClip enemyIdle;                   // Enemy Idle Sound
    public AudioClip enemyMove;                   // Enemy Moving Sound
    public AudioClip enemyAttack;                 // Enemy Attacking Sound
    public AudioClip enemyDeath;                  // Enemy Death Sound
    public AudioClip enemyProjectile;             // Enemy Firing projectiles

    void OnEnable()
    {
        // Gets audio components and set some default settings
        enemyLoopAudioSource = GetComponents<AudioSource>()[0];
        enemySFXAudioSource = GetComponents<AudioSource>()[1];

        enemyLoopAudioSource.playOnAwake = false;
        enemySFXAudioSource.playOnAwake = false;

        enemyLoopAudioSource.loop = true;
        enemySFXAudioSource.loop = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Gets audio components and set some default settings
        enemyLoopAudioSource = GetComponents<AudioSource>()[0];
        enemySFXAudioSource = GetComponents<AudioSource>()[1];

        enemyLoopAudioSource.playOnAwake = false;
        enemySFXAudioSource.playOnAwake = false;

        enemyLoopAudioSource.loop = true;
        enemySFXAudioSource.loop = false;

        SetSFXVolume(0.1f);
    }

    /// <summary>
    /// Functions to call to play, pause, and resume audio sources
    /// </summary>
    public void PlayMoving()
    {
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
        enemySFXAudioSource.PlayOneShot(enemyHit);
    }

    public void PlayEnemyGrunt()
    {
        enemySFXAudioSource.PlayOneShot(enemyGrunt);
    }

    public void PlayEnemyIdle()
    {
        enemySFXAudioSource.PlayOneShot(enemyIdle);
    }

    public void PlayEnemyAttack()
    {
        enemySFXAudioSource.PlayOneShot(enemyAttack);
    }

    public void PlayEnemyProjectile()
    {
        enemySFXAudioSource.PlayOneShot(enemyProjectile);
    }

    public void PlayEnemyDeath()
    {
        enemySFXAudioSource.PlayOneShot(enemyDeath);
    }

    public void SetSFXVolume(float vol)
    {
        enemyLoopAudioSource.volume = Mathf.Clamp01(vol);
        enemySFXAudioSource.volume = Mathf.Clamp01(vol);
    }

    public void PauseEnemyAudio()
    {
        enemyLoopAudioSource.Pause();
        enemySFXAudioSource.Pause();
    }

    public void UnPauseEnemyAudio()
    {
        enemyLoopAudioSource.UnPause();
        enemySFXAudioSource.UnPause();
    }
}
