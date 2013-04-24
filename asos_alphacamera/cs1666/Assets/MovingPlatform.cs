using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {


	public Transform destination;
	public float speed = 0.5f;
	private Vector3 src;
	private Vector3 dst;

	// Use this for initialization
	void Start () {
		// save current location
		src = transform.position;
		dst = destination.position;
	}
	
	// Update is called once per frame
	void Update () {
		float spd = Time.deltaTime * speed;
		Vector3 todst = dst - transform.position;
		Vector3 move = todst.normalized * spd;
		if (move.sqrMagnitude > todst.sqrMagnitude) move = todst; // avoid overshooting
		
		Transform t = transform;
		t.position = t.position + move;
		
		if (t.position == dst) {
			Vector3 tmp = src;
			src = dst;
			dst = tmp;
		}
	}
}
