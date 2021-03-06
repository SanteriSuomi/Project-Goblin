using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public LayerMask groundLayerMask;
    public Transform Aim;
    public float movementSpeed;
    public float jumpVelocity;
    private float fallMultiplier = 4.0f;
    private float lowJumpMultiplier = 3.0f;

    RaycastHit hit;

    public Animator anim;
    Vector2 moveVector;
    PlayerBow bow;
    PlayerMelee melee;
    AudioManager audioManager;

    public float moveVelocity;
    float rotateSpeed = 20f;

    public string facingDir = "Right";

    bool collision;
    bool grounded;
    bool jumping = false;
    bool flipped = false;

    float colliderX;
    float colliderY;

    public Rigidbody rb;
    public LayerMask groundLayers;

    float angle;
    float mx;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bow = GetComponent<PlayerBow>();
        melee = GetComponent<PlayerMelee>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        anim.SetBool("Grounded", IsGrounded());
        anim.SetFloat("velocityY", rb.velocity.y);
        Jump();
        Turn();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded() && !bow.IsCharging && !jumping && !melee.IsMelee)
        {
            anim.SetTrigger("Jump");
            audioManager.Play("Jump");
            jumping = true;
            rb.velocity = Vector3.up * jumpVelocity;
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, -transform.up, out rayHit, 1.5f))
        {
            anim.SetTrigger("StopJump");
            jumping = false;
        }
    }

    void Rotate(float dir)
    {
        float yRot = transform.rotation.eulerAngles.y;
        if (yRot >= -15 && yRot <= 15 && !flipped) // Flip X axis when rotating so animation is rotated too
        {
            flipped = true;
            Vector3 original = transform.localScale;
            transform.localScale = new Vector3(original.x, original.y, original.z);
        }
        else if (yRot < -15 || yRot > 15)
        {
            flipped = false;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, dir, 0), rotateSpeed);
    }

    public void Move()
    {
        if (!bow.IsCharging && !melee.IsMelee)
        {
            mx = Input.GetAxisRaw("Horizontal") * movementSpeed;
            Vector2 movement = new Vector2(mx, rb.velocity.y);
            rb.velocity = movement;
        }
        else if (bow.IsCharging)
        {
            mx = 0;
        }

        if (Mathf.Abs(mx) > 0.05f)
        {
            anim.SetBool("Running", true);
        }
        else if (Mathf.Abs(mx) == 0f)
        {
            anim.SetBool("Running", false);
        }
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, out RaycastHit rayHit, 0.15f, groundLayers, QueryTriggerInteraction.Ignore);
    }

    public void Turn()
    {
        anim.SetFloat("MovementSpeed", moveVelocity);
        if (bow.IsCharging || melee.IsMelee)
        {
            if (bow.mousePoint.x < transform.position.x && facingDir != "Left")
            {
                Flip();
                facingDir = "Left";
            }
            else if (bow.mousePoint.x > transform.position.x && facingDir != "Right")
            {
                Flip();
                facingDir = "Right";
            }
        }

        if (mx > 0 && facingDir != "Right" && (!bow.IsCharging || !melee.IsMelee))
        {
            Flip();
            facingDir = "Right";
        }
        else if (mx < 0 && facingDir != "Left" && (!bow.IsCharging || !melee.IsMelee))
        {
            Flip();
            facingDir = "Left";
        }

        if (facingDir == "Left")
        {
            moveVelocity = -rb.velocity.x;
        }
        else
        {
            moveVelocity = rb.velocity.x;
        }
    }

    private void Flip()
    {
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.z *= -1;
        Aim.transform.localScale = theScale;
        transform.localScale = theScale;
    }

    public void LeftStepSound() {
        audioManager.Play("StepLeft");
    }

    public void RightStepSound() {
        audioManager.Play("StepRight");
    }

}