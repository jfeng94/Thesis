using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ViewUser : MonoBehaviour {
	public Text userWhite;
	public Text userBlack;

	// Use this for initialization
	void Start () {
		userWhite.text = "Welcome back\n" + Session.instance.user;
		userBlack.text = "Welcome back\n" + Session.instance.user;
	}

	//
	public void Trial() {
		Session.instance.Trial();
	}
}
