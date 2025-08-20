using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    [Header("Better Jumping")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float coyoteTime = 0.1f; // Time player can still jump after leaving ground
    public int maxJumps = 2; // Double jump capability
    public float maxFallSpeed = 15f; // Maximum fall speed to prevent getting stuck

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Input Keys")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashKey = KeyCode.LeftShift;

    [Header("Debug Info")]
    [SerializeField] private int currentJumpsRemaining;
    [SerializeField] private bool isCurrentlyGrounded;

    // Private variables
    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashTimeLeft = 0f;
    private float dashCooldownTimer = 0f;
    private float coyoteTimeCounter = 0f;
    private int jumpsRemaining;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        // Get horizontal input
        moveInput = Input.GetAxisRaw("Horizontal");

        // Handle jumping
        HandleJump();

        // Handle dashing
        HandleDash();

        // Handle better jumping physics
        HandleBetterJump();

        // Update coyote time
        UpdateCoyoteTime();

        // Handle character flipping
        HandleCharacterFlip();

        // Update debug info
        currentJumpsRemaining = jumpsRemaining;
        isCurrentlyGrounded = isGrounded;
    }

    void HandleJump()
    {
        // Reset jumps when grounded
        if (isGrounded)
        {
            jumpsRemaining = maxJumps;
            coyoteTimeCounter = coyoteTime;
        }

        // Jump input
        if (Input.GetKeyDown(jumpKey))
        {
            if (isGrounded || coyoteTimeCounter > 0)
            {
                // Ground jump or coyote time jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                coyoteTimeCounter = 0f;
                jumpsRemaining = maxJumps - 1; // Use one jump, have one left for air jump
            }
            else if (jumpsRemaining > 0)
            {
                // Air jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpsRemaining--;
            }
        }
    }

    void HandleDash()
    {
        // Dash cooldown
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Dash input
        if (Input.GetKeyDown(dashKey) && canDash && dashCooldownTimer <= 0 && !isDashing)
        {
            StartDash();
        }

        // Handle dash duration
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                EndDash();
            }
        }
    }

    void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
        dashCooldownTimer = dashCooldown;

        // Apply dash force in the direction player is facing
        float dashDirection = facingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dashDirection * dashForce, 0f);

        // Optional: Add visual effects here
        // StartCoroutine(DashEffect());
    }

    void EndDash()
    {
        isDashing = false;
        canDash = true;
        // Don't immediately change velocity - let FixedUpdate handle it naturally
        // This prevents sudden velocity changes that can cause balance issues
    }

    void HandleBetterJump()
    {
        // Better falling
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            // Clamp fall speed to prevent getting stuck
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        // Better low jump (when jump button is released early)
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(jumpKey))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void UpdateCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    void HandleCharacterFlip()
    {
        // Flip character based on movement direction
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void FixedUpdate()
    {
        // Apply horizontal movement (only when not dashing)
        if (!isDashing)
        {
            // Preserve current horizontal velocity if no input, otherwise set to input speed
            float targetVelocityX = moveInput * moveSpeed;
            if (moveInput == 0)
            {
                // Gradually slow down when no input (adds friction)
                targetVelocityX = Mathf.Lerp(rb.linearVelocity.x, 0, 0.1f);
            }
            
            rb.linearVelocity = new Vector2(targetVelocityX, rb.linearVelocity.y);
        }

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    // Optional: Add this method if you want to create dash effects
    /*
    IEnumerator DashEffect()
    {
        // Add trail renderer or other visual effects
        yield return new WaitForSeconds(dashDuration);
    }
    */
}
