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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
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

        // Perform the actual attack logic
        PerformStabAttack();
    }

    private void PerformStabAttack()
    {
        // Check if player is still in range when the attack connects
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            // Here you would typically call a method on the player to take damage
            /*PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }*/

            // Optional: Add knockback or other effects
            Debug.Log("Roadman stabbed player for " + damage + " damage!");
        }
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

    // Optional: You can call this at the moment the knife would hit in the animation
    public void OnAttackHitFrame()
    {
        // This would be called via Animation Event at the precise frame when the knife stabs forward
        PerformStabAttack();
    }
}