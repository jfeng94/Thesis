using UnityEngine;
using System.Collections;

public class DontTouch : MonoBehaviour {
	private Hoop hoop = null;

	void Awake() {
		hoop = transform.parent.gameObject.GetComponent<Hoop>();
	}

	void OnTriggerEnter(Collider other) {
		hoop.touching = true;
	}

	void OnTriggerExit(Collider other) {
		hoop.touching = false;
	}
}
