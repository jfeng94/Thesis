using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class DataAnalysis {
	List<UserData> users = new List<UserData>();
	string basePath = Application.persistentDataPath + "/Data";

	public DataAnalysis() {
		LoadUserData();
		PrintData();
	}

	private void LoadUserData() {
		string path   = Session.instance.usersPath;
		string[] dirs = Directory.GetDirectories(path);
		for (int i = 0; i < dirs.Length; i++) {
			UserData ud = new UserData(dirs[i]);

			users.Add(ud);
		}
	}

	private void PrintData() {
		if (!Directory.Exists(basePath)) {
			Directory.CreateDirectory(basePath);
		}

		PrintGrabTime();
		PrintHoopTime();
		PrintTargTime();

		PrintHoopDist();
		PrintTargDist();

		PrintRedFrames();

		PrintAngularDivergence();

	}

	private void PrintGrabTime() {
		string path3D   = basePath + "/GrabTime3D.tsv";
		string path2D   = basePath + "/GrabTime2D.tsv";
		string pathComp = basePath + "/GrabTimeComp.tsv";

		int num3D = 0;
		int num2D = 0;

		float[] sum3D = new float[5];
		float[] sum2D = new float[5];

		float[] avg3D = new float[5];
		float[] avg2D = new float[5];

		StringBuilder str3D   = new StringBuilder();
		StringBuilder str2D   = new StringBuilder();
		StringBuilder strComp = new StringBuilder();

		for (int i = 0; i < users.Count; i++) {
			if (users[i].is3D) {
				num3D++;

				str3D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float grabTime = users[i].sessions[j].oTime;
					sum3D[j] += grabTime;
					str3D.Append(grabTime + "\t");
				}
				str3D.Append("\n");
			}
			else {
				num2D++;

				str2D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float grabTime = users[i].sessions[j].oTime;
					sum2D[j] += grabTime;
					str2D.Append(grabTime + "\t");
				}
				str2D.Append("\n");

			}
		}

		for (int i = 0; i < 5; i++) {
			avg2D[i] = sum2D[i] / num2D;
			avg3D[i] = sum3D[i] / num3D;
		}

		// Append comparisons
		strComp.Append("2D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg2D[i] + "\t");
		}
		strComp.Append("\n3D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg3D[i] + "\t");
		}

		// Create new files.
		File.WriteAllText(path3D,   str3D.ToString());
		File.WriteAllText(path2D,   str2D.ToString());
		File.WriteAllText(pathComp, strComp.ToString());
	}


	private void PrintHoopTime() {
		string path3D   = basePath + "/HoopTime3D.tsv";
		string path2D   = basePath + "/HoopTime2D.tsv";
		string pathComp = basePath + "/HoopTimeComp.tsv";

		int num3D = 0;
		int num2D = 0;

		float[] sum3D = new float[5];
		float[] sum2D = new float[5];

		float[] avg3D = new float[5];
		float[] avg2D = new float[5];

		StringBuilder str3D   = new StringBuilder();
		StringBuilder str2D   = new StringBuilder();
		StringBuilder strComp = new StringBuilder();

		for (int i = 0; i < users.Count; i++) {
			if (users[i].is3D) {
				num3D++;

				str3D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].hTime;
					sum3D[j] += val;
					str3D.Append(val + "\t");
				}
				str3D.Append("\n");
			}
			else {
				num2D++;

				str2D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].hTime;
					sum2D[j] += val;
					str2D.Append(val + "\t");
				}
				str2D.Append("\n");

			}
		}

		for (int i = 0; i < 5; i++) {
			avg2D[i] = sum2D[i] / num2D;
			avg3D[i] = sum3D[i] / num3D;
		}

		// Append comparisons
		strComp.Append("2D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg2D[i] + "\t");
		}
		strComp.Append("\n3D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg3D[i] + "\t");
		}

		// Create new files.
		File.WriteAllText(path3D,   str3D.ToString());
		File.WriteAllText(path2D,   str2D.ToString());
		File.WriteAllText(pathComp, strComp.ToString());
	}

	private void PrintTargTime() {
		string path3D   = basePath + "/TargTime3D.tsv";
		string path2D   = basePath + "/TargTime2D.tsv";
		string pathComp = basePath + "/TargTimeComp.tsv";

		int num3D = 0;
		int num2D = 0;

		float[] sum3D = new float[5];
		float[] sum2D = new float[5];

		float[] avg3D = new float[5];
		float[] avg2D = new float[5];

		StringBuilder str3D   = new StringBuilder();
		StringBuilder str2D   = new StringBuilder();
		StringBuilder strComp = new StringBuilder();

		for (int i = 0; i < users.Count; i++) {
			if (users[i].is3D) {
				num3D++;

				str3D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].tTime;
					sum3D[j] += val;
					str3D.Append(val + "\t");
				}
				str3D.Append("\n");
			}
			else {
				num2D++;

				str2D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].tTime;
					sum2D[j] += val;
					str2D.Append(val + "\t");
				}
				str2D.Append("\n");

			}
		}

		for (int i = 0; i < 5; i++) {
			avg2D[i] = sum2D[i] / num2D;
			avg3D[i] = sum3D[i] / num3D;
		}

		// Append comparisons
		strComp.Append("2D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg2D[i] + "\t");
		}
		strComp.Append("\n3D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg3D[i] + "\t");
		}

		// Create new files.
		File.WriteAllText(path3D,   str3D.ToString());
		File.WriteAllText(path2D,   str2D.ToString());
		File.WriteAllText(pathComp, strComp.ToString());
	}


	private void PrintHoopDist() {
		string path3D   = basePath + "/HoopDist3D.tsv";
		string path2D   = basePath + "/HoopDist2D.tsv";
		string pathComp = basePath + "/HoopDistComp.tsv";

		int num3D = 0;
		int num2D = 0;

		float[] sum3D = new float[5];
		float[] sum2D = new float[5];

		float[] avg3D = new float[5];
		float[] avg2D = new float[5];

		StringBuilder str3D   = new StringBuilder();
		StringBuilder str2D   = new StringBuilder();
		StringBuilder strComp = new StringBuilder();

		for (int i = 0; i < users.Count; i++) {
			if (users[i].is3D) {
				num3D++;

				str3D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].hDist;
					sum3D[j] += val;
					str3D.Append(val + "\t");
				}
				str3D.Append("\n");
			}
			else {
				num2D++;

				str2D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].hDist;
					sum2D[j] += val;
					str2D.Append(val + "\t");
				}
				str2D.Append("\n");

			}
		}

		for (int i = 0; i < 5; i++) {
			avg2D[i] = sum2D[i] / num2D;
			avg3D[i] = sum3D[i] / num3D;
		}

		// Append comparisons
		strComp.Append("2D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg2D[i] + "\t");
		}
		strComp.Append("\n3D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg3D[i] + "\t");
		}

		// Create new files.
		File.WriteAllText(path3D,   str3D.ToString());
		File.WriteAllText(path2D,   str2D.ToString());
		File.WriteAllText(pathComp, strComp.ToString());
	}

	private void PrintTargDist() {
		string path3D   = basePath + "/TargDist3D.tsv";
		string path2D   = basePath + "/TargDist2D.tsv";
		string pathComp = basePath + "/TargDistComp.tsv";

		int num3D = 0;
		int num2D = 0;

		float[] sum3D = new float[5];
		float[] sum2D = new float[5];

		float[] avg3D = new float[5];
		float[] avg2D = new float[5];

		StringBuilder str3D   = new StringBuilder();
		StringBuilder str2D   = new StringBuilder();
		StringBuilder strComp = new StringBuilder();

		for (int i = 0; i < users.Count; i++) {
			if (users[i].is3D) {
				num3D++;

				str3D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].tDist;
					sum3D[j] += val;
					str3D.Append(val + "\t");
				}
				str3D.Append("\n");
			}
			else {
				num2D++;

				str2D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].tDist;
					sum2D[j] += val;
					str2D.Append(val + "\t");
				}
				str2D.Append("\n");

			}
		}

		for (int i = 0; i < 5; i++) {
			avg2D[i] = sum2D[i] / num2D;
			avg3D[i] = sum3D[i] / num3D;
		}

		// Append comparisons
		strComp.Append("2D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg2D[i] + "\t");
		}
		strComp.Append("\n3D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg3D[i] + "\t");
		}

		// Create new files.
		File.WriteAllText(path3D,   str3D.ToString());
		File.WriteAllText(path2D,   str2D.ToString());
		File.WriteAllText(pathComp, strComp.ToString());
	}

	private void PrintRedFrames() {
		string path3D   = basePath + "/RedFrames3D.tsv";
		string path2D   = basePath + "/RedFrames2D.tsv";
		string pathComp = basePath + "/RedFramesComp.tsv";

		int num3D = 0;
		int num2D = 0;

		float[] sum3D = new float[5];
		float[] sum2D = new float[5];

		float[] avg3D = new float[5];
		float[] avg2D = new float[5];

		StringBuilder str3D   = new StringBuilder();
		StringBuilder str2D   = new StringBuilder();
		StringBuilder strComp = new StringBuilder();

		for (int i = 0; i < users.Count; i++) {
			if (users[i].is3D) {
				num3D++;

				str3D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].framesRed;
					sum3D[j] += val;
					str3D.Append(val + "\t");
				}
				str3D.Append("\n");
			}
			else {
				num2D++;

				str2D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].framesRed;
					sum2D[j] += val;
					str2D.Append(val + "\t");
				}
				str2D.Append("\n");

			}
		}

		for (int i = 0; i < 5; i++) {
			avg2D[i] = sum2D[i] / num2D;
			avg3D[i] = sum3D[i] / num3D;
		}

		// Append comparisons
		strComp.Append("2D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg2D[i] + "\t");
		}
		strComp.Append("\n3D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg3D[i] + "\t");
		}

		// Create new files.
		File.WriteAllText(path3D,   str3D.ToString());
		File.WriteAllText(path2D,   str2D.ToString());
		File.WriteAllText(pathComp, strComp.ToString());
	}

	private void PrintAngularDivergence() {
		PrintForwardAngDiv();
		PrintUpwardAngDiv();
	}

	private void PrintForwardAngDiv() {
		string path3D   = basePath + "/ForwardAngDiv3D.tsv";
		string path2D   = basePath + "/ForwardAngDiv2D.tsv";
		string pathComp = basePath + "/ForwardAngDivComp.tsv";

		int num3D = 0;
		int num2D = 0;

		float[] sum3D = new float[5];
		float[] sum2D = new float[5];

		float[] avg3D = new float[5];
		float[] avg2D = new float[5];

		StringBuilder str3D   = new StringBuilder();
		StringBuilder str2D   = new StringBuilder();
		StringBuilder strComp = new StringBuilder();

		for (int i = 0; i < users.Count; i++) {
			if (users[i].is3D) {
				num3D++;

				str3D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].fDivergence;
					sum3D[j] += val;
					str3D.Append(val + "\t");
				}
				str3D.Append("\n");
			}
			else {
				num2D++;

				str2D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].fDivergence;
					sum2D[j] += val;
					str2D.Append(val + "\t");
				}
				str2D.Append("\n");

			}
		}

		for (int i = 0; i < 5; i++) {
			avg2D[i] = sum2D[i] / num2D;
			avg3D[i] = sum3D[i] / num3D;
		}

		// Append comparisons
		strComp.Append("2D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg2D[i] + "\t");
		}
		strComp.Append("\n3D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg3D[i] + "\t");
		}

		// Create new files.
		File.WriteAllText(path3D,   str3D.ToString());
		File.WriteAllText(path2D,   str2D.ToString());
		File.WriteAllText(pathComp, strComp.ToString());
	}

	private void PrintUpwardAngDiv() {
		string path3D   = basePath + "/UpwardAngDiv3D.tsv";
		string path2D   = basePath + "/UpwardAngDiv2D.tsv";
		string pathComp = basePath + "/UpwardAngDivComp.tsv";

		int num3D = 0;
		int num2D = 0;

		float[] sum3D = new float[5];
		float[] sum2D = new float[5];

		float[] avg3D = new float[5];
		float[] avg2D = new float[5];

		StringBuilder str3D   = new StringBuilder();
		StringBuilder str2D   = new StringBuilder();
		StringBuilder strComp = new StringBuilder();

		for (int i = 0; i < users.Count; i++) {
			if (users[i].is3D) {
				num3D++;

				str3D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].uDivergence;
					sum3D[j] += val;
					str3D.Append(val + "\t");
				}
				str3D.Append("\n");
			}
			else {
				num2D++;

				str2D.Append(users[i].name + "\t");
				for (int j = 0; j < 5; j++) {
					float val = users[i].sessions[j].uDivergence;
					sum2D[j] += val;
					str2D.Append(val + "\t");
				}
				str2D.Append("\n");

			}
		}

		for (int i = 0; i < 5; i++) {
			avg2D[i] = sum2D[i] / num2D;
			avg3D[i] = sum3D[i] / num3D;
		}

		// Append comparisons
		strComp.Append("2D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg2D[i] + "\t");
		}
		strComp.Append("\n3D\t");
		for (int i = 0; i < 5; i++) {
			strComp.Append(avg3D[i] + "\t");
		}

		// Create new files.
		File.WriteAllText(path3D,   str3D.ToString());
		File.WriteAllText(path2D,   str2D.ToString());
		File.WriteAllText(pathComp, strComp.ToString());
	}

}

