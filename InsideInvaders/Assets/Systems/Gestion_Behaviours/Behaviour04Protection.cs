using UnityEngine;
using FYFY;

public class Behaviour04Protection : FSystem {
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
			if (go.GetComponent<CurrentBehaviour> ().index_behaviour == 4) {
				float distance_de_suivi = 10;

				Transform tr1 = go.GetComponent<Recuperable> ().cible_protection.transform;
				Transform tr2 = go.transform;

				float distance = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
					+ (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
				Rigidbody rb = go.GetComponent<Rigidbody> ();
				//Debug.Log ("test distance " + distance);

				if (distance > distance_de_suivi) {

					Vector3 v = tr1.position - tr2.position;
					//v.x = v.x * (distance-distance_de_suivi)/100;
					//v.y = v.y * (distance-distance_de_suivi)/100;
					//v.z = v.z * (distance-distance_de_suivi)/100;
					//rb.AddForce (v);
					rb.velocity = v;
				}
				else {
					rb.velocity = Vector3.zero;
				}
			}
		}
	}
}