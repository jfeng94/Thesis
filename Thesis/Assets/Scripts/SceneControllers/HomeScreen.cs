using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using System.IO;
using System.Collections;
using System.Collections.Generic;


public class HomeScreen : MonoBehaviour {
	Dropdown userList  = null;
	List<string> users = new List<string>();

	void Start() {
		VRSettings.enabled = false;

		Dropdown dd = FindObjectOfType(typeof(Dropdown)) as Dropdown;
		if (dd != null) {
			userList = dd;
		}

		PopulateUserList();
	}

	private void PopulateUserList() {
		// Get Users directory
		string path        = Session.instance.usersPath;
		string[] userPaths = null;
		if (Directory.Exists(path)) {
			userPaths = Directory.GetDirectories(path + "/");
		}
		
		users = new List<string>();

		// Get List of Users
		if (userPaths != null) {
			for (int i = 0; i < userPaths.Length; i++) {
				string[] substrings = userPaths[i].Split('/');
				users.Add(substrings[substrings.Length - 1]);
			}
		}

		// Populate Dropdown
		userList.options.Clear();
		foreach (string user in users) {
			userList.options.Add(new Dropdown.OptionData(user));
		}
		userList.value = 0;
		userList.captionText = userList.captionText;
		
	}



	public void CreateNewUser() {
		Session.instance.NewUser();
	}

	public void ViewUser() {
		Session.instance.SetUser(userList.options[userList.value].text);
		Session.instance.ViewUser();
	}

	public void Practice() {
		Session.instance.Practice();
	}

	public void Exit() {
		Session.instance.Exit();
	}
}