public class UserData {
	public string name;
	public bool is3D = false;
	public List<SessionData> sessions = new List<SessionData>();

	public UserData(string path) {
		// Navigate to path
		for (int i = 0; i < 5; i++) {
			string      dayPath = path + "/Day" + i;
			SessionData session = new SessionData(dayPath);
			
			if (session.initialized) {
				sessions.Add(session);
			}
		}

		string infoPath = path + "/info.txt";

		string[] lines;
		try {
			lines = File.ReadAllLines(infoPath);
		}
		catch(FileNotFoundException) {
			Debug.Log("user info.txt not found!!!");
			return;
		}

		// Check what kind of trial this user is doing
		string[] cells = lines[1].Split('\t');
		int use3D = Int32.Parse(cells[1]);
		if (use3D == 1) {
			is3D = true;
		}
		else {
			is3D = false;
		}

		name = (lines[0].Split('\t'))[1];

	}
}


public class SessionData {
	public bool initialized = false;

	public List<TrialData> trials = new List<TrialData>();

	public float hDist = 0f; // minimum distance to hoop
	public float tDist = 0f; // minimum distance to target

	public float oTime = 0f; // time to grab object
	public float hTime = 0f; // time to place in hoop
	public float tTime = 0f; // time to place in target

	public float framesRed = 0f; // Number of frames where the hoop was red

