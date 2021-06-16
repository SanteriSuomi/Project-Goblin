using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
	public LayerMask groundLayerMask;
	public float movementSpeed;
	public float jumpVelocity;
	private float fallMultiplier = 2.5f;
	private float lowJumpMultiplier = 2f;

	RaycastHit hit;

	public BoxCollider boxCollider;

	bool grounded;

	float colliderX;
	float colliderY;

	public string turned = "right";

	public Rigidbody rb;
	public LayerMask groundLayers;

	float angle;
	float mx;

	void Awake() {
		rb = GetComponent<Rigidbody> ();
		boxCollider = GetComponent<BoxCollider>();
		colliderY = boxCollider.bounds.size.y;
		colliderX = boxCollider.bounds.size.x;
	}

	private void Update() {
		mx = Input.GetAxisRaw("Horizontal");
		Jump();
	}

	private void FixedUpdate() {
		Vector2 movement = new Vector2(mx * movementSpeed, rb.velocity.y);
		rb.velocity = movement;
	}

	void Jump() {
		if(Input.GetButtonDown("Jump") && grounded) {
			GetComponent<Rigidbody>().velocity = Vector3.up * jumpVelocity;
		}
		if(rb.velocity.y < 0) {
			rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.layer == 3)
		{
			grounded = true;
		}
	}
	private void OnCollisionExit(Collision collision) {
		if(collision.gameObject.layer == 3)
		{
			grounded = false;
		}
	}

	public bool IsGrounded() {
		bool groundCheck = Physics.BoxCast(GetComponent<Collider>().bounds.center, transform.localScale, Vector3.down, out hit, transform.rotation, GetComponent<Collider>().bounds.extents.y + 1f, groundLayerMask);
		if(groundCheck) {
			Debug.Log("Hit : " + hit.collider.name);
		}
		return groundCheck;
	}

}

