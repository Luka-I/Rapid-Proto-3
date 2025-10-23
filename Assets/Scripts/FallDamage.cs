using UnityEngine;

public class FallDamage2D : MonoBehaviour
{
    public float fallDamageThreshold = 5f;
    public float damageMultiplier = 10f;
    public int playerHealth = 100;

    private bool isGrounded = false;
    private float fallStartY;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGrounded = false;
        fallStartY = transform.position.y;
    }

    void Update()
    {

        bool currentlyGrounded = IsGrounded();

        if (!isGrounded && currentlyGrounded)
        {

            float fallDistance = fallStartY - transform.position.y;

            if (fallDistance > fallDamageThreshold)
            {
                ApplyFallDamage(fallDistance);
            }
        }

        if (!currentlyGrounded)
        {
            
            fallStartY = transform.position.y;
        }

        isGrounded = currentlyGrounded;
    }

    bool IsGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, extraHeight);

        return hit.collider != null;
    }

    void ApplyFallDamage(float fallDistance)
    {
        float damage = (fallDistance - fallDamageThreshold) * damageMultiplier;
        playerHealth -= Mathf.RoundToInt(damage);
        Debug.Log($"Took {damage} fall damage! Health now: {playerHealth}");

        if (playerHealth <= 0)
        {
            Debug.Log("Player died from fall damage!");
            // Add death logic here
        }
    }
}
