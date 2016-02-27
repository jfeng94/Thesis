using UnityEngine;
using System;
using System.Collections;

public class HandStream : MonoBehaviour {
	///////////
	// HYDRA //
	///////////
	private SixenseInput.Controller hydra = null;

	/////////////////
	// CALIBRATION //
	/////////////////
	private Vector3    offsetPos = Vector3.zero;
	private Quaternion offsetRot = Quaternion.identity;

	//////////////////
	// INTERACTIONS //
	//////////////////
	public GameObject scene = null;

	private SceneInteractable target = null;
	private SceneInteractable touch  = null;

	private Vector3    lastPos = Vector3.zero;
	private Quaternion lastRot = Quaternion.identity;

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


		/////////////////////////////////////////////////////////////////
		// MOVE HAND
		/////////////////////////////////////////////////////////////////
		if (hydra.Enabled != false) {

			// Scale input to our room size with the calibration offset.
			Vector3 pos = (hydra.Position - offsetPos) * 0.05f;
			

			// Box input to room.
			pos.x = Math.Min( 25f, pos.x);
			pos.x = Math.Max(-25f, pos.x);
			pos.y = Math.Min( 25f, pos.y);
			pos.y = Math.Max(  0f, pos.y);
			pos.z = Math.Min( 25f, pos.z);
			pos.z = Math.Max(-25f, pos.z);

			// Calculate offset from last position.
			// Note if somehow the user grabs on frame 1 we'll get
			// weird results.
			Vector3    dPos = pos - transform.position;
			Quaternion dRot = (lastRot * Quaternion.Inverse(hydra.Rotation));

			// Move hand
			transform.position = pos;
			transform.rotation = hydra.Rotation;
		}

		/////////////////////////////////////////////////////////////////
		// HYDRA BUTTONS
		/////////////////////////////////////////////////////////////////
		if (hydra.GetButtonDown(SixenseButtons.TRIGGER)) {
			if (touch != null) {
				target = touch;
				target.transform.parent = transform;
			}
		}

		if (hydra.GetButtonUp(SixenseButtons.TRIGGER)) {
			if (target != null) {
				target.transform.parent = scene.transform;
				target = null;
			}
		}

		if (hydra.GetButtonDown(SixenseButtons.START)) {
			offsetPos = hydra.Position + new Vector3(0f, 10f, -5f);
		}
	}

	void OnTriggerEnter(Collider other) {
		SceneInteractable si = other.gameObject.
		                       GetComponent<SceneInteractable>()
		                       as SceneInteractable;
		if (si != null) {
			touch = si;
			touch.Highlight();
		}
	}

	void OnTriggerExit(Collider other) {
		if (touch != null) {
			touch.Unhighlight();
			touch = null;
		}
	}
}

