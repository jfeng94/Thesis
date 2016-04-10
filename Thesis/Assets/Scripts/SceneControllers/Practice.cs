using UnityEngine;
using UnityEngine.VR;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class Practice : MonoBehaviour {
	private SceneObject  sceneObject  = null;
	private SceneTarget  sceneTarget  = null;
	private Hoop         hoop         = null;  
	private CalibrateBox calibrateBox = null;
	private TouchMe      touchMe      = null; 

	private int calibrateNum = 0;

	public HandStream  hs = null;

	bool proximity = false;
	bool running   = false;
	bool firstGrab = false;

	private int practiceNum = 1;

	// Use this for initialization
	void Start () {
		practiceNum = 1;
		BeginCalibration();
		VRSettings.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (sceneTarget != null && sceneObject != null) {
			if ((euclideanDist(sceneObject, sceneTarget)) < 1f && !proximity) {
				sceneTarget.EnteredProximity();
				proximity = true;
			}
			else if ((euclideanDist(sceneObject, sceneTarget)) > 1f &&proximity) {
				sceneTarget.LeftProximity();
				proximity = false;
			}
		}

		if (Input.GetKeyDown("3")) {
			VRSettings.enabled = true;
			Camera.main.fieldOfView = 60;
		}

		if (Input.GetKeyDown("2")) {
			VRSettings.enabled = false;
			Camera.main.fieldOfView = 60;
		}

		if (Input.GetKeyDown("escape")) {
			Session.instance.Home();
		}

	}

	////////////////////////////////////////////////////////////////
	// TRIAL SET UP
	////////////////////////////////////////////////////////////////
	private void LoadTrialComponents() {
		CreateSceneObject();
		CreateSceneTarget();
		CreateHoop();
	}

	private void DestroyTrialComponents() {
		if (sceneObject != null) {
			Destroy(sceneObject.gameObject);
		}
		if (sceneTarget != null) {
			Destroy(sceneTarget.gameObject);
		}
		if (hoop != null) {
			Destroy(hoop.gameObject);
		}
	}

	// Create SceneObject
	private void CreateSceneObject() {
		if (sceneObject != null) {
			Destroy(sceneObject.gameObject);
		}

		// Create sceneObject
		string siPath = "Prefabs/SceneObject";
		GameObject go = Instantiate(Resources.Load(siPath),
			            Vector3.zero,
			            Quaternion.identity) as GameObject;
		sceneObject   = go.GetComponent<SceneObject>() as SceneObject;

		if (sceneObject != null) {
			// Give it a random position and orientation.
 			sceneObject.transform.position = new Vector3(UnityEngine.Random.Range(-20f, 20f),
														 UnityEngine.Random.Range(  2f, 20f),
														 UnityEngine.Random.Range(  7f, 23f));
			sceneObject.transform.rotation = UnityEngine.Random.rotation;
		}
	}

	private void CreateSceneTarget() {
		if (sceneTarget != null) {
			Destroy(sceneTarget.gameObject);
		}

		// Create sceneTarget
		string stPath = "Prefabs/SceneTarget";
		GameObject go = Instantiate(Resources.Load(stPath),
			            Vector3.zero,
			            Quaternion.identity) as GameObject;
		sceneTarget   = go.GetComponent<SceneTarget>() as SceneTarget;

		if (sceneTarget != null) {

			float dist = float.MinValue;
			Vector3    pos = Vector3.zero;
			Quaternion rot = Quaternion.identity;

			// Make sure the target is some distance away from the object
			while (dist < 10f) {
				// Give it a random position and orientation.
				pos = new Vector3(UnityEngine.Random.Range(-20f, 20f),
								  UnityEngine.Random.Range(  2f, 20f),
								  UnityEngine.Random.Range(  7f, 23f));
				rot = UnityEngine.Random.rotation;

				float dx = pos.x - sceneObject.transform.position.x;
				float dy = pos.y - sceneObject.transform.position.y;
				float dz = pos.z - sceneObject.transform.position.z;
				dist = Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
			}
			sceneTarget.transform.position = pos;							
			sceneTarget.transform.rotation = rot;
		}
	}

	private void CreateHoop() {
		if (hoop != null) {
			Destroy(hoop.gameObject);
		}

		// Create sceneObject
		string hPath = "Prefabs/Hoop";
		GameObject go = Instantiate(Resources.Load(hPath),
			            Vector3.zero,
			            Quaternion.identity) as GameObject;
		hoop          = go.GetComponent<Hoop>() as Hoop;

		if (hoop != null) {
			float dist1 = float.MinValue;
			float dist2 = float.MinValue;
			Vector3    pos = Vector3.zero;
			Quaternion rot = Quaternion.identity;


			// Make sure the hoop is some distance away from the object and target
			while (dist1 < 10f && dist2 < 10f) {
				// Give it a random position and orientation.
 				pos = new Vector3(UnityEngine.Random.Range(-20f, 20f),
						  		  UnityEngine.Random.Range(  2f, 20f),
						  		  UnityEngine.Random.Range(  7f, 23f));
				rot = UnityEngine.Random.rotation;

				float dx1 = pos.x - sceneObject.transform.position.x;
				float dy1 = pos.y - sceneObject.transform.position.y;
				float dz1 = pos.z - sceneObject.transform.position.z;
				dist1 = Mathf.Sqrt(dx1 * dx1 + dy1 * dy1 + dz1 * dz1);

				float dx2 = pos.x - sceneTarget.transform.position.x;
				float dy2 = pos.y - sceneTarget.transform.position.y;
				float dz2 = pos.z - sceneTarget.transform.position.z;
				dist2 = Mathf.Sqrt(dx2 * dx2 + dy2 * dy2 + dz2 * dz2);
			}
			hoop.transform.position = pos;
			hoop.transform.rotation = rot;
		}
	}

	private void ResetMembers() {
		firstGrab = false;
		proximity = false;
	}

	////////////////////////////////////////////////////////////////
	// FLOW CONTROL
	////////////////////////////////////////////////////////////////
	public void BeginCalibration() {
		if (calibrateBox != null) {
			Destroy(calibrateBox.gameObject);
		}
		if (touchMe != null) {
			Destroy(touchMe.gameObject);
		}

		TouchMe();
	}

	public void TouchMe() {
		if (calibrateBox != null) {
			Destroy(calibrateBox.gameObject);
		}
		if (touchMe != null) {
			Destroy(touchMe.gameObject);
		}

		// Create sceneObject
		string tmPath = "Prefabs/TouchMe";
		GameObject go = Instantiate(Resources.Load(tmPath),
			            Vector3.zero,
			            Quaternion.identity) as GameObject;
		touchMe       = go.GetComponent<TouchMe>() as TouchMe;

		if (touchMe != null) {
			Vector3 pos = Vector3.zero;
			switch (calibrateNum) {
				case (0):
					pos = new Vector3(23.5f, 1.5f, 23.5f);
					break;

				case (1):
					pos = new Vector3(-23.5f, 1.5f, 23.5f);
					break;

				case (2):
					pos = new Vector3(23.5f, 23.5f, 23.5f);
					break;

				case (3):
					pos = new Vector3(-23.5f, 23.5f, 23.5f);
					break;

				case (4):
					LoadCalibrationBox();
					return;

			}
 			touchMe.transform.position = pos;
			touchMe.transform.rotation = Quaternion.identity;
			calibrateNum++;
		}

	}

	public void LoadCalibrationBox() {
		if (calibrateBox != null) {
			Destroy(calibrateBox.gameObject);
		}
		if (touchMe != null) {
			Destroy(touchMe.gameObject);
		}

		// Create sceneObject
		string cbPath = "Prefabs/CalibrateBox";
		GameObject go = Instantiate(Resources.Load(cbPath),
			            Vector3.zero,
			            Quaternion.identity) as GameObject;
		calibrateBox   = go.GetComponent<CalibrateBox>() as CalibrateBox;

		if (calibrateBox != null) {
			// Give it a random position and orientation.
 			calibrateBox.transform.position = new Vector3(0f, 12.5f, 12.5f);
			calibrateBox.transform.rotation = Quaternion.identity;
		}

		calibrateBox.PracticeNum(practiceNum);
	}

	public void CloseCalibrationBox() {
		if (calibrateBox != null) {
			Destroy(calibrateBox.gameObject);
		}

		BeginTrial();
	}

	public void BeginTrial() {
		ResetMembers();

		LoadTrialComponents();
	}

	public void EndTrial() {

		// If we did the last trial, finish session.
		if (practiceNum == 10) {
			Session.instance.Home();
			return;
		}

		// Delete trial components
		DestroyTrialComponents();

		// Increment session count of practice
		practiceNum++;

		// Load Calibration Box
		LoadCalibrationBox();
	}

	public void CheckCompletion() {
		if (proximity) {
			EndTrial();
		}
	} 
	////////////////////////////////////////////////////////////////
	// UTILITY
	////////////////////////////////////////////////////////////////
	private float euclideanDist(MonoBehaviour tar, MonoBehaviour obj) {
		if (tar == null || obj == null) {
			return float.PositiveInfinity;
		}

		Vector3 dist = tar.transform.position - obj.transform.position;
		float d = (float) Math.Sqrt(dist.x * dist.x + dist.y * dist.y + dist.z * dist.z);

		return d;
	}
}
