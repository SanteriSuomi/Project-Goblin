using UnityEngine;
using System.Collections;

public class PlayerDashTest : MonoBehaviour
{
    [SerializeField]
    float acceleration = 1000;


    [SerializeField]
    float dashMultiplier = 100;
    [SerializeField]
    float dashDrag = 4;
    [SerializeField]
    float dashCooldown = 1.4f;
    WaitForSeconds dashWFS;

    Rigidbody rb;
    float originalDrag;

    // Input bools
    bool forward;
    bool backward;
    bool dash;
    bool canDash = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.drag;
        dashWFS = new WaitForSeconds(dashCooldown);
    }

    private void Update()
    {
        GetInput();
        Move();
    }

    void GetInput()
    {
        forward = Input.GetKey(KeyCode.D);
        backward = Input.GetKey(KeyCode.A);
        dash = Input.GetKeyDown(KeyCode.LeftShift);
    }

    void Move()
    {
        Vector3 move = Vector3.zero;
        if (forward)
        {
            move += transform.forward * acceleration;
        }
        if (backward)
        {
            move += -(transform.forward * acceleration);
        }
        if (dash && canDash && move.sqrMagnitude > 0)
        {
            move *= dashMultiplier;
            StartCoroutine(nameof(DashEnd));
        }
        move *= Time.deltaTime;
        rb.AddForce(move, ForceMode.Force);
    }

    IEnumerator DashEnd()
    {
        canDash = false;
        rb.drag = dashDrag;
        yield return dashWFS;
        rb.drag = originalDrag;
        canDash = true;
    }
}
