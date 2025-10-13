using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public HealthSystem hs;
    public Slider slider;
    public TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hs = GetComponent<HealthSystem>();
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            hs.TakeDamage(20);
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            hs.Heal(100);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        slider.value = (float)hs.GetCurrentHealth() / hs.GetMaxHealth();
        text.text = hs.GetCurrentHealth() + " / " + hs.GetMaxHealth();
    }
}
