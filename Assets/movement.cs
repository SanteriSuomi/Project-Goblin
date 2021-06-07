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

	public BoxCollider boxCollider;

	float colliderX;
	float colliderY;

	public string turned = "right";

	public Rigidbody rb;
	public LayerMask groundLayers;

	float angle;
	float mx;
	float flip = 0f;
	float width = 1f;
	float height = 1f;

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
		turnAround();
		transform.localRotation = Quaternion.Euler(0, flip, 0);
		rb.velocity = movement;
	}

	void Jump() {
		if(Input.GetButtonDown("Jump")) {
			GetComponent<Rigidbody>().velocity = Vector3.up * jumpVelocity;
		}
		if(rb.velocity.y < 0) {
			rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	void turnAround() {
		if ((angle >= 91f || angle <= -91f) || turned.Equals("left")) {
			if(flip < 180f && flip >= 135f) {
				flip += 2.5f;
			}
			else if(flip < 135f) {
				turned = "left";
				flip = 135f;
			}
		}
		if ((angle <= 90f && angle > -90f) || turned.Equals("right")) {
			if(flip > 0f && flip <= 45f) {
				flip -= 2.5f;
			}
			else if(flip > 45f) {
				turned = "right";
				flip = 45f;
			}
		}

	}

	public bool IsGrounded() {
		bool groundCheck = Physics2D.BoxCast(GetComponent<Collider>().bounds.center, GetComponent<Collider>().bounds.size, 0 , Vector2.down, GetComponent<Collider>().bounds.extents.y, groundLayerMask);
		return groundCheck;
	}
}

