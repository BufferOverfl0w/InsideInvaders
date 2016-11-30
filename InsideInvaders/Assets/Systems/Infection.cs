using UnityEngine;
using FYFY;

public class Infection : FSystem {
	private Family _infectableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Infectable)));
	private Family _infectieuxGO = FamilyManager.getFamily(new AllOfComponents(typeof(Infectieux)));
	public int seuil_infection = 200;

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
		float rayon_effet = 30;
		int pas_infection = 1;
		foreach (GameObject go1 in _infectieuxGO) {
			Transform tr1 = go1.GetComponent<Transform> ();
			foreach (GameObject go2 in _infectableGO) {
				Transform tr2 = go2.GetComponent<Transform> ();
				float distance = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
				                 + (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
				if (distance < rayon_effet) {
					if (go2.GetComponent<Infectable> ().progres_infection < seuil_infection) {
						Debug.Log ("infection en cours");
						go2.GetComponent<Infectable> ().progres_infection += pas_infection;
					} else {
						Debug.Log ("infection completee");
						go2.GetComponent<Infectable> ().infecte = true;
						// + TODO: changement couleur?
					}
				}
			}
		}
	}
}