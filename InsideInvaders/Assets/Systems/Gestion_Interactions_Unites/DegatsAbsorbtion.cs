using UnityEngine;
using FYFY;

public class DegatsAbsorbtion : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _absorbableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Absorbable)), new NoneOfComponents(typeof(Infectable)));
	private Family _absorbableInfectableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Absorbable),typeof(Infectable)));
	private Family _absorbeurGO = FamilyManager.getFamily(new AllOfComponents(typeof(Absorbeur)));
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){

		foreach (GameObject go1 in _absorbeurGO) {
			if(go1.GetComponent<Absorbeur> ().degats_absorbtion == 1)
				go1.GetComponent<Absorbeur> ().degats_absorbtion = 1 + Random.Range(-0.5f, 0.5f);
		}
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		
		foreach (GameObject go1 in _absorbeurGO) {
			float rayon_effet = go1.GetComponent<Absorbeur> ().rayon_effet;
			float degats_absorbtion = go1.GetComponent<Absorbeur> ().degats_absorbtion;
			Transform tr1 = go1.GetComponent<Transform> ();
			foreach (GameObject go2 in _absorbableGO) {
				Transform tr2 = go2.GetComponent<Transform> ();
				float distance = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
					+ (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
				if (distance < rayon_effet) {
					//Debug.Log ("je fais des degats");
					go2.GetComponent<Vivant> ().current_pv -= degats_absorbtion;
				}
			}

			foreach (GameObject go3 in _absorbableInfectableGO) {
				Transform tr3 = go3.GetComponent<Transform> ();
				float distance = Mathf.Sqrt ((tr1.position.x - tr3.position.x) * (tr1.position.x - tr3.position.x)
					+ (tr1.position.z - tr3.position.z) * (tr1.position.z - tr3.position.z));
				if (distance < rayon_effet) {
					//Debug.Log ("je fais des degats");
					go3.GetComponent<Vivant> ().current_pv -= degats_absorbtion;
					if (go3.GetComponent<Vivant> ().current_pv <= 0) {
						go3.GetComponent<Infectable> ().infecte = false;
					}
				}
			}
		}
	}
}