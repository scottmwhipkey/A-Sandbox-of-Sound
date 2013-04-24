using UnityEngine;
using System.Collections;

public class ParentingTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter (Collider other) {
		other.transform.parent = transform;
	}
	
	void OnTriggerExit (Collider other) {
		other.transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
