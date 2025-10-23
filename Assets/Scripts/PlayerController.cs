using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private SpriteRenderer spriteRenderer;
    private PlayerAttack playerAttack;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleActions();
    }

    private void HandleMovement()
    {
        float moveInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) moveInput = -1f;
            else if (Keyboard.current.dKey.isPressed) moveInput = 1f;
        }

        if (Gamepad.all.Count > 0)
        {
            var pad = Gamepad.all[0];
            moveInput = pad.leftStick.x.ReadValue();
        }

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Store previous flip state to detect changes
        bool wasFlipped = spriteRenderer.flipX;

        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        // If flip state changed, update attack point position
        if (wasFlipped != spriteRenderer.flipX && playerAttack != null && playerAttack.attackPoint != null)
        {
            FlipAttackPoint();
        }
    }

    private void FlipAttackPoint()
    {
        if (playerAttack.attackPoint != null)
        {
            // Flip the attack point's local position on the X-axis
            Vector3 currentPos = playerAttack.attackPoint.localPosition;
            playerAttack.attackPoint.localPosition = new Vector3(-currentPos.x, currentPos.y, currentPos.z);
        }
    }

    private void HandleJump()
    {
        if (!isGrounded) return;

        bool jumpKeyboard = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool jumpGamepad = false;

        foreach (var pad in Gamepad.all)
        {
            if (pad.buttonSouth.wasPressedThisFrame) jumpGamepad = true;
        }

        if (jumpKeyboard || jumpGamepad)
            Jump();
    }

    private void HandleActions()
    {
        if (playerAttack == null) return;

        bool punch = false;
        bool kick = false;

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

        if (punch) playerAttack.Punch();
        if (kick) playerAttack.Kick();
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
