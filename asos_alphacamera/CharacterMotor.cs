using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour {
	public float speed = 6.0f;
	public float airAcceleration = 30.0f;
	public float gravity = 20.0f;
	public float maxJumpHeight = 4.0f;
	
	[HideInInspector]
	[NonSerialized]
	public float initialJumpVelocity;
	
	private CharacterController controller;
	
	private Vector3 velocity = Vector3.zero;
	private float lastJumpPressTime = float.MinValue;
	
	// Use this for initialization
	void Awake () {
		controller = GetComponent<CharacterController>();
		velocity = Vector3.zero;
		
		initialJumpVelocity = Mathf.Sqrt(2 * gravity * maxJumpHeight);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Jump") != 0) {
			lastJumpPressTime = Time.time;
		}
		// if the last frame's movement caused a collision with the ground, then for this frame we are grounded
		bool grounded = controller.isGrounded;
		// Gravity
		float g = -gravity * Time.deltaTime;
		if (grounded) {
			velocity.y = Mathf.Max(velocity.y, 0.0f) + g;
			velocity.x = 0;
			velocity.z = 0;
			
			// Hack to keep us grounded
			if (velocity.y < 0)
				velocity.y = Mathf.Min(velocity.y, -1.0f);
		} else {
			velocity.y += g;
		}
		
		// Determine movement direction
		Transform cameraTransform = Camera.main.transform;
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		
		Vector3 move = Vector3.zero;
		// We want smoothed input, but want movement to stop instantly upon letting off the keys
		if ((Input.GetAxisRaw("Vertical") != 0.0f) || (Input.GetAxisRaw("Horizontal") != 0.0f)) {
			move = (forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal"));
		}
		move = move.normalized;
		// move is movement direction calculated from input. move * speed + velocity to determine frame velocity
		// (move * speed + velocity) * timedelta = frame movement
		
		if (grounded) {
			velocity += move * speed;
			if ((Time.time - lastJumpPressTime) < 0.2) { // if jump was pressed in last .2 seconds
				// jump
				grounded = false;
				velocity.y = initialJumpVelocity;
			}
		} else {
			Vector3 newvel = velocity + move * airAcceleration * Time.deltaTime;
			float xzMagSqr = newvel.x*newvel.x + newvel.z*newvel.z;
			if (xzMagSqr > (velocity.x * velocity.x + velocity.z * velocity.z)) { 
				if (xzMagSqr < (speed * speed)) {
					velocity = newvel;
				}
			} else {
				velocity = newvel;
			}
		}
		
		//Vector3 frameMove = (velocity + move * speed) * Time.deltaTime;
		Vector3 frameMove = velocity * Time.deltaTime;
		
		controller.Move(frameMove);
		
		if (move != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation(move);
		}
	}
}
