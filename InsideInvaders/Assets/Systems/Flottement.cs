using UnityEngine;
using FYFY;

public class Flottement : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _flottantGO = FamilyManager.getFamily(new AllOfComponents(typeof(Flottant)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		if (familiesUpdateCount % 50 == 0) {
			foreach (GameObject go in _flottantGO) {
				float force =  go.GetComponent<Flottant>().force;
				Rigidbody rb = go.GetComponent<Rigidbody> ();
				Vector3 v = new Vector3 (Random.Range (-10f, 10f) * force, go.transform.position.y, Random.Range (-10f, 10f) * force);
				rb.AddForce (v);
				float torque = Random.Range (-2f, 2f);
				rb.AddTorque (v*torque);
			}
		}
	}
}