using UnityEngine;
using FYFY;

public class Behaviour07Fuite : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _allUnitsGO = FamilyManager.getFamily(new AllOfComponents(typeof(Behaviour)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		foreach (GameObject go in _allUnitsGO) {
			if (go.GetComponent<Behaviour> ().index_currentBehaviour == EnumBehaviour.Fuite) {
				Vector3 direction = ManageBehaviours.poinBbackToEnemy (go, go.GetComponent<Behaviour> ().cible_a_fuir, 20);
				Rigidbody rb = go.GetComponent<Rigidbody> ();
				rb.velocity = direction;
			}
		}
	}
}