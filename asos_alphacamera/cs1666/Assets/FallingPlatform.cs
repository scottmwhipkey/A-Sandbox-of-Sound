using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour {

	private float fallTime = float.MaxValue;
	
	public float delay = 0.5f;
	public Rigidbody platform;

	// Use this for initialization
	void Start () {
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			fallTime = Time.time + delay;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > fallTime) {
			platform.useGravity = true;
			fallTime = float.MaxValue;
		}
	}
}
