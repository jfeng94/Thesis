using UnityEngine;
using System.Collections;

public class Session : MonoSingleton<Session> {
	public string user  = null;
	public int    day   = -1;
	public int    trial = -1;

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

	public void Home() {
		Application.LoadLevel("Home");
	}

	public void NewUser() {
		Application.LoadLevel("NewUser");
	}

	public void ViewUser() {
		Application.LoadLevel("Trial");
		//Application.LoadLevel("ViewUser");
	}

	public void Trial() {
		Application.LoadLevel("Trial");
	}
}
