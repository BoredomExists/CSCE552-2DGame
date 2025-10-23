using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the level such as
/// Win/Lose Conditions
/// Pause Menu
/// Key Spawns
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("References")]
    public LevelManagerAudioController lmAudio;                 // Audio Controller of the level manager

    [Header("Scenes")]
    public string winSceneName = "WinScene";                    // Name of Scene to load when player wins
    public string loseSceneName = "LoseScene";                  // Name of Scene to load when player loses
    public GameObject player;                                   // Gets Player Game Object to check when defeated
    public GameObject finalBoss;                                // Gets Final Boss Game Object to check when defeated

    [Header("Pause Settings")]
    public GameObject pauseMenu;                                // Game Object representing the pause menu
    public GameObject playBTN;                                  // Game Object representing the play button
    public GameObject pauseBTN;                                 // Game Object representing the pause button

    [Header("Enemies (Mini-Boss Room A)")]
    public GameObject EA1;                                      // Game Objects representing the mini-boss enemies in Boss Room A
    public GameObject EA2;
    public GameObject EA3;
    public GameObject key1;                                     // Key to set active when all Boss Room A is cleared

    [Header("Enemies (Mini-Boss Room B)")]
    public GameObject EB1;                                      // Game Objects representing the mini-boss enemies in Boss Room B
    public GameObject EB2;
    public GameObject key2;                                     // Key to set active when all Boss Room B is cleared

    [Header("Animation")]
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Time.timeScale = 1f;                                    // Sets the Time Scale to 1 incase it was set to 0 from other scripts
        Instance = this;
        lmAudio = GetComponent<LevelManagerAudioController>();  // Gets the audio controller
        player = GameObject.FindWithTag("Player");              // Gets the player
    }

    // Update is called once per frame
    void Update()
    {
        // Check Lose Condition
        if (player == null)
        {
            LoadScreen(loseSceneName);
        }

        // Check Win Condition
        if (GetFinalBoss() == null)
        {
            LoadScreen(winSceneName);
        }

        // Secondary way of accessing pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (playBTN.activeSelf)
                PauseGame();
            else
                UnPause();
        }

        // Check to see key from mini-boss has been grabbed
        if (key1 != null)
        {
            CheckMBRoomA();
        }
        if (key2 != null)
        {
            CheckMBRoomB();
        }
    }

    public void SetFinalBoss(GameObject boss)
    {
        finalBoss = boss;
    }

    public GameObject GetFinalBoss() => finalBoss;

    // Starts coroutine to load win/lose scenes
    public void LoadScreen(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    // Starts fade out animation to swap to win/lose screen
    IEnumerator LoadLevel(string sceneName)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }

    // Pauses game
    public void PauseGame()
    {
        lmAudio.PlayUIButtonPress();
        Time.timeScale = 0f;
        playBTN.SetActive(false);
        pauseBTN.SetActive(true);
        pauseMenu.SetActive(true);

        // Pauses all audio sources
        lmAudio.PauseAllAudio();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudioController>().PausePlayerAudio();

        var enemies = FindObjectsOfType<EnemyAudioController>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyAudioController>().PauseEnemyAudio();
        }

        GetFinalBoss().GetComponent<FinalBossAudioController>().PauseBossAudio();

    }

    // Unpauses game
    public void UnPause()
    {
        lmAudio.PlayUIButtonPress();
        Time.timeScale = 1f;
        playBTN.SetActive(true);
        pauseBTN.SetActive(false);
        pauseMenu.SetActive(false);

        // Unpauses any audio currently paused
        lmAudio.UnPauseAllAudio();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudioController>().UnPausePlayerAudio();

        var enemies = FindObjectsOfType<EnemyAudioController>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyAudioController>().UnPauseEnemyAudio();
        }

        GetFinalBoss().GetComponent<FinalBossAudioController>().UnPauseBossAudio();
    }

    // Function for pause menu button to go to main menu
    public void QuitToMM()
    {
        lmAudio.PlayUIButtonPress();
        LoadingScreen.DisableLevelGO();
        SceneManager.LoadScene("MainMenu");
    }

    // Check to see if all enemies in Mini-Boss Room A are defeated, if so, spawn key
    public void CheckMBRoomA()
    {
        if (EA1 == null && EA2 == null && EA3 == null)
        {
            key1.SetActive(true);
        }
    }

    // Check to see if all enemies in Mini-Boss Room B are defeated, if so, spawn key
    public void CheckMBRoomB()
    {
        if (EB1 == null && EB2 == null)
        {
            key2.SetActive(true);
        }
    }
}
