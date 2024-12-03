using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    public float speed;

    Rigidbody2D rb;
    bool grounded;

    public float apexHeight;
    public float apexTime;
    float apexTimer;

    public float terminalVelocity;

    public float coyoteTime;
    float coyoteTimer;

    public enum FacingDirection
    {
        left, right
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        Vector2 playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
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

            rb.AddForce(new Vector2(0f, apexHeight));
            apexTimer = 0f;
        }

        apexTimer += Time.deltaTime;

        if(apexTimer > apexTime && !IsGrounded()) { rb.gravityScale = 1f; }

        if (rb.velocity.y < -terminalVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, -terminalVelocity);
        }
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
