using UnityEngine;
using FYFY;

public class GestionMort : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _vivantsGO = FamilyManager.getFamily(new AllOfComponents(typeof(BarreDeVie)));
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		foreach (GameObject go in _vivantsGO){
			if (go.GetComponent<BarreDeVie>().pv <= 0){
				//Object.Destroy(go);
				GameObjectManager.destroyGameObject(go);
			}
		}
	}
}