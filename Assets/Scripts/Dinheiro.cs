using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinheiro : MonoBehaviour {

	Transform target;
	public void Init (Vector3 startPosition, Transform target) {
		this.transform.position = startPosition;
		this.target = target;
	}

	void FixedUpdate () {
		if (target != null) {
			transform.position = Vector3.Lerp (transform.position, target.position, Time.deltaTime * 3);
			if ((transform.position - target.position).magnitude < 0.5f) {
				Destroy (this.gameObject);
			}

		}
	}
}