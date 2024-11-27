using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    public float speed;

    Rigidbody2D rb;

    

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
        if(rb.velocity.x < 0f && playerInput.x > 0f) { rb.velocity = rb.velocity * Vector2.left; }
        if(rb.velocity.x > 0f && playerInput.x < 0f) { rb.velocity = rb.velocity * Vector2.left; }

        rb.AddForce(new Vector2(playerInput.x, 0f) * speed * 100f * Time.deltaTime);

        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x < -maxSpeed)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
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
        if(Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), new Vector2(0.33f, 1f), 0f, Vector2.down))
        {
            return true;
        }
        else
        {
            return false;
        }
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
