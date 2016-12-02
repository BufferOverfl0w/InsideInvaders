using UnityEngine;
using FYFY;

public class SpriteLookPlayer : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private GameObject[] listCanevas_Go;
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		listCanevas_Go =  GameObject.FindGameObjectsWithTag ("canvas_unite");
		foreach  (GameObject go in listCanevas_Go){
			go.transform.LookAt (Camera.main.transform.position, Vector3.up);
			go.transform.Rotate (new Vector3 (0, 180, 0));
		}
	}
}