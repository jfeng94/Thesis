using UnityEngine;
using System;
using System.Collections;

public class SceneObject : MonoBehaviour {
	bool flash = false;
	bool suspendFlash = false;
	bool unlit = false;
	DateTime lastFlash;

	private Material unlitMat   = null;
	private Material defaultMat = null;
	private Material highlitMat = null;
	private Renderer rend       = null;


	void Start() {
		unlitMat   = Resources.Load("Materials/ObjectDefaultUnlit") as Material;
		defaultMat = Resources.Load("Materials/ObjectDefault")      as Material;
		highlitMat = Resources.Load("Materials/ObjectHighlight")    as Material;
		rend       = gameObject.GetComponent<Renderer>() as Renderer;
	}

	void Update() {
		float secs = (float) (DateTime.Now - lastFlash).TotalSeconds;
		if (flash && !suspendFlash && secs > 0.25f) {
			if (unlit) {
				if (defaultMat != null && rend != null) {
					rend.material = defaultMat;
				}
			}
			else {
				if (unlitMat != null && rend != null) {
					rend.material = unlitMat;
				}
			}

			unlit = !unlit;
			lastFlash = DateTime.Now;
		}
	}

	public void Highlight() {
		suspendFlash = true;
		if (highlitMat != null && rend != null) { 
			rend.material = highlitMat;
		}
	}

	public void Unhighlight() {
		suspendFlash = false;
		if (defaultMat != null && rend != null) { 
			rend.material = defaultMat;
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
