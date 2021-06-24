using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float acceleration = 1000;


    [SerializeField]
    float dashMultiplier = 100;
    [SerializeField]
    float dashCooldown1 = 0.45f;
    [SerializeField]
    float dashCooldown2 = 0.8f;
    [SerializeField]
    float dashAngleMultiplier = 6;
    WaitForSeconds dashWFS1;
    WaitForSeconds dashWFS2;

    [SerializeField]
    float jumpStrength = 100;
    [SerializeField]
    float jumpCooldown = 0.2f;
    WaitForSeconds jumpWFS;

    [SerializeField]
    float rotateSpeed = 5.25f;

    [SerializeField]
    float runThreshold = 3;

    [SerializeField]
    float gravityRayLength = 0.4f;
    [SerializeField]
    float gravityMultiplier = 16;

    [SerializeField]
    KeyCode forwardKey = KeyCode.D;
    [SerializeField]
    KeyCode backwardKey = KeyCode.A;
    [SerializeField]
    KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField]
    KeyCode jumpKey = KeyCode.Space;

    Vector3 moveVector;
    Rigidbody rb;
    Animator anim;
    float originalDrag;

    bool forwardPressed = false;
    bool backwardPressed = false;
    bool dashPressed = false;
    bool jumpPressed = false;

    bool canDash = true;
    bool canJump = true;

    bool running = false;
    bool flipped = false;
    bool onGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        originalDrag = rb.drag;
        dashWFS1 = new WaitForSeconds(dashCooldown1);
        dashWFS2 = new WaitForSeconds(dashCooldown2);
        jumpWFS = new WaitForSeconds(jumpCooldown);
    }

    private void Update()
    {
        GetInput();
    }

    void ForceZ()
    {
        if (transform.position.z < 0 || transform.position.z > 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    void GetInput()
    {
        if (!forwardPressed) forwardPressed = Input.GetKey(forwardKey);
        if (!backwardPressed) backwardPressed = Input.GetKey(backwardKey);
        if (!dashPressed) dashPressed = Input.GetKeyDown(dashKey);
        if (!jumpPressed) jumpPressed = Input.GetKeyDown(jumpKey);
    }

    void FixedUpdate()
    {
        Move();
        forwardPressed = false;
        backwardPressed = false;
        dashPressed = false;
        jumpPressed = false;
    }

    void Move()
    {
        moveVector = Vector3.zero;
        Gravity();

        if (forwardPressed)
        {
            forwardPressed = false;
            moveVector += Vector3.right * acceleration;
            Rotate(90);
        }
        else if (backwardPressed)
        {
            backwardPressed = false;
            moveVector += -(Vector3.right * acceleration);
            Rotate(-90);
        }

        if (dashPressed && canDash && Mathf.Abs(moveVector.sqrMagnitude) >= 0 && onGround)
        {
            anim.SetTrigger("Dash");
            dashPressed = false;
            moveVector += (moveVector + (Vector3.up * dashAngleMultiplier)) * dashMultiplier;
            StartCoroutine(nameof(DashEnd));
        }
        else if (jumpPressed && canJump)
        {
            anim.SetTrigger("Jump");
            canJump = false;
            jumpPressed = false;
            moveVector.y += jumpStrength;
            StartCoroutine(nameof(JumpEnd));
        }
        rb.AddForce(moveVector, ForceMode.Force);
        RunAnimation();
    }

    private void Gravity()
    {
        if (!Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, gravityRayLength))
        {
            onGround = false;
            moveVector += (Vector3.down * gravityMultiplier);
        }
        else
        {
            onGround = true;
        }
    }

    void RunAnimation()
    {
        if (Mathf.Abs(moveVector.x) >= runThreshold && !running && canJump)
        {
            anim.ResetTrigger("StopRun");
            anim.SetTrigger("Run");
            running = true;
        }
        else if (Mathf.Approximately(Mathf.Abs(moveVector.x), 0) && running)
        {
            anim.SetTrigger("StopRun");
            running = false;
        }
    }

    void Rotate(float dir)
    {
        float yRot = transform.rotation.eulerAngles.y;
        if (yRot >= -15 && yRot <= 15 && !flipped) // Flip X axis when rotating so animation is rotated too
        {
            flipped = true;
            Vector3 original = transform.localScale;
            transform.localScale = new Vector3(-original.x, original.y, original.z);
        }
        else if (yRot < -15 || yRot > 15)
        {
            flipped = false;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, dir, 0), rotateSpeed);
    }

    IEnumerator DashEnd()
    {
        canDash = false;
        yield return dashWFS1;
        running = false;
        yield return dashWFS2;
        anim.ResetTrigger("StopRun");
        canDash = true;
    }

    IEnumerator JumpEnd()
    {
        StartCoroutine(nameof(HitGround));
        yield return jumpWFS;
        anim.ResetTrigger("StopRun");
        running = false;
        canJump = true;
    }

    IEnumerator HitGround()
    {
        yield return new WaitForSeconds(0.1f);
        while (!Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, gravityRayLength))
        {
            yield return null;
        }
        anim.SetTrigger("StopJump");
    }
}
