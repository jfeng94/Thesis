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
		while (searchingDay) {
			string path = this.thisUserPath + "/Day" + day;
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
				searchingDay = false;
			}
			else {
				
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
