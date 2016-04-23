using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class DataAnalysis {
	List<UserData> Users = new List<UserData>();

	public DataAnalysis() {
		LoadUserData();
	}

	private void LoadUserData() {
		return;
	}
}

public class UserData {
	List<DayData> Days = new List<DayData>();

	public UserData(string path) {
		// Navigate to path

	}
}

public class DayData {
	private List<SummaryData> summaries = new List<SummaryData>();

	public DayData() {

	}
}

public class SummaryData {
	public Vector3 oPos; // object position
	public Vector3 hPos; // hoop position
	public Vector3 tPos; // target position

	public float minHDist; // minimum distance to hoop
	public float minTDist; // minimum distance to target

	public float oTime; // time to grab object
	public float hTime; // time to place in hoop
	public float tTime; // time to place in target

	public float framesRed; // Number of frames where the hoop was red

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
		for (int i = 0; i < lines.Length. i++) {
			string[] cells = lines[i].Split("\t");

			try {
				int   trialNum =  Int32.Parse(cells[0]);

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
		}
	}
}