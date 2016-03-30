﻿using UnityEngine;
using System.Collections;

public class SceneTarget : MonoBehaviour {

	public void EnteredProximity() {
		Material m = Resources.Load("Materials/TargetHighlight") as Material;
		if (m != null) {
			Renderer r = gameObject.GetComponent<Renderer>() as Renderer;
			if (r != null) {
				r.material = m;
			}
		}

	}

	public void LeftProximity() {
		Material m = Resources.Load("Materials/TargetDefault") as Material;
		if (m != null) {
			Renderer r = gameObject.GetComponent<Renderer>() as Renderer;
			if (r != null) {
				r.material = m;
			}
		}
	}
}
