using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;

    [SerializeField]
    float walkingThreshold = 0.25f;
    [SerializeField]
    string walkCondition = "Walking";

    [SerializeField]
    float jumpingThreshold = 0.25f;
    [SerializeField]
    string jumpCondition = "Jumping";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        bool walkState = animator.GetBool(walkCondition);
        if (Mathf.Abs(rb.velocity.z) > walkingThreshold && !walkState)
        {
            animator.SetBool(walkCondition, true);
        }
        else if (walkState)
        {
            animator.SetBool(walkCondition, false);
        }

        bool jumpState = animator.GetBool(jumpCondition);
        if (Mathf.Abs(rb.velocity.y) > jumpingThreshold && !jumpState)
        {
            animator.SetBool(jumpCondition, true);
        }
        else if (jumpState)
        {
            animator.SetBool(jumpCondition, false);
        }
    }
}
