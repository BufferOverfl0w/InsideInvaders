using UnityEngine;
using FYFY;

public class Behaviour01Patrouille : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _allUnitsGO = FamilyManager.getFamily(new AllOfComponents(typeof(CurrentBehaviour)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		foreach (GameObject go in _allUnitsGO) {
			if (go.GetComponent<CurrentBehaviour> ().index_behaviour == 1) {
				//TODO voir patrouille deja faite
			}
		}
	}
}