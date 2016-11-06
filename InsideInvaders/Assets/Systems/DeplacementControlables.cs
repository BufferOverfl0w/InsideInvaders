using UnityEngine;
using FYFY;

public class DeplacementControlables : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _controlableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Controlable)));
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		foreach (GameObject go in _controlableGO) {
			//Rigidbody rb = go.GetComponent<Rigidbody> ();
			Transform tr = go.GetComponent<Transform> ();
			Controlable mv = go.GetComponent<Controlable> ();

			if (Input.GetKey (KeyCode.Z) == true) {
				//rb.AddForce (Vector3.forward * mv.speed);
				tr.Translate(new Vector3(0,1,0) * mv.speed * Time.deltaTime);
			}
			if (Input.GetKey (KeyCode.Q) == true) {
				//rb.AddForce (Vector3.left * mv.speed);
				tr.Rotate(new Vector3(0,0,1) * mv.speed*10 * Time.deltaTime);
			}
			if (Input.GetKey (KeyCode.S) == true) {
				//rb.AddForce (Vector3.back * mv.speed);
				tr.Translate(new Vector3(0,-1,0) * mv.speed * Time.deltaTime);
			}
			if (Input.GetKey (KeyCode.D) == true) {
				//rb.AddForce (Vector3.right * mv.speed);
				tr.Rotate(new Vector3(0,0,-1) * mv.speed*10 * Time.deltaTime);
			}
		}
	}
}