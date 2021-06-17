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

    bool forward = false;
    bool backward = false;
    bool dash = false;
    bool jump = false;

    bool canDash = true;
    bool canJump = true;

    bool running = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        originalDrag = rb.drag;
        dashWFS1 = new WaitForSeconds(dashCooldown1);
        dashWFS2 = new WaitForSeconds(dashCooldown2);
        jumpWFS = new WaitForSeconds(jumpCooldown);
        StartCoroutine(nameof(ApplyGravity));
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
            moveVector += (moveVector + (Vector3.up * dashAngleMultiplier)) * dashMultiplier;
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
        RunAnimation();
    }

    void RunAnimation()
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
        if (yRot > -15 && yRot < 15) // Flip X axis when rotating so animation is rotated too
        {
            Vector3 original = transform.localScale;
            transform.localScale = new Vector3(-original.x, original.y, original.z);
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
        yield return jumpWFS;
        anim.SetTrigger("StopJump");
        anim.ResetTrigger("StopRun");
        running = false;
    }

    IEnumerator ApplyGravity()
    {
        while (true)
        {
            if (!Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, gravityRayLength))
            {
                canJump = false;
                rb.AddForce(Vector3.down * gravityMultiplier);
            }
            else
            {
                canJump = true;
            }
            yield return null;
        }
    }
}
