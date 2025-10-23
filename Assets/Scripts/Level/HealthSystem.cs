using UnityEngine;


/// <summary>
/// Health System of Entities (Enemies, Players, and other things that can be "Destroyed")
/// </summary>
public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;                                     // Base Max health of an entity
    public int currentHealth;                                       // Current health of an entity

    private bool healthChanged = false;

    void Awake()
    {
        currentHealth = maxHealth;                                  // Sets the current health
    }


    // Function to cause damage to gameobjects with the health system
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        healthChanged = true;
        if (currentHealth < 0)
        {
            currentHealth = 0;
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

    // Sets the Current Health
    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
    }

    // Gets the max health
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    // Sets the Max health
    public void SetMaxHealth(int health)
    {
        maxHealth = health;
    }

    // Gets the check if the health is changed for the player
    public bool IsHealthChanged()
    {
        return healthChanged;
    }
}
