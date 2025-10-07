using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public HealthSystem playerHealth;

    public Slider slider;
    public TMP_Text healthText;

    void Start()
    {
        if (slider != null) slider.value = 0f;

        if (playerHealth != null)
        {
            // init UI immediately
            UpdateBar(playerHealth.currentHealth, playerHealth.maxHealth);

            // subscribe to changes
            playerHealth.OnHealthChange += UpdateBar;
        }
    }

    void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChange -= UpdateBar;
    }

    // called by HealthSystem event
    private void UpdateBar(int cur, int max)
    {
        float pct = (max > 0) ? Mathf.Clamp01((float)cur / max) : 0f;
        if (slider != null) slider.value = pct;

        if (healthText != null)
            healthText.text = cur + " / " + max;
    }
}
