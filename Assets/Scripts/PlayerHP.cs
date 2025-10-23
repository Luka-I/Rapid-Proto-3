using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHP = 100;
    public int currentHP;
    public bool isInvincible = false;
    public float invincibilityDuration = 1f;

    [Header("Events")]
    public UnityEvent OnDamageTaken;
    public UnityEvent OnDeath;

    [Header("Visual Feedback")]
    public SpriteRenderer playerSprite;
    public Color damageColor = Color.red;
    public float flashDuration = 0.1f;
    private Color originalColor;

    private bool isDead = false;
    private float invincibilityTimer = 0f;

    public string SceneToLoad;

    void Start()
    {
        currentHP = maxHP;
        if (playerSprite != null)
        {
            originalColor = playerSprite.color;
        }
    }

    void Update()
    {
        // Handle invincibility timer
        if (isInvincible && invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                if (playerSprite != null)
                {
                    playerSprite.color = originalColor;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead || isInvincible) return;

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        OnDamageTaken?.Invoke();

        // Visual feedback
        if (playerSprite != null)
        {
            StartCoroutine(DamageFlash());
        }

        // Check for death
        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            // Start invincibility period
            StartInvincibility();
        }
    }

    public void SetHealth(int newHealth)
    {
        currentHP = Mathf.Clamp(newHealth, 0, maxHP);

        if (currentHP <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
        SceneManager.LoadScene(SceneToLoad);

        // You can add death logic here, like:
        // - Play death animation
        // - Disable player controls
        // - Show game over screen
        Debug.Log("Player Died!");
    }

    private void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    private System.Collections.IEnumerator DamageFlash()
    {
        if (playerSprite != null)
        {
            playerSprite.color = damageColor;
            yield return new WaitForSeconds(flashDuration);

            if (!isInvincible) // Only revert color if not still invincible
            {
                playerSprite.color = originalColor;
            }
        }
    }

    // Public methods for external access
    public int GetCurrentHP()
    {
        return currentHP;
    }

    public int GetMaxHP()
    {
        return maxHP;
    }

    public float GetHealthPercentage()
    {
        return (float)currentHP / maxHP;
    }

    public bool IsAlive()
    {
        return !isDead;
    }

    // Automatically handle collision with enemy projectiles or enemies
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // You can modify this to get damage amount from the enemy
            TakeDamage(10);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // You can modify this to get damage amount from the enemy
            TakeDamage(10);
        }
    }
}
