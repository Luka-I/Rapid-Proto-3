using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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
        if (Time.time < nextAttackTime) return;

        bool punch = false;
        bool kick = false;

        if (Keyboard.current != null)
        {
            punch |= Keyboard.current.leftCtrlKey.wasPressedThisFrame;
            kick |= Keyboard.current.leftAltKey.wasPressedThisFrame;
        }

        if (Mouse.current != null)
        {
            punch |= Mouse.current.leftButton.wasPressedThisFrame;
            kick |= Mouse.current.rightButton.wasPressedThisFrame;
        }

        foreach (var pad in Gamepad.all)
        {
            punch |= pad.buttonEast.wasPressedThisFrame;
            kick |= pad.buttonNorth.wasPressedThisFrame;
        }

        foreach (var js in Joystick.all)
        {
            foreach (var control in js.allControls)
            {
                if (control is ButtonControl btn && btn.wasPressedThisFrame)
                {
                    string n = btn.name.ToLower();
                    if (n.Contains("1") || n.Contains("b")) punch = true;
                    else if (n.Contains("3") || n.Contains("y")) kick = true;
                }
            }
        }

        if (punch) Punch();
        if (kick) Kick();
    }

    public void Punch()
    {
        animator.SetTrigger("Punch");
        nextAttackTime = Time.time + 1f / attackRate;
    }

    public void Kick()
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
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, punchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, kickRange);
    }
}
