using UnityEngine;
using FYFY;

public class Behaviour04Protection : FSystem {
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
			if (go.GetComponent<Behaviour> ().index_currentBehaviour == EnumBehaviour.Protection) {
				float distance_de_suivi = 30;

				Transform tr1 = go.GetComponent<Behaviour> ().cible_protection.transform;
				Transform tr2 = go.transform;

				float distance = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
					+ (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
				Rigidbody rb = go.GetComponent<Rigidbody> ();
				if (distance > distance_de_suivi) {
					propulse (go, tr1.position);
				}
				else 
					rb.velocity = Vector3.zero;
			}
		}
	}
	private static void propulse(GameObject him, Vector3 target){
		Rigidbody rb = him.GetComponent<Rigidbody> ();
		if (rb == null) return;
		Vector3 v = target - him.transform.position;
		rb.velocity = v;
		if (rb.velocity.magnitude > him.GetComponent<Vivant> ().speedAgent) { // trop rapide !
			rb.velocity = rb.velocity.normalized * him.GetComponent<Vivant> ().speedAgent * 4;
		}
	}
}