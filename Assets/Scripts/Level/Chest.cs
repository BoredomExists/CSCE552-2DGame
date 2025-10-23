using UnityEngine;


/// <summary>
/// Manages the Chest's Animation
/// Upgrades the Players stats when unlocked
/// </summary>
public class Chest : MonoBehaviour
{
    [Header("References")]
    public UserInput userInput;                                                             // UserInput Script of Player Game Object
    public HealthSystem playerHS;                                                           // HealthSystem of Player Game Object
    public Animator animator;                                                               // Animator Component

    public AudioSource chestAudio;                                                          // Audio Source for the Chest

    [Header("Audio Clip")]
    public AudioClip chestOpen;

    void Start()
    {
        animator = GetComponent<Animator>();                                                    // Gets the Chest Animator
        userInput = GameObject.FindGameObjectWithTag("Player").GetComponent<UserInput>();       // Gets the User Input off the player
        playerHS = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();     // Gets the Health System off the player
        chestAudio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;                                            // Makes sure nothing else causes the trigger

        if (collision.CompareTag("Player"))
        {
            chestAudio.PlayOneShot(chestOpen);
            // Upgrades the Player's Stats (Congrats, you unlocked the secret room!)
            userInput.SetDamage(100);
            playerHS.SetMaxHealth(200);
            playerHS.Heal(200);
            animator.SetTrigger("OpenChest");                                                   // Starts trigger animation for chest
        }
    }
}
