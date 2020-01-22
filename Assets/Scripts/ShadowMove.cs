using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMove : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
        target = transform.parent.transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 posXZ = target.position;
		RaycastHit hit;
		int layerMask = 1 << 8;
		Physics.Raycast (target.position, Vector3.down, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide);

		transform.position = new Vector3 (posXZ.x, hit.point.y + 0.15f, posXZ.z);
		transform.up = hit.normal;
	}
}