	public float fDivergence = 0f;
	public float uDivergence = 0f;

	public SessionData(string path) {


		string[] lines;
		// Read summary text
		try {
			lines = File.ReadAllLines(path + "/summary.tsv");
		}
		catch(FileNotFoundException) {
			Debug.Log("File " + path + " not found...");
			return;
		}

		// Parse lines
		// Starts at i = 1 because i = 0 is the header
		for (int i = 1; i < lines.Length; i++) {
			TrialData td = new TrialData(lines[i], path);
			if (td.initialized) {
				trials.Add(td);
			}
		}

		for (int i = 0; i < trials.Count; i++) {
			tDist += trials[i].distToTarg;
			hDist += trials[i].lastHoopDistance;

			oTime += trials[i].timeToGrab;
			hTime += (trials[i].lastHoopAttempt - trials[i].timeToGrab);
			tTime += (trials[i].timeToTarg - trials[i].lastHoopAttempt);

			framesRed += trials[i].red;

			fDivergence += trials[i].fAngle;
			uDivergence += trials[i].uAngle;
		}

		tDist /= trials.Count;
		hDist /= trials.Count;
		oTime /= trials.Count;
		hTime /= trials.Count;
		tTime /= trials.Count;
		framesRed /= trials.Count;
		fDivergence /= trials.Count;
		uDivergence /= trials.Count;

		initialized = true;

	}

