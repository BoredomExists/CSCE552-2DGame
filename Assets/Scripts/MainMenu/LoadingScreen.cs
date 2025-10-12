using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public string levelSceneName = "Level";

    public GameObject loadingScreen;
    public GameObject mainMenu;
    public Slider loadingScreenSlider;
    public TMP_Text loadingText;

    AsyncOperation loadOperation;
    string menuSceneName;

    void Awake()
    {
        menuSceneName = SceneManager.GetActiveScene().name;

        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);
        loadingScreenSlider.value = 0f;
        loadingText.text = "0%";
        StartCoroutine(PreloadLevel());
    }

    IEnumerator PreloadLevel()
    {
        loadOperation = SceneManager.LoadSceneAsync(levelSceneName, LoadSceneMode.Additive);
        loadOperation.allowSceneActivation = false;

        while (loadOperation.progress < 0.9f)
        {
            loadingScreenSlider.value = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingText.text = (Mathf.Clamp01(loadOperation.progress / 0.9f) * 100f).ToString() + "%";
            yield return null;
        }
        loadingScreenSlider.value = 1f;
        loadingText.text = "100%";

        loadingScreen.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void StartGame()
    {
        StartCoroutine(ActivatePreloadLevel());
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    IEnumerator ActivatePreloadLevel()
    {
        loadOperation.allowSceneActivation = true;
        while (!loadOperation.isDone) yield return null;

        var level = SceneManager.GetSceneByName(levelSceneName);
        if (level.IsValid())
        {
            SceneManager.SetActiveScene(level);
            SceneManager.UnloadSceneAsync(menuSceneName);
        }
    }
}
