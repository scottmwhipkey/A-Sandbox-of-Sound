using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(CharacterMotor))]
public class PlatformGenerator : MonoBehaviour {
	private CharacterMotor3P motor;
	private Dictionary<Collider, int> platformDegrees;
	
	private const int MAX_SPAWN_TRIES = 5;
	
	public float sphereRadius = 20.0f;
	public float tickRate = 1.25f;
	public float initialDelay = 5.0f;
	public GameObject platformPrefab;
	public float coneTheta = 30; // 60 degrees = pi/3 radians
	public float lowerSoftClamp = -0.5f;
	public float upperSoftClamp = 0.5f;
	public float lowerHardClamp = -0.75f;
	public float upperHardClamp = 0.75f;
	
	private float coneThetaRads;
	private Vector3 lastDir = new Vector3(0.0f, 1.0f, 1.0f);

	// Use this for initialization
	void Start () {
		motor = GetComponent<CharacterMotor3P>();
		InvokeRepeating("DoGeneration", initialDelay, tickRate);
		
		platformDegrees = new Dictionary<Collider, int>();
		coneThetaRads = Mathf.Deg2Rad * coneTheta;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void DoGeneration() {
		if (motor.velocity.y < motor.terminalVelocity + 1) {
			if (Physics.OverlapSphere(transform.position, sphereRadius).Where(c => c.CompareTag("Platform")).Count() != 0) return;
			// FAILSAFE: Spawn a platform below us
			Instantiate(platformPrefab, transform.position + motor.velocity*(tickRate/2) + new Vector3(0, -2.0f, 0), Quaternion.identity);
			return;
		}
		if (lastDir.y < lowerSoftClamp) { lastDir.y = lowerSoftClamp; lastDir.Normalize(); }
		if (lastDir.y > upperSoftClamp) { lastDir.y = upperSoftClamp; lastDir.Normalize(); }
		Vector3 camdir = Camera.main.transform.TransformDirection(Vector3.forward);
		
		// find platforms in sphere of influence
		Collider[] platforms = Physics.OverlapSphere(transform.position, sphereRadius).Where(c => c.CompareTag("Platform")).Where(c =>
		{
			int degree = 0;
			platformDegrees.TryGetValue(c, out degree);
			return degree == 0;
		}).ToArray();
		if (platforms.Length == 0) return;
		int mindegree = int.MaxValue;
		Collider srcPlatform = platforms[0];
		
		Vector3 src;
		if (srcPlatform != null) {
			Transform t = srcPlatform.transform;
			while (t.parent != null) t = t.parent;
			src = t.position;
		} else {
			// don't generate
			return;
		}
		
		float minZ = Mathf.Cos(coneThetaRads);
		float z = Random.Range(minZ, 1.0f);
		float radius = Mathf.Sqrt(1.0f - z*z);
		float theta = Random.Range(0, Mathf.PI*2);
		Vector3 dir = new Vector3(Mathf.Cos(theta)*radius, Mathf.Sin(theta)*radius, z);
		// Alright, we have a vector randomized within a cone centered on (0, 0, 1) now we rotate on to world space
		
		dir = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, 1.0f), lastDir) * dir;
		// Prevent platforms from generating straight up by clamping vertical angle
		if (dir.y > upperHardClamp) dir.y = upperHardClamp;
		if (dir.y < lowerHardClamp) dir.y = lowerHardClamp;
		dir.Normalize();
		
		lastDir = dir;
		Vector3 pos = motor.calcJumpIntersect(dir) + src;  
		Debug.DrawLine(src, pos, Color.green, float.MaxValue);
		Debug.DrawLine(src, pos, Color.magenta, 1);
		Instantiate(platformPrefab, pos, Quaternion.identity);
		if (srcPlatform != null) {
			int curdegree = 0;
			platformDegrees.TryGetValue(srcPlatform, out curdegree);
			platformDegrees[srcPlatform] = curdegree + 1;
		}
		return;
	}
}