	public string ToString() {
		StringBuilder sb = new StringBuilder();

		sb.Append("Average time to Object:\t" + oTime + "\n");
		sb.Append("Average time to Hoop:\t" + hTime + "\n");
		sb.Append("Average time to Target:\t" + tTime + "\n");
		sb.Append("Average distance to Hoop:\t" + hDist + "\n");
		sb.Append("Average distance to Target:\t" + tDist + "\n");
		sb.Append("Average red frames:\t" + framesRed + "\n");
		sb.Append("Average forward divergence:\t" + fDivergence + "\n");
		sb.Append("Average upward divergence:\t" + uDivergence + "\n");

		return sb.ToString();
	}
}

public class TrialData {
	public bool initialized = false;

	public int trialNum;

	// Scene Object initial configuration
	public Vector3    oPos;
	public Quaternion oRot;

	// Scene Target properties
	public Vector3    tPos;
	public Quaternion tRot;

	// Hoop properties
	public Vector3    hPos;
	public Quaternion hRot;

	// Important times
	public float timeToGrab;
	public float timeToHoop;
	public float timeToTarg;

	// Distances
	public float distToHoop;
	public float distToTarg;

	// Red Frames
	public float red;

	// Forward vector stuff
	public Vector3 oForward;
	public Vector3 tForward;
	public float fAngle;

