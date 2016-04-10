using UnityEngine;
using System.Collections;

public class Hoop : MonoBehaviour {
	public bool inside   = false;
	public bool touching = false;

	private enum Mode {DEFAULT, RED, GREEN};
	private Mode mode = Mode.DEFAULT;

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
	}

	private void UpdateMode(Mode mode_) {
		string path = "Materials/";
		switch (mode_) {
			case (Mode.DEFAULT):
				path += "HoopDefault";
				break;

			case (Mode.RED):
				path += "HoopRed";
				break;

			case (Mode.GREEN):
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
}
