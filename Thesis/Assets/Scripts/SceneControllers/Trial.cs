using UnityEngine;
using System;
using System.Text;
using System.Collections;

public class Trial : MonoBehaviour {
	SceneObject  sceneObject  = null;
	SceneTarget  sceneTarget  = null;
	Hoop         hoop         = null;  
	CalibrateBox calibrateBox = null;   

	public HandStream  hs = null;

	bool proximity = false;
	bool running   = false;
	bool firstGrab = false;

	private float minHoopDist   = float.PositiveInfinity;
	private float minTargDist   = float.PositiveInfinity;
	private float minHoopTime   = float.PositiveInfinity;
	private float minTargTime   = float.PositiveInfinity;
	private int   minHoopFrame  = 0;
	private int   minTargFrame  = 0;
	private float firstGrabTime = float.PositiveInfinity;

	private DateTime start;
	private int frame = 0;

	private int trialNum = 0;
	// Use this for initialization
	void Start () {
		BeginCalibration();
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

		if (running) {
			UpdateMinDists();
			string line = GetStateText();
			frame++;
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
			// Give it a random position and orientation.
			sceneTarget.transform.position = new Vector3(UnityEngine.Random.Range(-20f, 20f),
														 UnityEngine.Random.Range(  2f, 20f),
														 UnityEngine.Random.Range(  7f, 23f));
			sceneTarget.transform.rotation = UnityEngine.Random.rotation;
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
			// Give it a random position and orientation.
 			hoop.transform.position = new Vector3(UnityEngine.Random.Range(-20f, 20f),
												  UnityEngine.Random.Range(  2f, 20f),
												  UnityEngine.Random.Range(  7f, 23f));
			hoop.transform.rotation = UnityEngine.Random.rotation;
		}

	}

	private void ResetMembers() {
		minHoopDist   = float.PositiveInfinity;
		minTargDist   = float.PositiveInfinity;
		minHoopTime   = float.PositiveInfinity;
		minTargTime   = float.PositiveInfinity;
		minHoopFrame  = 0;
		minTargFrame  = 0;
		firstGrabTime = float.PositiveInfinity;

		frame     = 0;
		running   = true;
		firstGrab = false;
		proximity = false;
	}
	////////////////////////////////////////////////////////////////
	// UPDATE HELPERS
	////////////////////////////////////////////////////////////////
	private void UpdateMinDists() {
		// Distance between sceneObject and Hoop
		float dist = euclideanDist(sceneObject, hoop);
		if (dist < minHoopDist) {
			minHoopDist  = dist;
			minHoopTime  = (float) (DateTime.Now - start).TotalSeconds;
			minHoopFrame = frame;
		}

		// Distance between sceneObject and sceneTarget {
		dist = euclideanDist(sceneObject, sceneTarget);
		if (dist < minTargDist) {
			minTargDist  = dist;
			minTargTime  = (float) (DateTime.Now - start).TotalSeconds;
			minTargFrame = frame;
		}
	}

	private string GetStateText() {
		// On every frame, I want to record the following things:
		StringBuilder line = new StringBuilder();
		line.Append("Frame ");
		line.Append(frame);
		line.Append("\t");												// Column 0: Frame info

		// 1. Hand position, rotation
		Vector3    pos = hs.transform.position;
		Quaternion rot = hs.transform.rotation;
		AppendVector3(ref line, pos);									// Column 1, 2, 3: Hand pos
		AppendQuaternion(ref line, rot);								// Column 4, 5, 6, 7: Hand rot

		// 2. Hand button status.
		line.Append(hs.TriggerStatus());
		line.Append("\t");												// Column 8: Trigger status

		// 3. SceneObject position, rotation.
		pos = sceneObject.transform.position;
		rot = sceneObject.transform.rotation;
		AppendVector3(ref line, pos);									// Column 9, 10, 11: SceneObject pos
		AppendQuaternion(ref line, rot);								// Column 12, 13, 14, 15: SceneObject rot

		// 6. Minimum distance from center of hoop
		line.Append(minHoopDist);
		line.Append("\t");												// Column 16: Minimum distance from hoop achieved

		// 7. Minimum distance from center of target
		line.Append(minTargDist);
		line.Append("\t");												// Column 17: Minimum distance from sceneTarget achieved.

		// 8. Time since trial began
		line.Append((DateTime.Now - start).TotalSeconds);

		line.Append("\n");

		return line.ToString();
	}

	public string GetSummaryText() {
		StringBuilder line = new StringBuilder();

		line.Append("Summary of Trial\t" + trialNum + "\n");
		line.Append("Time to first grab:\t" + firstGrabTime + "\n");
		line.Append("Time to hoop:\t" + minHoopTime + "\n");
		line.Append("Time to target:\t" + minTargTime + "\n");
		line.Append("Minimum distance to hoop:\t" + minHoopDist + "\n");
		line.Append("Minimum distance to target:\t" + minTargDist + "\n");

		Debug.Log(line.ToString());

		return line.ToString();
	}

	////////////////////////////////////////////////////////////////
	// FLOW CONTROL
	////////////////////////////////////////////////////////////////
	public void BeginCalibration() {
		if (calibrateBox != null) {
			Destroy(calibrateBox.gameObject);
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
	}

	public void EndCalibration() {
		if (calibrateBox != null) {
			Destroy(calibrateBox.gameObject);
		}

		trialNum = 0;

		BeginTrial();
	}

	public void BeginTrial() {
		start = DateTime.Now;

		// Create write out filestream for this trial.

		LoadTrialComponents();
		// Write initial configuration of trial
		// SceneObject location
		// SceneTarget location
		// Hoop location

		ResetMembers();
	}

	public void CheckFirstGrab() {
		if (!firstGrab) {
			firstGrab = true;
			firstGrabTime = (float) (DateTime.Now - start).TotalSeconds;
		}

	}

	public void CheckCompletion() {
		if (proximity) {
			running = false;
			NextTrial();
		}
	}
	public void NextTrial() {
		// Write summary for trial
		GetSummaryText();

		trialNum++;
		BeginTrial();
	}

	public void EndTrials() {
		// Write summary of entire session
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

	private void AppendVector3(ref StringBuilder sb, Vector3 pos) {
		sb.Append(pos.x);
		sb.Append("\t");
		sb.Append(pos.y);
		sb.Append("\t");
		sb.Append(pos.z);
		sb.Append("\t");
	}

	private void AppendQuaternion(ref StringBuilder sb, Quaternion rot) {
		sb.Append(rot.w);
		sb.Append("\t");
		sb.Append(rot.x);
		sb.Append("\t");
		sb.Append(rot.y);
		sb.Append("\t");
		sb.Append(rot.z);
		sb.Append("\t");

	}
}
