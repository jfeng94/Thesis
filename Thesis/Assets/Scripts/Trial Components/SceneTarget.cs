using UnityEngine;
using System;
using System.Collections;

public class SceneTarget : MonoBehaviour {
	bool flash = false;
	bool suspendFlash = false;
	bool unlit = false;
	DateTime lastFlash;

	void Update() {
		float secs = (float) (DateTime.Now - lastFlash).TotalSeconds;
		if (flash && !suspendFlash && secs > 0.25f) {
			if (unlit) {
				Material m = Resources.Load("Materials/TargetDefault") as Material;
				if (m != null) {
					Renderer r = gameObject.GetComponent<Renderer>() as Renderer;
					if (r != null) {
						r.material = m;
					}
				}
			}
			else {
				Material m = Resources.Load("Materials/TargetDefaultUnlit") as Material;
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

	public void EnteredProximity() {
		suspendFlash = true;
		Material m = Resources.Load("Materials/TargetHighlight") as Material;
		if (m != null) {
			Renderer r = gameObject.GetComponent<Renderer>() as Renderer;
			if (r != null) {
				r.material = m;
			}
		}

	}

	public void LeftProximity() {
		suspendFlash = false;
		Material m = Resources.Load("Materials/TargetDefault") as Material;
		if (m != null) {
			Renderer r = gameObject.GetComponent<Renderer>() as Renderer;
			if (r != null) {
				r.material = m;
			}
		}
	}

	public void Flash() {
		flash = true;
		lastFlash = DateTime.Now;
	}

	public void DontFlash() {
		flash = false;
	}
}
