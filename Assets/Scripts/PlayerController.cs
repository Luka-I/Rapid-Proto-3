using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 7.0f;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Read movement input
        float moveInput = 0f;

        if (Keyboard.current.aKey.isPressed)
            moveInput = -1f;
        else if (Keyboard.current.dKey.isPressed)
            moveInput = 1f;
        else if (Gamepad.current != null)
            moveInput = Gamepad.current.leftStick.x.ReadValue();

        // Apply horizontal movement (✅ fixed here)
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Flip only the sprite
        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;

        // Jump (space or gamepad A button)
        if (isGrounded && (Keyboard.current.spaceKey.wasPressedThisFrame ||
                           (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}
