using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    [Header("Punch Settings")]
    public float punchRange = 0.5f;
    public int punchDamage = 20;

    [Header("Kick Settings")]
    public float kickRange = 0.7f;
    public int kickDamage = 30;

    [Header("Attack Timing")]
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Punch();
            }
            else if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Kick();
            }
        }
    }

    private void Punch()
    {
        animator.SetTrigger("Punch");
        nextAttackTime = Time.time + 1f / attackRate;
    }

    private void Kick()
    {
        animator.SetTrigger("Kick");
        nextAttackTime = Time.time + 1f / attackRate;
    }

    public void HitPunch()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, punchRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            RoadmanScript roadman = enemy.GetComponent<RoadmanScript>();
            if (roadman != null)
            {
                roadman.TakeDamage(punchDamage);
                Debug.Log($"Punched {enemy.name} for {punchDamage} damage!");
            }
        }
    }

    public void HitKick()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, kickRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            RoadmanScript roadman = enemy.GetComponent<RoadmanScript>();
            if (roadman != null)
            {
                roadman.TakeDamage(kickDamage);
                Debug.Log($"Kicked {enemy.name} for {kickDamage} damage!");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, punchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, kickRange);
    }
}