using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class DataAnalysis {
	List<UserData> Users = new List<UserData>();

	public DataAnalysis() {
		LoadUserData();
	}

	private void LoadUserData() {
		string path   = Session.instance.usersPath;
		string[] dirs = Directory.GetDirectories(path);
		for (int i = 0; i < dirs.Length; i++) {
			Debug.Log("User " + i + ": " + dirs[i]);
			UserData ud = new UserData(dirs[i]);
		}
	}
}

public class UserData {
	List<DayData> Days = new List<DayData>();
	List<SummaryData> summaries = new List<SummaryData>();

	public UserData(string path) {
		// Navigate to path
		for (int i = 0; i < 5; i++) {
			string      dayPath = path + "/Day" + i;
			SummaryData summary = new SummaryData(dayPath + "/summary.tsv");
			Debug.Log("Day " + i + ":\n" + summary.ToString());
		}
	}
}

public class DayData {
	private List<SummaryData> summaries = new List<SummaryData>();

	public DayData(string path) {

	}
}

public class SummaryData {
	public Vector3 oPos = new Vector3(0f, 0f, 0f); // object position
	public Vector3 hPos = new Vector3(0f, 0f, 0f); // hoop position
	public Vector3 tPos = new Vector3(0f, 0f, 0f); // target position

	public float minHDist = 0f; // minimum distance to hoop
	public float minTDist = 0f; // minimum distance to target

	public float oTime = 0f; // time to grab object
	public float hTime = 0f; // time to place in hoop
	public float tTime = 0f; // time to place in target

	public float framesRed = 0f; // Number of frames where the hoop was red

	public float fDivergence = 0f;
	public float uDivergence = 0f;

	public SummaryData(string path) {
		string[] lines;
		
		// Read summary text
		try {
			lines = File.ReadAllLines(path);
		}
		catch(FileNotFoundException) {
			Debug.Log("File " + path + " not found...");
			return;
		}

		// Parse lines
		// Starts at i = 1 because i = 0 is the header
		for (int i = 1; i < lines.Length; i++) {
			string[] cells = lines[i].Split('\t');

			try {
				int trialNum = Int32.Parse(cells[0]);

				// Scene Object properties
				float oPosX    = Single.Parse(cells[1]);
				float oPosY    = Single.Parse(cells[2]);
				float oPosZ    = Single.Parse(cells[3]);
				float oRotX    = Single.Parse(cells[4]);
				float oRotY    = Single.Parse(cells[5]);
				float oRotZ    = Single.Parse(cells[6]);
				float oRotW    = Single.Parse(cells[7]);

				// Scene Target properties
				float tPosX    = Single.Parse(cells[8]);
				float tPosY    = Single.Parse(cells[9]);
				float tPosZ    = Single.Parse(cells[10]);
				float tRotX    = Single.Parse(cells[11]);
				float tRotY    = Single.Parse(cells[12]);
				float tRotZ    = Single.Parse(cells[13]);
				float tRotW    = Single.Parse(cells[14]);

				// Hoop properties
				float hPosX    = Single.Parse(cells[15]);
				float hPosY    = Single.Parse(cells[16]);
				float hPosZ    = Single.Parse(cells[17]);
				float hRotX    = Single.Parse(cells[18]);
				float hRotY    = Single.Parse(cells[19]);
				float hRotZ    = Single.Parse(cells[20]);
				float hRotW    = Single.Parse(cells[21]);

				// Important times
				float timeToGrab = Single.Parse(cells[22]);
				float timeToHoop = Single.Parse(cells[23]);
				float timeToTarg = Single.Parse(cells[24]);

				// Distances
				float distToHoop = Single.Parse(cells[25]);
				float distToTarg = Single.Parse(cells[26]);

				// Red Frames
				float red = Single.Parse(cells[27]);

				// Forward vector stuff
				float oForwardX = Single.Parse(cells[28]);
				float oForwardY = Single.Parse(cells[29]);
				float oForwardZ = Single.Parse(cells[30]);
				float tForwardX = Single.Parse(cells[31]);
				float tForwardY = Single.Parse(cells[32]);
				float tForwardZ = Single.Parse(cells[33]);
				float fAngle    = Single.Parse(cells[34]);

				// Up vector stuff
				float oUpX   = Single.Parse(cells[35]);
				float oUpY   = Single.Parse(cells[36]);
				float oUpZ   = Single.Parse(cells[37]);
				float tUpX   = Single.Parse(cells[38]);
				float tUpY   = Single.Parse(cells[39]);
				float tUpZ   = Single.Parse(cells[40]);
				float uAngle = Single.Parse(cells[41]);

				// Max angle
				float mAngle = Single.Parse(cells[42]);





				// SET INTERNAL VALUES
				//oPos += new Vector3(oPosX, oPosY, oPosZ);
				//hPos += new Vector3(hPosX, hPosY, hPosZ);
				//tPos += new Vector3(tPosX, tPosY, tPosZ);

				oTime += timeToGrab;
				hTime += timeToHoop;
				tTime += timeToTarg;

				minHDist += distToHoop;
				minTDist += distToTarg;

				framesRed += red;

				fDivergence += fAngle;
				uDivergence += uAngle;
			}
			catch {
				Debug.Log("durr");
			}
		}
		// Get average
		//oPos /= (lines.Length - 1);
		//hPos /= (lines.Length - 1);
		//tPos /= (lines.Length - 1);
		
		oTime /= (lines.Length - 1);
		hTime /= (lines.Length - 1);
		tTime /= (lines.Length - 1);

		minHDist /= (lines.Length - 1);
		minTDist /= (lines.Length - 1);

		framesRed /= (lines.Length - 1);

		fDivergence /= (lines.Length - 1);
		uDivergence /= (lines.Length - 1);
	}

	public string ToString() {
		StringBuilder sb = new StringBuilder();

		sb.Append("Average time to Object:\t" + oTime + "\n");
		sb.Append("Average time to Hoop:\t" + hTime + "\n");
		sb.Append("Average time to Target:\t" + tTime + "\n");
		sb.Append("Average distance to Hoop:\t" + minHDist + "\n");
		sb.Append("Average distance to Target:\t" + minTDist + "\n");
		sb.Append("Average red frames:\t" + framesRed + "\n");
		sb.Append("Average forward divergence:\t" + fDivergence + "\n");
		sb.Append("Average upward divergence:\t" + uDivergence + "\n");

		return sb.ToString();
	}
}