using UnityEngine;
using System.Collections;

public class HandStream : MonoBehaviour {
	private SixenseInput.Controller hydra = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (hydra == null) {
			if (SixenseInput.IsBaseConnected(0) != false) {
				hydra = SixenseInput.Controllers[0];
			}
		}

		if (hydra == null) {
			return;
		}

		//if (hydra.Enabled != false) {
		//	transform.position = hydra.Position;
		//	transform.rotation = hydra.Rotation;
		//}
	}
}
