using UnityEngine;
using System;
using System.Collections;

public class Hoop : MonoBehaviour {
	public bool inside   = false;
	public bool touching = false;

	private enum Mode {DEFAULT, RED, GREEN};
	private Mode mode = Mode.DEFAULT;

	private Material unlitMat   = null;
	private Material defaultMat = null;
	private Material redMat     = null;
	private Material greenMat   = null; 
	private Renderer rend       = null;

	bool flash = false;
	bool suspendFlash = false;
	bool unlit = false;
	DateTime lastFlash;

	void Start() {
		unlitMat   = Resources.Load("Materials/HoopDefaultUnlit") as Material;
		defaultMat = Resources.Load("Materials/HoopDefault")      as Material;
		redMat     = Resources.Load("Materials/HoopRed")          as Material;
		greenMat   = Resources.Load("Materials/HoopGreen")        as Material;
		rend       = gameObject.GetComponent<Renderer>() as Renderer;
	}

	// Update is called once per frame
	void Update () {
		if (touching) {
			if (mode != Mode.RED) {
				UpdateMode(Mode.RED);
			}
		}
		else if (inside) {
			if (mode != Mode.GREEN) {
				UpdateMode(Mode.GREEN);
			}
		}
		else {
			if (mode != Mode.DEFAULT) {
				UpdateMode(Mode.DEFAULT);
			}
		}

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

	private void UpdateMode(Mode mode_) {
		Material m = null;
		switch (mode_) {
			case (Mode.DEFAULT):
				suspendFlash = false;
				m = defaultMat;
				break;

			case (Mode.RED):
				suspendFlash = true;
				m = redMat;
				break;

			case (Mode.GREEN):
				suspendFlash = true;
				m = greenMat;
				break;
		}
		mode = mode_;

		if (m != null && rend != null) {
			rend.material = m;
		}
		else {
				Debug.Log("Renderer is null!");
		}
	}

	public bool IsRed() {
		return (mode == Mode.RED);
	}

	public bool InHoop() {
		return (mode == Mode.RED || mode == Mode.GREEN);
	}

	public void Flash() {
		flash = true;
		lastFlash = DateTime.Now;
	}

	public void DontFlash() {
		flash = false;
		
	}
}
