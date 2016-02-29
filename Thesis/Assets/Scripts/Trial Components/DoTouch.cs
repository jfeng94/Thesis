using UnityEngine;
using System.Collections;

public class DoTouch : MonoBehaviour {
	private Hoop hoop = null;

	void Awake() {
		hoop = transform.parent.gameObject.GetComponent<Hoop>();
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log("Entered Hoop");
		hoop.inside = true;
	}

	void OnTriggerExit(Collider other) {
		Debug.Log("Exited Hoop");
		hoop.inside = false;
	}
}
