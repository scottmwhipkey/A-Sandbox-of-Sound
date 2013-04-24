using UnityEngine;
using System.Collections;

public class SpawnTrigger : MonoBehaviour {

	public Transform respawnPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			other.transform.position = respawnPoint.position;
			other.transform.rotation = respawnPoint.rotation;
		} else {
			other.SendMessage("Reset", SendMessageOptions.RequireReceiver);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
