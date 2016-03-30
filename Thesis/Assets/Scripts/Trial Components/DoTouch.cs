using UnityEngine;
using System.Collections;

public class DoTouch : MonoBehaviour {
	private Hoop hoop = null;

	void Awake() {
		hoop = transform.parent.gameObject.GetComponent<Hoop>();
	}

	void OnTriggerEnter(Collider other) {
		hoop.inside = true;
	}

	void OnTriggerExit(Collider other) {
		hoop.inside = false;
	}
}
