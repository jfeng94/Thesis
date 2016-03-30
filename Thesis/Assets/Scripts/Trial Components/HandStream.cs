﻿using UnityEngine;
using System;
using System.Collections;

public class HandStream : MonoBehaviour {
	///////////
	// HYDRA //
	///////////
	public SixenseInput.Controller hydra = null;

	/////////////////
	// CALIBRATION //
	/////////////////
	private Vector3    offsetPos = Vector3.zero;
	private Quaternion offsetRot = Quaternion.identity;

	//////////////////
	// INTERACTIONS //
	//////////////////
	public GameObject scene = null;
	public Trial      trial = null;

	private SceneObject target = null;
	private SceneObject touch  = null;

	private CalibrateBox calibrateBox = null;

	private Vector3    lastPos = Vector3.zero;
	private Quaternion lastRot = Quaternion.identity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Try to connect hydra if not already.
		if (hydra == null) {
			ConnectHydra();
			if (hydra == null) {
				Debug.Log("Hydra not connected???");
				return;
			}
		}

		MoveHand();
		ReadButtonStatus();

	}

	private void ConnectHydra() {
		if (SixenseInput.IsBaseConnected(0) != false) {
			hydra = SixenseInput.Controllers[0];
		}
	}

	/////////////////////////////////////////////////////////////////
	// MOVE HAND
	/////////////////////////////////////////////////////////////////
	public void MoveHand() {
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
	}

	/////////////////////////////////////////////////////////////////
	// HYDRA BUTTONS
	/////////////////////////////////////////////////////////////////
	private void ReadButtonStatus() {
		if (hydra.GetButtonDown(SixenseButtons.TRIGGER)) {
			if (touch != null) {
				target = touch;
				target.transform.parent = transform;
				trial.CheckFirstGrab();
			}
			if (calibrateBox != null) {
				trial.CloseCalibration();
			}
		}

		if (hydra.GetButtonUp(SixenseButtons.TRIGGER)) {
			if (target != null) {
				target.transform.parent = scene.transform;
				target = null;
				trial.CheckCompletion();
			}
		}

		if (hydra.GetButtonDown(SixenseButtons.START)) {
			offsetPos = hydra.Position + new Vector3(0f, 10f, -5f);
		}
	}


	public int TriggerStatus() {
		if (hydra == null) {
			return 0;
		}

		if (hydra.GetButton(SixenseButtons.TRIGGER)) {
			return 1;
		}
		return 0;
	}

	/////////////////////////////////////////////////////////////////
	// COLLIDER METHODS
	/////////////////////////////////////////////////////////////////
	void OnTriggerEnter(Collider other) {
		SceneObject si = other.gameObject.
		                       GetComponent<SceneObject>()
		                       as SceneObject;
		if (si != null) {
			touch = si;
			touch.Highlight();
		}

		CalibrateBox cb =  other.gameObject.
		                         GetComponent<CalibrateBox>()
		                         as CalibrateBox;

		if (cb != null) {
			calibrateBox = cb;
			calibrateBox.Highlight();
		}

		TouchMe tm = other.gameObject.
		                   GetComponent<TouchMe>()
		                   as TouchMe;

		if (tm != null) {
			trial.TouchMe();
		}
	}

	void OnTriggerExit(Collider other) {
		if (touch != null) {
			touch.Unhighlight();
			touch = null;
		}

		if (calibrateBox != null) {
			calibrateBox.Unhighlight();
			calibrateBox = null;
		}
	}
}

