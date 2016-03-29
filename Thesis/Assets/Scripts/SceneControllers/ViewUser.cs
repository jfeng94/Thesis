using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;

public class ViewUser : MonoBehaviour {
	public Text userWhite;
	public Text userBlack;

	public Text infoWhite;
	public Text infoBlack;

	// Use this for initialization
	void Start () {
		userWhite.text = "Welcome back\n" + Session.instance.user;
		userBlack.text = "Welcome back\n" + Session.instance.user;

		StringBuilder info = new StringBuilder();
		if (Session.instance.day > Session.totalDays) {
			info.Append("You're done with the trials!\n\n\nGet out.");
		}
		else {
			info.Append("Day " + Session.instance.day + "\n");
			if (Session.instance.trial != 0) {
				info.Append("Continue where you left off -- Trial " + Session.instance.trial);
			}
		} 

		infoWhite.text = infoBlack.text = info.ToString();
	}

	//
	public void Trial() {
		Session.instance.Trial();
	}
}
