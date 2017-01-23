using UnityEngine;
using FYFY;

using System.Collections;
using System.Collections.Generic;

public class Behaviour07Fuite : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _allUnitsGO = FamilyManager.getFamily(new AllOfComponents(typeof(Behaviour)));
	private Family _vivantsGO = FamilyManager.getFamily(new AllOfComponents(typeof(Vivant)));


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
				propulseBackwards (go, go.GetComponent<Behaviour> ().cible_a_fuir.transform.position);

				/*
				go.GetComponent<Behaviour> ().cible_a_fuir = null;

				List<GameObject> List = ManageBehaviours.getVisionUnitsSorted (go, _vivantsGO);
				foreach (GameObject other in List) {
					
					if (ManageBehaviours.myTargetIs (go, other) == -1) {
						go.GetComponent<Behaviour> ().cible_a_fuir = other;
						go.GetComponent<Behaviour> ().index_currentBehaviour = EnumBehaviour.Fuite;
						break; // on fuit le plus proche 
					}
				}
				*/
			}
		}
	}
	private static void propulseBackwards(GameObject him, Vector3 target){
		Rigidbody rb = him.GetComponent<Rigidbody> ();
		if (rb == null) return;
		Vector3 v = target - him.transform.position;
		v.x = - (v.x);
		v.z = - (v.z);
		rb.velocity = v;
		if (rb.velocity.magnitude > him.GetComponent<Vivant> ().speedAgent) { // trop rapide !
			rb.velocity = rb.velocity.normalized * him.GetComponent<Vivant> ().speedAgent * 2;
		}
	}
}