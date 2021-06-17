using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float acceleration = 1000;


    [SerializeField]
    float dashMultiplier = 100;
    [SerializeField]
    float dashDrag = 4;
    [SerializeField]
    float dashGravityMultiplier = 1.75f;
    [SerializeField]
    float dashCooldown = 1.4f;
    WaitForSeconds dashWFS;

    [SerializeField]
    float jumpStrength = 100;
    [SerializeField]
    float jumpGravityMultiplier = 1.75f;
    [SerializeField]
    float jumpFirstCooldown = 0.2f;
    WaitForSeconds jumpFirstWFS;

    [SerializeField]
    float rotateSpeed = 5.25f;

    [SerializeField]
    KeyCode forwardKey = KeyCode.D;
    [SerializeField]
    KeyCode backwardKey = KeyCode.A;
    [SerializeField]
    KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField]
    KeyCode jumpKey = KeyCode.Space;

    [SerializeField]
    float runThreshold = 3;

    Vector3 moveVector;
    Rigidbody rb;
    Animator anim;
    float originalDrag;

    bool forward = false;
    bool backward = false;
    bool dash = false;
    bool jump = false;

    bool canDash = true;
    bool canJump = true;

    bool running = false;

    bool isColliding;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        originalDrag = rb.drag;
        dashWFS = new WaitForSeconds(dashCooldown);
        jumpFirstWFS = new WaitForSeconds(jumpFirstCooldown);
    }

    private void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (!forward) forward = Input.GetKey(forwardKey);
        if (!backward) backward = Input.GetKey(backwardKey);
        if (!dash) dash = Input.GetKeyDown(dashKey);
        if (!jump) jump = Input.GetKeyDown(jumpKey);
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        moveVector = Vector3.zero;
        if (forward)
        {
            forward = false;
            moveVector += Vector3.right * acceleration;
            Rotate(90);
        }
        if (backward)
        {
            backward = false;
            moveVector += -(Vector3.right * acceleration);
            Rotate(-90);
        }
        if (dash && canDash && Mathf.Abs(moveVector.sqrMagnitude) >= 0)
        {
            anim.SetTrigger("Dash");
            dash = false;
            moveVector *= dashMultiplier;
            StartCoroutine(nameof(DashEnd));
        }
        if (jump && canJump)
        {
            anim.SetTrigger("Jump");
            jump = false;
            moveVector.y += jumpStrength;
            StartCoroutine(nameof(JumpEnd));
        }
        rb.AddForce(moveVector, ForceMode.Force);
        RunCheck();
    }

    void RunCheck()
    {
        if (Mathf.Abs(moveVector.x) >= runThreshold && !running)
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
        if (yRot > -1 && yRot < 1) // Flip X axis when rotating so animation is rotated too
        {
            Vector3 original = transform.localScale;
            transform.localScale = new Vector3(-original.x, original.y, original.z);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, dir, 0), rotateSpeed);
    }

    IEnumerator DashEnd()
    {
        canDash = false;
        rb.drag = dashDrag;
        StartCoroutine(nameof(DashGravity));
        yield return dashWFS;
        canDash = true;
        anim.ResetTrigger("StopRun");
        running = false;
    }

    IEnumerator DashGravity()
    {
        while (!isColliding)
        {
            rb.AddForce(Vector3.down * dashGravityMultiplier);
            yield return null;
        }
        rb.drag = originalDrag;
    }

    IEnumerator JumpEnd()
    {
        canJump = false;
        yield return jumpFirstWFS;
        while (!isColliding)
        {
            rb.AddForce(Vector3.down * jumpGravityMultiplier);
            yield return null;
        }
        canJump = true;
        anim.SetTrigger("StopJump");
        anim.ResetTrigger("StopRun");
        running = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        isColliding = true;
    }

    private void OnCollisionExit(Collision other)
    {
        isColliding = false;
    }
}
