using System.Collections;
using UnityEngine;


/// <summary>
/// Key Manager for the UI elements representing the keys
/// </summary>
public class Key : MonoBehaviour
{
    [Header("References")]
    public GameObject key1;
    public GameObject key2;
    public AudioSource keyAudio;

    [Header("Audio Clips")]
    public AudioClip getKey;

    void Start()
    {
        keyAudio = GetComponent<AudioSource>();
        keyAudio.volume = .2f;
    }


    // If the player collides with the key game object, enable the key UI element.
    // If have a key, enable second key
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            keyAudio.PlayOneShot(getKey);
            StartCoroutine(DestroyKey());
        }
    }

    IEnumerator DestroyKey()
    {
        yield return new WaitForSeconds(1f);
        if (!key1.activeSelf)
        {
            key1.SetActive(true);
        }
        else
            key2.SetActive(true);
        Destroy(gameObject);
    }
}
