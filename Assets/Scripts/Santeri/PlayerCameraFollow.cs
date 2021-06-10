using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField]
    Camera cam;
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
        offset = cam.transform.position - transform.position;
    }

    private void FixedUpdate()
    {
        float smooth = Mathf.Clamp((Mathf.Abs(rb.velocity.z) / 10) / 1, minSmooth, maxSmooth) * smoothMultiplier;
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, transform.position + offset, ref velocity, smooth);
    }
}
