using UnityEngine;

public class movement : MonoBehaviour
{
    public LayerMask groundLayerMask;
    public float movementSpeed;
    public float jumpVelocity;
    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2f;

    float dashMultiplier = 15f;
    float dashCooldown1 = 0.45f;
    float dashCooldown2 = 0.8f;
    float dashAngleMultiplier = 2;
    WaitForSeconds dashWFS1;
    WaitForSeconds dashWFS2;

    RaycastHit hit;

    public CapsuleCollider capCollider;
    public Animator anim;
    Vector2 moveVector;
    PlayerBow bow;

    public float moveVelocity;
    public float rotateSpeed = 2f;
    string facingDir = "Right";

    bool collision;
    bool grounded;
    bool running = false;
    bool jumping = false;
    bool falling = false;
    bool flipped = false;
    bool dashPressed = false;
    bool canDash = true;

    float colliderX;
    float colliderY;

    public string turned = "right";

    public Rigidbody rb;

    float angle;
    float mx;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capCollider = GetComponent<CapsuleCollider>();
        colliderY = capCollider.bounds.size.y;
        colliderX = capCollider.bounds.size.x;
        bow = GetComponent<PlayerBow>();
    }

    private void Update()
    {
        mx = Input.GetAxisRaw("Horizontal");
        anim.SetBool("Grounded", IsGrounded());
        anim.SetFloat("velocityY", rb.velocity.y);
        Jump();
        Turn();
        //Dash();

        if (!bow.IsCharging)
        {
            Vector2 movement = new Vector2(mx * movementSpeed, rb.velocity.y);
            rb.velocity = movement;
        }

        if (Mathf.Abs(mx) > 0.1f && !running)
        {
            anim.ResetTrigger("StopRun");
            anim.SetTrigger("Run");
            running = true;
        }
        else if (Mathf.Approximately(Mathf.Abs(mx), 0) && running)
        {
            anim.SetTrigger("StopRun");
            running = false;
        }
    }

    void Jump()
    {
        if (Input.GetButton("Jump") && IsGrounded() && !bow.IsCharging)
        {
            running = false;
            anim.SetTrigger("Jump");
            jumping = true;
            GetComponent<Rigidbody>().velocity = Vector3.up * jumpVelocity;
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, 0.11f))
        {
            anim.SetTrigger("StopJump");
        }
    }

    public bool IsFalling()
    {
        if (rb.velocity.y < -1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        collision = true;
    }

    private void OnCollisionExit(Collision other)
    {
        collision = false;
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

    public bool IsGrounded()
    {
        bool groundCheck = Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, 0.11f, groundLayerMask);
        return groundCheck || collision;
        // if (groundCheck)
        // {
        //     //Debug.Log("Hit : " + rayHit.collider.name);
        // }
        // else
        // {
        //     if (collision)
        //     {
        //         return true;
        //     }
        // }
        // return groundCheck;
    }

    public void Turn()
    {
        anim.SetFloat("MovementSpeed", moveVelocity);
        if (bow.mousePoint.x < transform.position.x)
        {
            Rotate(-90);
            facingDir = "Left";
        }
        else
        {
            Rotate(90);
            facingDir = "Right";
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

    /*public void Dash() {
		if(Input.GetKeyDown(KeyCode.LeftShift)) {
			dashPressed = true;
		}
		if (dashPressed && canDash && IsGrounded()) {
            anim.SetTrigger("Dash");
            moveVector = new Vector2(mx * dashMultiplier, rb.velocity.y);
            rb.velocity = moveVector;
            dashPressed = false;
            StartCoroutine(nameof(DashEnd));
        }
	}

	IEnumerator DashEnd() {
        canDash = false;
        yield return dashWFS1;
        running = false;
        yield return dashWFS2;
        anim.ResetTrigger("StopRun");
        canDash = true;
    }*/
}