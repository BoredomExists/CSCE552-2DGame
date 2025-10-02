using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth;
    private int currentHealth;


    public Text healthText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        healthText.text = maxHealth.ToString();
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

    private void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        healthText.text = currentHealth.ToString();
    }
    
    
}
