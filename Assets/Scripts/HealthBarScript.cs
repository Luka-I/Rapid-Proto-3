using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [Header("Health Bar References")]
    public Slider healthSlider;
    public Image healthFill;
    public Text healthText;

    [Header("Color Settings")]
    public Color highHealthColor = Color.green;
    public Color mediumHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;

    [Header("Settings")]
    public bool showText = true;
    public bool changeColor = true;

    private PlayerHP playerHP;

    void Start()
    {
        // Find the PlayerHP component
        playerHP = Object.FindFirstObjectByType<PlayerHP>();

        if (playerHP == null)
        {
            Debug.LogError("PlayerHP not found in scene!");
            return;
        }

        // Set up the health bar
        if (healthSlider != null)
        {
            healthSlider.maxValue = playerHP.GetMaxHP();
            healthSlider.value = playerHP.GetCurrentHP();
        }

        // Update text if available
        UpdateHealthText();

        // Update color
        UpdateHealthBarColor();
    }

    void Update()
    {
        if (playerHP != null)
        {
            // Update health bar value
            if (healthSlider != null)
            {
                healthSlider.value = playerHP.GetCurrentHP();
            }

            // Update text and color
            UpdateHealthText();
            UpdateHealthBarColor();
        }
    }

    private void UpdateHealthText()
    {
        if (healthText != null && showText && playerHP != null)
        {
            healthText.text = $"{playerHP.GetCurrentHP()} / {playerHP.GetMaxHP()}";
        }
    }

    private void UpdateHealthBarColor()
    {
        if (healthFill != null && changeColor && playerHP != null)
        {
            float healthPercentage = playerHP.GetHealthPercentage();

            if (healthPercentage > 0.6f)
                healthFill.color = highHealthColor;
            else if (healthPercentage > 0.3f)
                healthFill.color = mediumHealthColor;
            else
                healthFill.color = lowHealthColor;
        }
    }
}
