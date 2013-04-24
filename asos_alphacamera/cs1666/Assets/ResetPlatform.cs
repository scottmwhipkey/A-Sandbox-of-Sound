using UnityEngine;
using System.Collections;

public class ResetPlatform : MonoBehaviour {

	private Vector3 origin;
	private Quaternion rotation;

	// Use this for initialization
	void Start () {
		Transform t = transform;
		origin = t.position;
		rotation = t.rotation;
	}
	
	void Reset () {
		Transform t = transform;
		t.position = origin;
		t.rotation = rotation;
		Rigidbody phys = GetComponent<Rigidbody>();
		phys.velocity = new Vector3(0.0f, 0.0f, 0.0f);
		phys.useGravity = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
