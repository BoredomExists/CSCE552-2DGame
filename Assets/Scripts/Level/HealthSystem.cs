using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    private bool healthChanged = false;
     
    void Awake()
    {
        currentHealth = maxHealth;
    }


    // Function to cause damage to gameobjects with the health system
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        healthChanged = true;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Function to heal damage to gameobjects with the health system
    public void Heal(int amount)
    {
        currentHealth += amount;
        healthChanged = true;
        if (currentHealth > maxHealth)
        { currentHealth = maxHealth; }
    }

    // Gets the current health
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    // Gets the max health
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    
    // Gets the check if the health is changed for the player
    public bool IsHealthChanged()
    {
        return healthChanged;
    }
}