	// Up vector stuff
	public Vector3 oUp;
	public Vector3 tUp;
	public float uAngle;

	// Max angle
	public float mAngle;

	public Vector3 initHSPos;
	public float   lastHoopAttempt;
	public float   lastHoopDistance;




	public List<TrialState> states = new List<TrialState>();

	// Private empty constructor
	private TrialData() {
		return;
	}

	public TrialData(string line, string path) {

		///////////////////////////////////////////////////////////////////////
		// PARSE TRIAL SUMMARY LINE
		///////////////////////////////////////////////////////////////////////
		string[] cells = line.Split('\t');
		try {
			trialNum = Int32.Parse(cells[0]);
	
			// Scene Object properties
			float oPosX = Single.Parse(cells[1]);
			float oPosY = Single.Parse(cells[2]);
			float oPosZ = Single.Parse(cells[3]);
			float oRotX = Single.Parse(cells[4]);
			float oRotY = Single.Parse(cells[5]);
			float oRotZ = Single.Parse(cells[6]);
			float oRotW = Single.Parse(cells[7]);
			oPos = new    Vector3(oPosX, oPosY, oPosZ);
			oRot = new Quaternion(oRotX, oRotY, oRotZ, oRotW);
	
			// Scene Target properties
			float tPosX = Single.Parse(cells[ 8]);
			float tPosY = Single.Parse(cells[ 9]);
			float tPosZ = Single.Parse(cells[10]);
			float tRotX = Single.Parse(cells[11]);
			float tRotY = Single.Parse(cells[12]);
			float tRotZ = Single.Parse(cells[13]);
			float tRotW = Single.Parse(cells[14]);
			tPos = new    Vector3(tPosX, tPosY, tPosZ);
			tRot = new Quaternion(tRotX, tRotY, tRotZ, tRotW);
	
			// Hoop properties
			float hPosX = Single.Parse(cells[15]);
			float hPosY = Single.Parse(cells[16]);
			float hPosZ = Single.Parse(cells[17]);
			float hRotX = Single.Parse(cells[18]);
			float hRotY = Single.Parse(cells[19]);
			float hRotZ = Single.Parse(cells[20]);
			float hRotW = Single.Parse(cells[21]);
			hPos = new    Vector3(hPosX, hPosY, hPosZ);
			hRot = new Quaternion(hRotX, hRotY, hRotZ, hRotW);
	
			// Important times
			timeToGrab = Single.Parse(cells[22]);
			timeToHoop = Single.Parse(cells[23]);
			timeToTarg = Single.Parse(cells[24]);
	
			// Distances
			distToHoop = Single.Parse(cells[25]);
			distToTarg = Single.Parse(cells[26]);
	
			// Red Frames
			red = Single.Parse(cells[27]);
	
			// Forward vector stuff
			float oForwardX = Single.Parse(cells[28]);
			float oForwardY = Single.Parse(cells[29]);
			float oForwardZ = Single.Parse(cells[30]);
			float tForwardX = Single.Parse(cells[31]);
			float tForwardY = Single.Parse(cells[32]);
			float tForwardZ = Single.Parse(cells[33]);
			fAngle          = Single.Parse(cells[34]);
			oForward = new Vector3(oForwardX, oForwardY, oForwardZ);
			tForward = new Vector3(tForwardX, tForwardY, tForwardZ);

			// Up vector stuff
			float oUpX   = Single.Parse(cells[35]);
			float oUpY   = Single.Parse(cells[36]);
			float oUpZ   = Single.Parse(cells[37]);
			float tUpX   = Single.Parse(cells[38]);
			float tUpY   = Single.Parse(cells[39]);
			float tUpZ   = Single.Parse(cells[40]);
			uAngle       = Single.Parse(cells[41]);
			oUp = new Vector3(oUpX, oUpY, oUpZ);
			tUp = new Vector3(tUpX, tUpY, tUpZ);
	
			// Max angle
			mAngle = Single.Parse(cells[42]);
		}
		catch {
			Debug.Log("Line ignored: " + path + "\n" + line);
			initialized = false;
			return;
		}

		///////////////////////////////////////////////////////////////////////
		// PARSE RAW TRIAL DATA
		///////////////////////////////////////////////////////////////////////
		string[] lines;
		string trialPath = path + "/Trial_" + trialNum + ".tsv";
		try {
			lines = File.ReadAllLines(trialPath);

			for (int i = 1; i < lines.Length; i++) {
				TrialState s = new TrialState(lines[i]);
				if (s.initialized) {
					states.Add(s);
				}
			}
		}
		catch {
			Debug.Log("File not found:\n" + trialPath);
			initialized = false;
		}

		///////////////////////////////////////////////////////////////////////
		// GET INITIAL HANDSTREAM POSITION
		///////////////////////////////////////////////////////////////////////
		initHSPos = states[0].hsPos;


		///////////////////////////////////////////////////////////////////////
		// LOOK FOR LAST HOOP ATTEMPT (FINAL LOCAL MINIMA)
		///////////////////////////////////////////////////////////////////////	
		for (int i = states.Count - 2; i >= 1; i--) {
			float d0 = Vector3.Distance(states[i - 1].oPos, hPos);
			float d1 = Vector3.Distance(states[i    ].oPos, hPos);
			float d2 = Vector3.Distance(states[i + 1].oPos, hPos);
			if (d1 <= d0 && d1 <= d2 && d1 < 3f) {
				lastHoopAttempt  = states[i].time;
				lastHoopDistance = d1; 
				break;
			}
		}

		initialized = true;
	}
}

