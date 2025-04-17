// using UnityEngine;
// using UnityEngine.UI;

// public class HealthBar : MonoBehaviour
// {
//     public Image healthFillImage;

//     public void SetHealth(float healthNormalized)
//     {
//         healthFillImage.fillAmount = Mathf.Clamp01(healthNormalized);
//     }
// }
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthFillImage;
    public TextMeshProUGUI coinText;

    private int coinCount = 0;

    private float currentHealth = 1f;

    void Update()
    {
        // Auto simulate health value between 1 and 0
        

        healthFillImage.fillAmount = Mathf.Clamp01(currentHealth);

    }

    public void SetHealth(float healthNormalized)
    {
        currentHealth = Mathf.Clamp01(healthNormalized);
        healthFillImage.fillAmount = currentHealth;
    }

    void Start()
    {
        coinText.text = "x " + coinCount.ToString();
    }

    // void Update()
    // {
    //     // Simulate coin collection
        
    // }

    // Optional method to update coins from other scripts
    public void SetCoins(int coins)
    {
        coinCount = coins;
        coinText.text = "x " + coinCount.ToString();
    }
}
