using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class NewUserScreen : MonoBehaviour {
	// UI Elements
	private InputField nameField     = null;
	private Dropdown   trialType     = null;
	private Dropdown   handedness    = null;
	private Slider     CADExperience = null;

	public Text CADWhite = null;
	public Text CADBlack = null;

	void Start() {
		InputField inF = FindObjectOfType(typeof(InputField)) as InputField;
		Dropdown[] dds = FindObjectsOfType(typeof(Dropdown))  as Dropdown[];
		Slider     CAD = FindObjectOfType(typeof(Slider))     as Slider;

		if (inF != null) {
			nameField = inF;
		}

		if (dds != null) {
			for (int i = 0; i < dds.Length; i++) {
				Dropdown dd = dds[i];
				if (dd.name == "Trial Type") {
					trialType = dd;
				}
				else if (dd.name == "Handedness") {
					handedness = dd;
				}
			}
		}

		if (CAD != null) {
			CADExperience = CAD;
		}
	} 

	public void GoBack() {
		Session.instance.Home();
	}

	public void CreateNewUser() {
		// Get name from input
		string name = nameField.text;

		// Set session user
		Session.instance.user = name;
		string path = Session.instance.thisUserPath;

		if (Directory.Exists(path)) {
			Debug.Log("Error! User already exists!");
			Session.instance.user = null;
			return;
		}

		// Create user directory
		Directory.CreateDirectory(path);
		Debug.Log("Created user at " + path);

		// Create user info file.
        using (FileStream fs = File.Create(path + "/info.txt"))
        {
        	StringBuilder sb = new StringBuilder();
        	sb.Append("Name\t"       + name                      + "\n");
        	sb.Append("Trial Type\t" + trialType.value           + "\n");
        	sb.Append("Handedness\t" + handedness.value          + "\n");
        	sb.Append("CAD Exp\t"    + (int) CADExperience.value + "\n");

        	Byte[] info = new UTF8Encoding(true).GetBytes(sb.ToString());
        	// Add some information to the file.
        	fs.Write(info, 0, info.Length);
        }
	}

	public void UpdateCAD() {
		string text;
		switch ((int) CADExperience.value) {
			case (1):
				text = "1 - No experience.";
				break;

			case (2):
				text = "2 - Beginner User.";
				break;

			case (3):
				text = "3 - Intermediate User.";
				break;

			case (4):
				text = "4 - Proficient User.";
				break;

			case (5):
				text = "5 - Expert User.";
				break;

			default:
				text = "Unrecognized?";
				break;
		}

		CADWhite.text = CADBlack.text = text;
	}
}
