using UnityEngine;
using System;
using System.Collections;

public class SceneObject : MonoBehaviour {
	bool flash = false;
	bool suspendFlash = false;
	bool unlit = false;
	DateTime lastFlash;

	void Update() {
		float secs = (float) (DateTime.Now - lastFlash).TotalSeconds;
		if (flash && !suspendFlash && secs > 0.25f) {
			if (unlit) {
				Material m = Resources.Load("Materials/ObjectDefault") as Material;
				if (m != null) {
					Renderer r = gameObject.GetComponent<Renderer>() as Renderer;
					if (r != null) {
						r.material = m;
					}
				}
			}
			else {
				Material m = Resources.Load("Materials/ObjectDefaultUnlit") as Material;
				if (m != null) {
					Renderer r = gameObject.GetComponent<Renderer>() as Renderer;
					if (r != null) {
						r.material = m;
					}
				}
			}

			unlit = !unlit;
			lastFlash = DateTime.Now;
		}
	}

	public void Highlight() {
		suspendFlash = true;
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
		suspendFlash = false;
		Material m = Resources.Load("Materials/ObjectDefault") as Material;
		if (m != null) {
			Renderer r = gameObject.GetComponent<Renderer>() as Renderer;
			if (r != null) {
				r.material = m;
			}
		}
	}

	public void Flash() {
		lastFlash = DateTime.Now;
		flash = true;
	}

	public void DontFlash() {
		flash = false;
	}
}
