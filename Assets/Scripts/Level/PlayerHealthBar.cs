using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Animator animator;
    public HealthSystem hs;
    public Slider slider;
    public TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hs = GetComponent<HealthSystem>();
        UpdateUI();

        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hs.IsHealthChanged())
        {
            UpdateUI();
            if (hs.GetCurrentHealth() <= 0)
            {
                StartCoroutine(PlayerDeath());
            }
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

    IEnumerator PlayerDeath()
    {
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
