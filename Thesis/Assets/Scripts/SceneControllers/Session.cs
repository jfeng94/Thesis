using UnityEngine;
using System.IO;
using System.Collections;

public class Session : MonoSingleton<Session> {
	//////////////////////////////////////////
	// System preferences
	//////////////////////////////////////////
	public int    totalDays = 5;
	public int    totalTrials = 20;

	public string user  = null;
	public int    day   = 0;
	public int    trial = 0;

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
			return this.basePath + "/Users";
		}
	}

	public string thisUserPath {
		get {
			if (user != null) {
				return this.usersPath + "/" + user;
			}
			return null;
		}
	}

	public void SetUser(string s) {
		user = s;


		// Figure out how many days this user has already participated in.
		bool searchingDay = true;
		while (day <= totalDays) {
			// Current day
			string path = this.thisUserPath + "/Day" + day;

			// Previous day's trial 20
			string prev = this.thisUserPath + "/Day" + (day - 1) + "/Trial_20.txt";

			// If this day's folder doesn't exist, but the previous day is completed
			// (there is a trial 20)
			if (!Directory.Exists(path) && !File.Exists(prev)) {
				Directory.CreateDirectory(path);

				return;

			}
			// Else if the directory already exists, figure out what the last trial was.
			else if (Directory.Exists(path)) {
				// Cycle through all possible 
				while (trial < totalTrials) {
					string trialPath = this.thisUserPath + "/Day" + day + "/Trial_" + trial + ".txt";
					if (!File.Exists(trialPath)) {
						
						return;
					}

					trial++;
				}
			}

			day++;
		}
	}

	public void Home() {
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
}
