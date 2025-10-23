using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Loading Screen Manager to preload the level
/// </summary>
public class LoadingScreen : MonoBehaviour
{
    [Header("References")]
    public static string levelSceneName = "Level";      // Level Scene Name
    public AudioSource mainMenuAudio;                   // Audio Source to play background music

    [Header("Audio Clip")]
    public AudioClip backgroundMusic;
    public float backgroundMusicVolume = 0.5f;

    [Header("UI Elements")]
    public GameObject loadingScreen;                    // Loading Screen Game Object
    public GameObject mainMenu;                         // Main Menu Game Object
    public Slider loadingScreenSlider;                  // Slider for Loading Screen progress
    public TMP_Text loadingText;                        // Text for Loading Screen progress

    AsyncOperation loadOperation;                       // Load Operation to preload the level scene
    string menuSceneName;

    // Handles Presetting Game Objects and values
    void Awake()
    {
        menuSceneName = SceneManager.GetActiveScene().name;
        mainMenuAudio = GetComponent<AudioSource>();

        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);
        loadingScreenSlider.value = 0f;
        loadingText.text = "0%";
        StartCoroutine(PreloadLevel());     // Starts Preloading the Level Scene
    }

    IEnumerator PreloadLevel()
    {
        loadOperation = SceneManager.LoadSceneAsync(levelSceneName, LoadSceneMode.Additive);        // Loads the scene along with the current scene
        loadOperation.allowSceneActivation = false;                                                 // Makes sure the scene does not automatically become primary

        // Fills Loading Bar based on progress of preloading level
        while (loadOperation.progress < 0.9f)
        {
            loadingScreenSlider.value = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingText.text = (Mathf.Clamp01(loadOperation.progress / 0.9f) * 100f).ToString() + "%";
            yield return null;
        }
        loadingScreenSlider.value = 1f;
        loadingText.text = "100%";

        // Disable preloaded level game objects so they do not merge with main menu scene
        DisableLevelGO();
        loadingScreen.SetActive(false);
        mainMenu.SetActive(true);
        StartCoroutine(PlayBackgroundMusic());
    }

    // Function for Play button to activate the preloaded level scene
    public void StartGame()
    {
        StartCoroutine(ActivatePreloadLevel());
    }

    // Function for quit button to exit game
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // IEnumerator to start the level
    IEnumerator ActivatePreloadLevel()
    {
        loadOperation.allowSceneActivation = true;              // Reenables load operation scene activation
        while (!loadOperation.isDone) yield return null;

        // Loads the level scene and unloads the main menu scene
        var level = SceneManager.GetSceneByName(levelSceneName);
        if (level.IsValid())
        {
            EnableLevelGO();
            SceneManager.SetActiveScene(level);
            SceneManager.UnloadSceneAsync(menuSceneName);
        }
    }

    // Disable game objects in Level scene when preloading
    public static void DisableLevelGO()
    {
        var GOs = SceneManager.GetSceneByName(levelSceneName).GetRootGameObjects();
        for (int i = 0; i < GOs.Length; i++)
        {
            if (GOs[i]) GOs[i].SetActive(false);
        }
    }

    // Reenable game objects when user presses play to go into the level
    public void EnableLevelGO()
    {
        var GOs = SceneManager.GetSceneByName(levelSceneName).GetRootGameObjects();
        for (int i = 0; i < GOs.Length; i++)
        {
            if (GOs[i]) GOs[i].SetActive(true);
        }
    }

    IEnumerator PlayBackgroundMusic()
    {
        yield return new WaitForSeconds(1f);
        mainMenuAudio.clip = backgroundMusic;
        mainMenuAudio.loop = true;
        mainMenuAudio.volume = backgroundMusicVolume;
        mainMenuAudio.Play();
    }
}
