using System.Collections;
using UnityEngine;

/// <summary>
/// Animation Script for the Boss Door
/// Checks if the Player has the two keys needed to unlock the door
/// </summary>
public class BossDoorAnimation : MonoBehaviour
{
    [Header("References")]
    public Animator animator;                                               // Animator Component of the Boss Door
    public BoxCollider2D colBox;                                           // Non-Trigger collider box of the Boss Door

    public GameObject key1;                                                // UI Element representing the first key
    public GameObject key2;                                                // UI Element representing the second key

    public AudioSource doorAudio;                                          // Audio Source on the final boss door
    

    [Header("Audio Clips")]
    public AudioClip doorUnlock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();                                // Gets the Animator of the Boss Door
        colBox.gameObject.SetActive(true);                                  // Gets the Non-Trigger Collider box to prevent player from walking through
        doorAudio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;                        // Make sure nothing else causes the trigger or collision
        if (checkKeys() && collision.CompareTag("Player"))                  // Checks to see if the player has obtained both keys and if so, unlock door
        {
            doorAudio.PlayOneShot(doorUnlock);
            animator.SetBool("bossDoorOpen", true);
        }
    }

    // Checks to see if the player has both keys in the UI
    private bool checkKeys()
    {
        return key1.activeSelf && key2.activeSelf;
    }

    // Disables the Non-Trigger Collider on the Boss Door
    public void DisableBossDoorCollider()
    {
        colBox.GetComponents<BoxCollider2D>()[1].enabled = false;
    }
}
