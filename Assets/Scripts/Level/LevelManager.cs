using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Scenes")]
    public string winSceneName = "WinScene";
    public string loseSceneName = "LoseScene";
    public GameObject player;
    public GameObject finalBoss;

    [Header("Pause Settings")]
    public GameObject pauseMenu;
    public GameObject playBTN;
    public GameObject pauseBTN;

    [Header("Enemies (Mini-Boss Room A)")]
    public GameObject EA1;
    public GameObject EA2;
    public GameObject EA3;
    public GameObject key1;

    [Header("Enemies (Mini-Boss Room B)")]
    public GameObject EB1;
    public GameObject EB2;
    public GameObject key2;

    [Header("Animation")]
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        player = GameObject.FindWithTag("Player");
        finalBoss = GameObject.FindWithTag("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            LoadScreen(loseSceneName);
        }

        if (finalBoss == null)
        {
            //LoadScreen(winSceneName);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (playBTN.activeSelf)
                PauseGame();
            else
                UnPause();
        }

        if (key1 != null)
        {
            CheckMBRoomA();
        }
        if (key2 != null)
        {
            CheckMBRoomB();
        }
    }

    public void LoadScreen(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        playBTN.SetActive(false);
        pauseBTN.SetActive(true);
        pauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        playBTN.SetActive(true);
        pauseBTN.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void QuitToMM()
    {
        LoadingScreen.DisableLevelGO();
        SceneManager.LoadScene("MainMenu");
    }

    public void CheckMBRoomA()
    {
        if (EA1 == null && EA2 == null && EA3 == null)
        {
            key1.SetActive(true);
        }
    }

    public void CheckMBRoomB()
    {
        if (EB1 == null && EB2 == null)
        {
            key2.SetActive(true);
        }
    }
}
