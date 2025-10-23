using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the UI Element representing the player health bar
/// </summary>
public class PlayerHealthBar : MonoBehaviour
{
    [Header("References")]
    public PlayerAudioController playerAudio;       // Player Audio Controller
    public Animator animator;       // Animator of Player
    public HealthSystem hs;         // Health System of Player
    public Slider slider;           // Slider of Health Bar
    public TMP_Text text;           // Health Points of Health Bar

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hs = GetComponent<HealthSystem>();              // Gets the player health system
        playerAudio = GetComponent<PlayerAudioController>(); // Gets the player audio controller
        UpdateUI();                                     // Updates the UI of the Health bar

        animator = GetComponentInChildren<Animator>();  // Gets animator of the player sprite
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if the health has changed, if so Update the UI
        if (hs.IsHealthChanged())
        {
            UpdateUI();
            if (hs.GetCurrentHealth() <= 0)
            {
                StartCoroutine(PlayerDeath());
            }
        }

        /**
        if (Input.GetKeyDown(KeyCode.E))
        {
            hs.Heal(100);
            UpdateUI();
        }
        */
    }

    // Updates the slider and text of the player health bar
    private void UpdateUI()
    {
        slider.value = (float)hs.GetCurrentHealth() / hs.GetMaxHealth();
        text.text = hs.GetCurrentHealth() + " / " + hs.GetMaxHealth();
    }

    // Animation for when the player's health == 0
    IEnumerator PlayerDeath()
    {
        animator.SetTrigger("isDead");
        playerAudio.PlayPlayerDeath();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
