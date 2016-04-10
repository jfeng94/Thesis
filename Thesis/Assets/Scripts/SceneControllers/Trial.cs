using UnityEngine;
using UnityEngine.VR;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class Trial : MonoBehaviour {
	private SceneObject  sceneObject  = null;
	private SceneTarget  sceneTarget  = null;
	private Hoop         hoop         = null;  
	private CalibrateBox calibrateBox = null;
	private TouchMe      touchMe      = null; 


	private StreamWriter trialStream;
	private StreamWriter summaryStream;
	private int calibrateNum = 0;

	public HandStream  hs = null;

	bool proximity  = false;
	bool running    = false;
	bool firstGrab  = false;
	bool passedHoop = false;

	////////////////////////////////////////////////////////////////////////////
	// Data to keep track of per trial
	////////////////////////////////////////////////////////////////////////////
	private float minHoopDist   = float.PositiveInfinity;
	private float minTargDist   = float.PositiveInfinity;
	private float minHoopTime   = float.PositiveInfinity;
	private float minTargTime   = float.PositiveInfinity;
	private int   minHoopFrame  = 0;
	private int   minTargFrame  = 0;
	private float firstGrabTime = float.PositiveInfinity;

	private Vector3    initSOPos   = Vector3.zero;
	private Vector3    initSTPos   = Vector3.zero;
	private Vector3    initHoopPos = Vector3.zero;
	private Quaternion initSORot   = Quaternion.identity; 
	private Quaternion initSTRot   = Quaternion.identity; 
	private Quaternion initHoopRot = Quaternion.identity; 

	private int hoopRedFrames = 0;

	private DateTime start;
	private int frame = 0;

	private int trialNum = 0;

	// Use this for initialization
	void Start () {
		BeginCalibration();

		if (Session.instance.user3D) {
			VRSettings.enabled = true;
		}
		else {
			VRSettings.enabled = false;
			Camera.main.fieldOfView = 60;

		}
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
			trialStream.Write(line);

			if (hoop.InHoop()) {
				passedHoop = true;

				hoop.DontFlash();
				sceneTarget.Flash();
			}

			frame++;
		}

		if (Input.GetKeyDown("space")) {
			if (Session.instance.user3D) {
				VRSettings.enabled = !VRSettings.enabled;
			}
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

		// Assign initial configuration items
		initSOPos = sceneObject.transform.position;
		initSORot = sceneObject.transform.rotation;
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
			initSTPos = pos;
			initSTRot = rot;
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
			while (dist1 < 10f || dist2 < 10f) {
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
			initHoopPos = pos;
			initHoopRot = rot;
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

		hoopRedFrames = 0;

		frame      = 0;
		running    = true;
		firstGrab  = false;
		proximity  = false;
		passedHoop = false;
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

	private string GetStateHeader() {
		StringBuilder line = new StringBuilder();
		line.Append("0. Frame Number\t");

		line.Append("1. HandStream pos x\t");
		line.Append("2. HandStream pos y\t");
		line.Append("3. HandStream pos z\t");
		line.Append("4. HandStream rot x\t");
		line.Append("5. HandStream rot y\t");
		line.Append("6. HandStream rot z\t");
		line.Append("7. HandStream rot w\t");

		line.Append("8. Trigger status\t");

		line.Append("9. SceneObject pos x\t");
		line.Append("10. SceneObject pos y\t");
		line.Append("11. SceneObject pos z\t");
		line.Append("12. SceneObject rot x\t");
		line.Append("13. SceneObject rot y\t");
		line.Append("14. SceneObject rot z\t");
		line.Append("15. SceneObject rot w\t");

		line.Append("16. MinHoopDist\t");
		line.Append("17. MinTargDist\t");
		line.Append("18. Time\t");

		line.Append("19. Initial SO pos x\t");
		line.Append("20. Initial SO pos y\t");
		line.Append("21. Initial SO pos z\t");
		line.Append("22. Initial SO rot x\t");
		line.Append("23. Initial SO rot y\t");
		line.Append("24. Initial SO rot z\t");
		line.Append("25. Initial SO rot w\t");

		line.Append("26. Initial ST pos x\t");
		line.Append("27. Initial ST pos y\t");
		line.Append("28. Initial ST pos z\t");
		line.Append("29. Initial ST rot x\t");
		line.Append("30. Initial ST rot y\t");
		line.Append("31. Initial ST rot z\t");
		line.Append("32. Initial ST rot w\t");

		line.Append("33. Initial Hoop pos x\t");
		line.Append("34. Initial Hoop pos y\t");
		line.Append("35. Initial Hoop pos z\t");
		line.Append("36. Initial Hoop rot x\t");
		line.Append("37. Initial Hoop rot y\t");
		line.Append("38. Initial Hoop rot z\t");
		line.Append("39. Initial Hoop rot w\t");

		line.Append("40. Touching hoop\n");

		return line.ToString();
	}

	private string GetStateText() {
		// On every frame, I want to record the following things:
		StringBuilder line = new StringBuilder();
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
		line.Append((DateTime.Now - start).TotalSeconds);				// Column 18: Time since trial began
		line.Append("\t");

		// 9. Scene Object Initial Configuration
		AppendVector3(ref line, initSOPos);								// Column 19, 20, 21: SceneObject Pos
		AppendQuaternion(ref line, initSORot);							// Column 22, 23, 24, 25: SceneObject rot


		// 10. SceneTarget Initial Configuration
		AppendVector3(ref line, initSTPos);								// Column 26, 27, 28: SceneTarget Pos
		AppendQuaternion(ref line, initSTRot);							// Column 29, 30, 31, 32: SceneTarget rot


		// 11. Hoop Initial Configuration
		AppendVector3(ref line, initHoopPos);							// Column 33, 34, 35: Hoop Pos
		AppendQuaternion(ref line, initHoopRot);						// Column 36, 37, 38, 39: Hoop rot


		// 12. Hoop error state
		if (hoop.IsRed()) {
			hoopRedFrames++;
			line.Append("1");
		}
		else {
			line.Append("0");
		}
		line.Append("\n");

		return line.ToString();
	}

	public string GetSummaryHeader() {
		StringBuilder line = new StringBuilder();

		line.Append("0. Trial num\t");

		line.Append("1. Init. SO Pos x\t");
		line.Append("2. Init. SO Pos y\t");
		line.Append("3. Init. SO Pos z\t");
		line.Append("4. Init. SO Rot x\t");
		line.Append("5. Init. SO Rot y\t");
		line.Append("6. Init. SO Rot z\t");
		line.Append("7. Init. SO Rot w\t");

		line.Append("8. Init. ST Pos x\t");
		line.Append("9. Init. ST Pos y\t");
		line.Append("10. Init. ST Pos z\t");
		line.Append("11. Init. ST Rot x\t");
		line.Append("12. Init. ST Rot y\t");
		line.Append("13. Init. ST Rot z\t");
		line.Append("14. Init. ST Rot w\t");

		line.Append("15. Init. Hoop Pos x\t");
		line.Append("16. Init. Hoop Pos y\t");
		line.Append("17. Init. Hoop Pos z\t");
		line.Append("18. Init. Hoop Rot x\t");
		line.Append("19. Init. Hoop Rot y\t");
		line.Append("20. Init. Hoop Rot z\t");
		line.Append("21. Init. Hoop Rot w\t");

		line.Append("22. Time to first grab\t");
		line.Append("23. Time to hoop\t");
		line.Append("24. Time to target\t");
		line.Append("25. Min dist to hoop\t");
		line.Append("26. Min dist to target\t");

		line.Append("27. Total frames hoop was red\t");

		line.Append("28. Target Forward Vector x\t");
		line.Append("29. Target Forward Vector y\t");
		line.Append("30. Target Forward Vector z\t");
		line.Append("31. Object Forward Vector x\t");
		line.Append("32. Object Forward Vector y\t");
		line.Append("33. Object Forward Vector z\t");
		line.Append("34. Angle between the two\t");

		line.Append("35. Target Up Vector x\t");
		line.Append("36. Target Up Vector y\t");
		line.Append("37. Target Up Vector z\t");
		line.Append("38. Object Up Vector x\t");
		line.Append("39. Object Up Vector y\t");
		line.Append("40. Object Up Vector z\t");
		line.Append("41. Angle between the two\t");

		line.Append("42. Maximal Angle difference\n");

		return line.ToString();
	}

	public string GetSummaryText() {
		StringBuilder line = new StringBuilder();
		line.Append(Session.instance.trial + "\t");

		line.Append(initSOPos.x + "\t");
		line.Append(initSOPos.y + "\t");
		line.Append(initSOPos.z + "\t");
		line.Append(initSORot.x + "\t");
		line.Append(initSORot.y + "\t");
		line.Append(initSORot.z + "\t");
		line.Append(initSORot.w + "\t");

		line.Append(initSTPos.x + "\t");
		line.Append(initSTPos.y + "\t");
		line.Append(initSTPos.z + "\t");
		line.Append(initSTRot.x + "\t");
		line.Append(initSTRot.y + "\t");
		line.Append(initSTRot.z + "\t");
		line.Append(initSTRot.w + "\t");

		line.Append(initHoopPos.x + "\t");
		line.Append(initHoopPos.y + "\t");
		line.Append(initHoopPos.z + "\t");
		line.Append(initHoopRot.x + "\t");
		line.Append(initHoopRot.y + "\t");
		line.Append(initHoopRot.z + "\t");
		line.Append(initHoopRot.w + "\t");

		line.Append(firstGrabTime + "\t");
		line.Append(minHoopTime + "\t");
		line.Append(minTargTime + "\t");
		line.Append(minHoopDist + "\t");
		line.Append(minTargDist + "\t");
		line.Append(hoopRedFrames + "\t");

		Vector3 f1 = sceneTarget.transform.forward;
		Vector3 f2 = sceneObject.transform.forward;
		float   fAngle = AngleDeviation(f1, f2);
		AppendVector3(ref line, f1);
		AppendVector3(ref line, f2);
		line.Append(fAngle + "\t");

		Vector3 u1 = sceneTarget.transform.up;
		Vector3 u2 = sceneObject.transform.up;
		float   uAngle = AngleDeviation(u1, u2);
		AppendVector3(ref line, u1);
		AppendVector3(ref line, u2);
		line.Append(uAngle + "\t");

		line.Append(Mathf.Max(fAngle, uAngle) + "\n");
		return line.ToString();
	}

	// Takes two vectors, and determines the angle deviation
	// given that orthogonality and parallelism are considered
	// isomorphic. This gives us the angle offset between the 
	// two cubes
	private float AngleDeviation(Vector3 a, Vector3 b) {
		float cosTheta  = Vector3.Dot(a, b); 
		float theta     = Mathf.Acos(cosTheta);
		float degrees   = theta * 180 / Mathf.PI;
		float deviation = ((degrees % 90) + 90) % 90;

		// Determine minimum distance from an orthogonal position
		deviation = Mathf.Min(deviation, 90 - deviation);
		return deviation;
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

		calibrateBox.TrialNum(Session.instance.trial);
	}

	public void CloseCalibrationBox() {
		if (calibrateBox != null) {
			Destroy(calibrateBox.gameObject);
		}

		BeginTrial();
	}

	public void BeginTrial() {
		start = DateTime.Now;

		// Create write out filestream for this trial.
		trialStream = File.AppendText(Session.instance.thisTrialPath);
		trialStream.Write(GetStateHeader());

		ResetMembers();

		LoadTrialComponents();

		sceneObject.Flash();
	}

	public void EndTrial() {
		running = false;

		// Close trial stream
		trialStream.Close();

		// Write summary for trial
		summaryStream = File.AppendText(Session.instance.thisDayPath + "/summary.tsv");
		if (Session.instance.trial == 0) {
			summaryStream.Write(GetSummaryHeader());
		}
		summaryStream.Write(GetSummaryText());
		summaryStream.Close();


		// If we did the last trial, finish session.
		if (Session.instance.trial == Session.totalTrials - 1) {
			// Close summary stream
			Debug.Log("Close Summary stream");
			// Write summary of entire session
	

			Session.instance.Home();
			return;
		}

		// Delete trial components
		DestroyTrialComponents();

		// Increment session count of trial
		Session.instance.IncrementTrial();

		// Load Calibration Box
		LoadCalibrationBox();
	}

	public void CheckFirstGrab() {
		if (!firstGrab) {
			firstGrab = true;
			firstGrabTime = (float) (DateTime.Now - start).TotalSeconds;

			sceneObject.DontFlash();
			hoop.Flash();
		}
	}

	public void CheckCompletion() {
		if (proximity && passedHoop) {
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
