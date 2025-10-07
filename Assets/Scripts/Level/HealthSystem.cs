using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    public event Action<int, int> OnHealthChange;

    void Awake()
    {
        currentHealth = maxHealth;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnHealthChange?.Invoke(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && gameObject.tag == "Player")
        {
            TakeDamage(20);
        }
        if (Input.GetKeyDown(KeyCode.E) && gameObject.tag == "Enemy")
        {
            TakeDamage(20);
        }
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
public void TakeDamage(int amount)
    {
        if (amount <= 0) return;
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        OnHealthChange?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0) Die();
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChange?.Invoke(currentHealth, maxHealth);
    }

    public void SetHealth(int value)
    {
        currentHealth = Mathf.Clamp(value, 0, maxHealth);
        OnHealthChange?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        // simple default: disable object. Replace with your own death logic.
        gameObject.SetActive(false);
    }
    
    
}
