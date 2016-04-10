using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class Session : MonoSingleton<Session> {
	//////////////////////////////////////////
	// System preferences
	//////////////////////////////////////////
	public static int totalDays {
		get { return MAXDAYS; }
	}

	public static int totalTrials {
		get { return MAXTRIALS; }
	}

	private static int MAXDAYS = 5;
	private static int MAXTRIALS = 20;

	public string user   = null;
	public int    day    = 0;
	public int    trial  = 0;
	public bool   user3D = false;

	// Use this for initialization
	void Start () {
		// This session manager controls everything. Should not disappear on
		// new loaded levels.
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public string basePath {
		get {
			return Application.persistentDataPath;
		}
	}

	public string usersPath {
		get {
			return basePath + "/Users";
		}
	}

	public string thisUserPath {
		get {
			if (user != null) {
				return usersPath + "/" + user;
			}
			return null;
		}
	}

	public string thisDayPath {
		get {
			if (user != null) {
				return thisUserPath + "/Day" + day;
			}
			return null;
		}
	}

	public string thisTrialPath {
		get {
			if (user != null) {
				return thisDayPath + "/Trial_" + trial + ".txt";
			}
			return null;
		}
	}

	public void SetUser(string s) {
		user = s;
		day  = 0;
		while (day < totalDays) {
			// Start at trial 0
			trial = 0;

			// If this day does not exist, we're on this day, trial 0;
			if (!Directory.Exists(thisDayPath)) {
				Debug.Log(thisDayPath + "does not exist yet! Creating...");
				Directory.CreateDirectory(thisDayPath);
				OpenUserInfo();
				return;
			}
			else {
				while (trial < totalTrials) {
					if (!File.Exists(thisTrialPath)) {
						Debug.Log("Trial " + thisTrialPath + " does not exist!");
						OpenUserInfo();
						return;
					}
					trial++;
				}
			}
			day++;
		}
	}


	private void OpenUserInfo() {
		// Open user data file.
		string path = thisUserPath + "/info.txt";
		string[] lines;

		try {
			lines = File.ReadAllLines(path);
		}
		catch(FileNotFoundException) {
			Debug.Log("user info.txt not found!!!");
			return;
		}

		// Check what kind of trial this user is doing
		string[] cells = lines[1].Split('\t');
		int use3D = Int32.Parse(cells[1]);
		Debug.Log(lines[1]);
		Debug.Log(user3D);
		if (use3D == 1) {
			user3D = true;
		}
		else {
			user3D = false;
		}
	}


	public void IncrementTrial() {
		trial++;
	}

	public void Home() {
		user = null;
		trial = 0;
		day = 0;
		Application.LoadLevel("Home");
	}

	public void NewUser() {
		Application.LoadLevel("NewUser");
	}

	public void ViewUser() {
		Application.LoadLevel("ViewUser");
	}

	public void Trial() {
		Application.LoadLevel("Trial");
	}

	public void Practice() {
		Application.LoadLevel("Practice");
	}
}
