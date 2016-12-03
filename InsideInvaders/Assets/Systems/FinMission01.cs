using UnityEngine;
using FYFY;

public class FinMission01 : FSystem {
	private Family _intrusGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamIntrus)));

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		int count = 0;
		foreach (GameObject go in _intrusGO) {
			count += 1;
		}
		if (count == 0) {
			Debug.Log ("Mission Accomplie !!!");
		}
	}
}