using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RoadmanScript : MonoBehaviour
{
    public float speed = 5f; // Speed of the roadman
    public int health = 100; // Health of the roadman
    public int damage = 10; // Damage dealt by the roadman
    public Transform player; // Reference to the player transform
    public float attackRange = 2f; // Range within which the roadman can attack
    public float detectionRange = 10f; // Range within which the roadman can detect the player
    public float attackCooldown = 1f; // Cooldown time between attacks
    public float recoveryTime = 0.5f; // Time to recover after attack animation

    private Animator anim;
    private enum State { Idle, Chasing, Attacking, Recovering }

    private float cooldownTimer = Mathf.Infinity;
    private float recoveryTimer = 0f;

    private State currentState = State.Idle;
    private PlayerHP playerHP; // Reference to PlayerHP component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Get reference to PlayerHP component
        playerHP = player.GetComponent<PlayerHP>();
        if (playerHP == null)
        {
            Debug.LogError("PlayerHP component not found on player! Make sure your player has the PlayerHP script attached.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Update()
    {
        // Don't do anything if player is dead or reference is missing
        if (playerHP == null || !playerHP.IsAlive())
            return;

        float distance = Vector2.Distance(transform.position, player.position);
        cooldownTimer += Time.deltaTime;

        switch (currentState)
        {
            case State.Idle:
                anim.SetBool("isMoving", false);
                if (distance <= detectionRange)
                    currentState = State.Chasing;
                break;

            case State.Chasing:
                if (distance > detectionRange)
                {
                    currentState = State.Idle;
                    break;
                }

                if (distance <= attackRange)
                {
                    if (cooldownTimer >= attackCooldown)
                    {
                        currentState = State.Attacking;
                        StartAttack();
                    }
                    break;
                }

                MoveTowardPlayer();
                break;

            case State.Attacking:
                // Wait for OnAttackAnimationEnd to handle transition to Recovering
                break;

            case State.Recovering:
                recoveryTimer -= Time.deltaTime;
                if (recoveryTimer <= 0)
                {
                    currentState = State.Idle;
                }
                break;
        }
    }

    private void MoveTowardPlayer()
    {
        anim.SetBool("isMoving", true);
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Flip sprite
        if (direction.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void StartAttack()
    {
        // Stop moving during attack
        anim.SetBool("isMoving", false);

        // Trigger attack animation - make sure you have this trigger parameter in your Animator
        anim.SetTrigger("Attack");

        // Reset cooldown timer
        cooldownTimer = 0f;
    }

    private void PerformStabAttack()
    {
        // Check if player is still in range and alive when the attack connects
        if (playerHP == null || !playerHP.IsAlive()) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            // Deal damage to the player using PlayerHP component
            playerHP.TakeDamage(damage);
            Debug.Log("Roadman stabbed player for " + damage + " damage! Player HP: " + playerHP.GetCurrentHP());

            // Optional: Add knockback or other effects
            // ApplyKnockbackToPlayer();
        }
    }

    // This method should be called by an Animation Event at the moment the knife would hit
    public void OnAttackHitFrame()
    {
        // This would be called via Animation Event at the precise frame when the knife stabs forward
        PerformStabAttack();
    }

    // This method should be called by an Animation Event at the end of your attack animation
    public void OnAttackAnimationEnd()
    {
        if (currentState == State.Attacking)
        {
            currentState = State.Recovering;
            recoveryTimer = recoveryTime;
        }
    }

    // Optional: Method to apply knockback to player
    private void ApplyKnockbackToPlayer()
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 knockbackDirection = (player.position - transform.position).normalized;
            playerRb.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse);
        }
    }

    // Optional: Method to handle when the roadman takes damage
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        Debug.Log($"Roadman took {damageAmount} damage! Health: {health}");

        // Optional: Add a hit reaction
        if (health > 0)
        {
            // Play hit animation or flash effect
            StartCoroutine(HitReaction());
        }
        else
        {
            Die();
        }
    }

    private System.Collections.IEnumerator HitReaction()
    {
        // Optional: Visual feedback when hit
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
    }

    private void Die()
    {
        // Handle roadman death
        Debug.Log("Roadman died!");

        // Play death animation if you have one
        anim.SetTrigger("Die");

        // Disable collisions and AI
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        // Destroy after a delay to allow animation to play
        Destroy(gameObject, 2f);
    }
}