public class TrialState {
	public bool initialized;

	public int   frame;

	public Vector3    hsPos;
	public Quaternion hsRot;

	public Vector3    oPos;
	public Quaternion oRot;

	public int   triggerStatus;

	public float time;

	public TrialState(string line) {
		// Try to parse line
		string[] cells = line.Split('\t');
		try {
			frame         =  Int32.Parse(cells[ 0]);

			float hsPosX        = Single.Parse(cells[ 1]);
			float hsPosY        = Single.Parse(cells[ 2]);
			float hsPosZ        = Single.Parse(cells[ 3]);
			float hsRotX        = Single.Parse(cells[ 4]);
			float hsRotY        = Single.Parse(cells[ 5]);
			float hsRotZ        = Single.Parse(cells[ 6]);
			float hsRotW        = Single.Parse(cells[ 7]);
			hsPos = new    Vector3(hsPosX, hsPosY, hsPosZ);
			hsRot = new Quaternion(hsRotX, hsRotY, hsRotZ, hsRotW);

			triggerStatus =  Int32.Parse(cells[ 8]);

			float oPosX         = Single.Parse(cells[ 9]);
			float oPosY         = Single.Parse(cells[10]);
			float oPosZ         = Single.Parse(cells[11]);
			float oRotX         = Single.Parse(cells[12]);
			float oRotY         = Single.Parse(cells[13]);
			float oRotZ         = Single.Parse(cells[14]);
			float oRotW         = Single.Parse(cells[15]);
			oPos = new    Vector3(oPosX, oPosY, oPosZ);
			oRot = new Quaternion(oRotX, oRotY, oRotZ, oRotW);

			time = Single.Parse(cells[18]);
		}
		catch {
			Debug.Log("Line ignored:\n" + line);
		}
		initialized = true;
	}
}