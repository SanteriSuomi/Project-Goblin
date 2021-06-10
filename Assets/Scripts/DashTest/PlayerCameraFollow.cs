using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField]
    Camera mainCam;
    [SerializeField]
    float smoothMultiplier = 0.5f;
    [SerializeField]
    float minSmooth = 0.2f;
    [SerializeField]
    float maxSmooth = 1;

    Rigidbody rb;
    Vector3 velocity = Vector3.zero;
    Vector3 offset;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        offset = mainCam.transform.position - transform.position;
    }

    private void FixedUpdate()
    {
        float smooth = Mathf.Clamp(((Mathf.Abs(rb.velocity.z) / 10) * smoothMultiplier) / 1, minSmooth, maxSmooth);
        mainCam.transform.position = Vector3.SmoothDamp(mainCam.transform.position, transform.position + offset, ref velocity, smooth);
    }
}
