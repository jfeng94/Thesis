using UnityEngine;
using System;
using System.Collections;

public class Hoop : MonoBehaviour {
	public bool inside   = false;
	public bool touching = false;

	private enum Mode {DEFAULT, RED, GREEN};
	private Mode mode = Mode.DEFAULT;


	bool flash = false;
	bool suspendFlash = false;
	bool unlit = false;
	DateTime lastFlash;

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
				Material m = Resources.Load("Materials/HoopDefault") as Material;
				if (m != null) {
					Renderer r = gameObject.GetComponent<Renderer>() as Renderer;
					if (r != null) {
						r.material = m;
					}
				}
			}
			else {
				Material m = Resources.Load("Materials/HoopDefaultUnlit") as Material;
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

	private void UpdateMode(Mode mode_) {
		string path = "Materials/";
		switch (mode_) {
			case (Mode.DEFAULT):
				suspendFlash = false;
				path += "HoopDefault";
				break;

			case (Mode.RED):
				suspendFlash = true;
				path += "HoopRed";
				break;

			case (Mode.GREEN):
				suspendFlash = true;
				path += "HoopGreen";
				break;
		}
		mode = mode_;

		Material m = Resources.Load(path) as Material;
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
