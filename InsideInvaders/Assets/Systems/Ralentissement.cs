using UnityEngine;
using FYFY;

public class Ralentissement : FSystem {
	private Family _ralentissableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Ralentissable)));
	private Family _ralentisseurGO = FamilyManager.getFamily(new AllOfComponents(typeof(Ralentisseur)));

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		foreach (GameObject go1 in _ralentisseurGO) {
			float rayon_effet = go1.GetComponent<Ralentisseur> ().rayon_effet;
			Transform tr1 = go1.GetComponent<Transform> ();
			foreach (GameObject go2 in _ralentissableGO) {
				Transform tr2 = go2.GetComponent<Transform> ();
				float distance = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
					+ (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
				if (distance < rayon_effet && go2.GetComponent<Ralentissable>().ralenti == false && go2.GetComponent<MouvantAleatoire>() != null) {
					go2.GetComponent<Ralentissable> ().ralenti = true;
					go2.GetComponent<MouvantAleatoire> ().vitesse /= 2;
				}
			}
		}
	}
}