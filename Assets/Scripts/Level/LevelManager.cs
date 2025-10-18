using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Scenes")]
    public string winSceneName = "WinScene";
    public string loseSceneName = "LoseScene";
    public GameObject player;
    public GameObject finalBoss;

    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
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
}
