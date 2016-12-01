using UnityEngine;
using FYFY;

public class Infection : FSystem {
	private Family _infectableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Infectable)));
	private Family _infectieuxGO = FamilyManager.getFamily(new AllOfComponents(typeof(Infectieux)));


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
		foreach (GameObject go1 in _infectieuxGO) {
			float rayon_effet = go1.GetComponent<Infectieux> ().rayon_effet;
			float pas_infection = go1.GetComponent<Infectieux> ().vitesse_infection;
			Transform tr1 = go1.GetComponent<Transform> ();

			foreach (GameObject go2 in _infectableGO) {
				Transform tr2 = go2.GetComponent<Transform> ();
				float distance = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
				                 + (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
				

				Infectable inf = go2.GetComponent<Infectable> ();
				if (distance < rayon_effet) {
					
					if (inf.progres_infection < inf.timeForInfect) {
						Debug.Log ("infection en cours");
						inf.progres_infection += pas_infection * Time.deltaTime;
						Debug.Log ("value cell : " + (int)inf.progres_infection);
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