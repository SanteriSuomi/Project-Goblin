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
            moveVector += Vector3.forward * acceleration;
            Rotate(0);
        }
        if (backward)
        {

            backward = false;
            moveVector += -(Vector3.forward * acceleration);
            Rotate(180);
        }
        if (dash && canDash && Mathf.Abs(moveVector.sqrMagnitude) >= 0)
        {
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
    }

    void Rotate(float dir)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, dir, 0), rotateSpeed);
    }

    IEnumerator DashEnd()
    {
        canDash = false;
        rb.drag = dashDrag;
        StartCoroutine(nameof(DashGravity));
        yield return dashWFS;
        float time = Time.deltaTime;
        rb.drag = originalDrag;
        canDash = true;
    }

    IEnumerator DashGravity()
    {
        while (!isColliding)
        {
            rb.AddForce(Vector3.down * dashGravityMultiplier);
            yield return null;
        }
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
