using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    float initialMaxSpeed;
    public float speed;

    Rigidbody2D rb;
    bool grounded;

    public float apexHeight;
    public float apexTime;
    float apexTimer;

    public float terminalVelocity;

    public float coyoteTime;
    float coyoteTimer;

    public float dashCooldown;
    float dashCooldownTimer;
    public float dashVelocityThreshold;

    public float slamReboundTime;
    float slamReboundTimer;
    bool slamming;
    float slammingTimer;

    public enum FacingDirection
    {
        left, right
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialMaxSpeed = maxSpeed;
        slamming = false;
    }

    void Update()
    {

        Vector2 playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        MovementUpdate(playerInput);

        InputManager();
        dashCooldownTimer -= Time.deltaTime;
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        if(maxSpeed > initialMaxSpeed)
        {
            maxSpeed = maxSpeed - Time.deltaTime * 5f;
            if (maxSpeed < initialMaxSpeed) { maxSpeed = initialMaxSpeed; }
        }

        //ground movement
        if(rb.velocity.x < 0f && playerInput.x > 0f) { rb.velocity = new Vector2(rb.velocity.x / -2f, rb.velocity.y); }
        if(rb.velocity.x > 0f && playerInput.x < 0f) { rb.velocity = new Vector2(rb.velocity.x / -2f, rb.velocity.y); }

        rb.AddForce(new Vector2(playerInput.x, 0f) * speed * 100f * Time.deltaTime);

        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x < -maxSpeed)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }

        //air movement
        coyoteTimer -= Time.deltaTime;

        if (playerInput.y > 0f && (IsGrounded() || coyoteTimer > 0f))
        {
            coyoteTimer = 0f;
            rb.gravityScale = 0f;

            if(slamReboundTimer > 0f)
            {
                rb.AddForce(new Vector2(0f, apexHeight * (1 + slammingTimer)));
                slamReboundTimer = 0f;
                slammingTimer = 0f;
            }
            else
            {
                rb.AddForce(new Vector2(0f, apexHeight));
            }
            apexTimer = 0f;
        }

        apexTimer += Time.deltaTime;

        if(apexTimer > apexTime && !IsGrounded()) { rb.gravityScale = 1f; }

        if (rb.velocity.y < -terminalVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, -terminalVelocity);
        }

        //Slamming
        slamReboundTimer -= Time.deltaTime;

        if(playerInput.y < 0f && !IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, -terminalVelocity);
            slamming = true;
            slammingTimer = 0f;
        }
        
        if(slamming && !IsGrounded())
        {
            slammingTimer += Time.deltaTime;
        }

        if(slamming && IsGrounded())
        {
            slamming = false;
            slamReboundTimer = slamReboundTime;
        }
    }

    public void InputManager()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(dashCooldownTimer <= 0)
            {

                Dash();
            }
        }
    }

    void Dash()
    {
        maxSpeed = maxSpeed * 2;

        if(rb.velocity.magnitude > dashVelocityThreshold)
        {
            rb.velocity = rb.velocity * 2f;
        }
        else
        {
            rb.velocity = rb.velocity * dashVelocityThreshold;
        }
        
        dashCooldownTimer = dashCooldown;
    }

    public bool IsWalking()
    {
        if(rb.velocity.x == 0f)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }
    public bool IsGrounded()
    {
        if(grounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        grounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        grounded = false;
        coyoteTimer = coyoteTime;
    }

    public FacingDirection GetFacingDirection()
    {
        if(rb.velocity.x > 0f)
        {
            return FacingDirection.right;
        }
        else if (rb.velocity.x < 0f)
        {
            return FacingDirection.left;
        }
        else
        {
            return FacingDirection.right;
        }
        
    }

    
}
