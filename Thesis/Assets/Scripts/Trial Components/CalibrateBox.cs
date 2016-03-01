using UnityEngine;
using System.Collections;

public class CalibrateBox : MonoBehaviour {
	public void Highlight() {
		Material m = Resources.Load("Materials/ObjectHighlight") as Material;
		if (m != null) {
			Renderer r = gameObject.GetComponent<Renderer>() as Renderer;
			if (r != null) {
				r.material = m;
			}
			else {
				Debug.Log("Renderer is null!");
			}
		}
		else {
			Debug.Log("Material is null");
		}
	}

	public void Unhighlight() {
		Material m = Resources.Load("Materials/ObjectDefault") as Material;
		if (m != null) {
			Renderer r = gameObject.GetComponent<Renderer>() as Renderer;
			if (r != null) {
				r.material = m;
			}
		}
	}
}
