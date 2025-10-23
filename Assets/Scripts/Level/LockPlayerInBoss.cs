using System.Collections;
using UnityEngine;

/// <summary>
/// A Check system to lock player in boss room when entered fully
/// </summary>
public class LockPlayerInBoss : MonoBehaviour
{
    [Header("References")]
    public GameObject bossDoor;                                             // Final Boss Door (removed so it doesnt look clunky)
    public GameObject wallBlock;                                            // Setup to block player in the boss room

    public LevelManagerAudioController levelManager;                       // Level Manager to start boss music

    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManagerAudioController>();
    }

    // Setup to trap character into the boss fight
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (collision.CompareTag("Player"))
        {
            bossDoor.SetActive(false);
            wallBlock.SetActive(true);
            StartCoroutine(PlayBossMusic());
        }
    }

    // Setup to play when entering the room
    IEnumerator PlayBossMusic()
    {
        yield return new WaitForSeconds(3f);
        levelManager.StopMusicSource();
        yield return new WaitForSeconds(1f);
        levelManager.PlayBossMusic();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
