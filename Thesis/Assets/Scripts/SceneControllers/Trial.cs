using UnityEngine;
using System.Collections;

public class Trial : MonoBehaviour {
	SceneInteractable sceneObject = null;
	SceneTarget       sceneTarget = null;
	// Use this for initialization
	void Start () {
		// Create sceneobject and scenetarget and hoops
		string siPath = "Prefabs/SceneInteractable";
		GameObject go = Instantiate(Resources.Load(siPath),
			            Vector3.zero,
			            Quaternion.identity) as GameObject;
		sceneObject   = go.GetComponent<SceneInteractable>() as SceneInteractable;

		if (sceneObject != null) {
			// Give it a random position and orientation.
			sceneObject.transform.position = new Vector3(Random.Range(-23f, 23f),
														 Random.Range(  2f, 23f),
														 Random.Range(  2f, 23f));
			sceneObject.transform.rotation = Random.rotation;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
