using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Manages Music and SFX Audio changes from pause menu audio settings sliders
/// </summary>
public class AudioSettings : MonoBehaviour
{
    public static AudioSettings Instance { get; private set; }

    [Header("References")]
    public Slider musicSlider;                                              // Music Audio Slider
    public Slider sfxSlider;                                                // SFX Audio Slider

    public LevelManagerAudioController lmAudio;                             // Level Manager Audio
    public PlayerAudioController playerAudio;                               // Player Audio
    public EnemyAudioController enemyAudio;                                 // Enemy Audio
    public FinalBossAudioController finalBossAudio;                         // Final Boss Audio

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        // Gets each audio source
        lmAudio = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManagerAudioController>();
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudioController>();
        enemyAudio = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAudioController>();
        finalBossAudio = LevelManager.Instance.GetFinalBoss().GetComponent<FinalBossAudioController>();

        // Presets slider values
        musicSlider.minValue = 0f;
        musicSlider.maxValue = 1f;
        musicSlider.value = 0.1f;

        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;
        sfxSlider.value = 0.1f;

        // Gets the default values for sliders
        musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
        sfxSlider.onValueChanged.AddListener(onSFXValueChanged);

        StartCoroutine(SetAudioSettings());
    }

    // Sets the default values for the audio sources
    IEnumerator SetAudioSettings()
    {
        yield return new WaitForSeconds(2f);
        lmAudio.SetMusicVolume(0.1f);
        lmAudio.SetSFXVolume(0.1f);
        playerAudio.SetSFXVolume(0.1f);
        enemyAudio.SetSFXVolume(0.1f);
        finalBossAudio.SetSFXVolume(0.1f);
    }

    // Removes the listeners when the slider do not exists (Exit game)
    void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(OnMusicValueChanged);
        sfxSlider.onValueChanged.RemoveListener(onSFXValueChanged);
    }

    // Changes the background/boss music value
    private void OnMusicValueChanged(float vol)
    {
        lmAudio.SetMusicVolume(vol);
    }

    // Changes all sfx audio
    private void onSFXValueChanged(float vol)
    {
        playerAudio.SetSFXVolume(vol);

        var enemies = FindObjectsOfType<EnemyAudioController>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetSFXVolume(vol);
        }

        if (finalBossAudio != null)
            finalBossAudio.SetSFXVolume(vol);
        if (lmAudio.lmAudioSource != null)
            lmAudio.lmAudioSource.volume = vol;
    }

    public float GetCurrentSFXValue()
    {
        return sfxSlider.value;
    }
}
