using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    public int health;
    public int treasure;
    public HealthBar healthBar;

    private int maxHealth = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 100;
        treasure = 0;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateUI()
    {
        if (healthBar != null)
        {
            // Convert raw health to normalized value (0-1)
            float normalizedHealth = (float)health / maxHealth;
            healthBar.SetHealth(normalizedHealth);
            healthBar.SetCoins(treasure);
        }
    }
    
    // Called when health changes
    public void SetHealth(int newHealth)
    {
        health = newHealth;
        UpdateUI();
    }
    
    // Called when treasure changes
    public void AddTreasure(int amount)
    {
        treasure += amount;
        UpdateUI();
    }
}
