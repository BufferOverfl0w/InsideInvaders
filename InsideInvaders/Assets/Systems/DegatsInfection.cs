using UnityEngine;
using FYFY;

public class DegatsInfection : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _infectableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Infectable)));
	int degats_infection = 1;

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject go1 in _infectableGO) {
			if (go1.GetComponent<Infectable>().infecte == true) {
				
				go1.GetComponent<BarreDeVie> ().current_pv -= degats_infection;
			
			}
		}
	}
}