using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private float jumpForce = 7.0f;

    [SerializeField]
    private float spinDuration = 0.5f; // Time to complete the 360 spin

    private Rigidbody2D rb;
    private bool isGrounded = true;

    private bool isSpinning = false;
    private float spinTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Move forward continuously
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        // Check jump input via keyboard or gamepad directly
        if (isGrounded && (Keyboard.current.spaceKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)))
        {
            Jump();
        }

        // Handle spinning while in air
        if (isSpinning)
        {
            spinTimer += Time.deltaTime;
            float rotationProgress = spinTimer / spinDuration;

            if (rotationProgress < 1f)
            {
                // Rotate based on progress (360 degrees)
                float angle = 360f * rotationProgress;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                // Spin complete - reset rotation and stop spinning
                transform.rotation = Quaternion.identity;
                isSpinning = false;
                spinTimer = 0f;
            }
        }
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isGrounded = false;

        // Start spinning
        isSpinning = true;
        spinTimer = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            // Optional: Reset rotation when you land, in case spin is interrupted
            transform.rotation = Quaternion.identity;
            isSpinning = false;
            spinTimer = 0f;
        }
    }
}
