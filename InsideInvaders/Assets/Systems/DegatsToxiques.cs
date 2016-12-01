using UnityEngine;
using FYFY;

public class DegatsToxiques : FSystem {
	private Family _toxiqueGO = FamilyManager.getFamily(new AllOfComponents(typeof(Toxique)));
	private Family _intoxiquesGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamDefense)), new NoneOfComponents(typeof(Absorbeur)));

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
		foreach (GameObject go1 in _toxiqueGO) {
			float rayon_effet = go1.GetComponent<Toxique> ().rayon_effet;
			int degats_toxiques = go1.GetComponent<Toxique> ().degats_toxiques;
			Transform tr1 = go1.GetComponent<Transform> ();
			foreach (GameObject go2 in _intoxiquesGO) {
				Transform tr2 = go2.GetComponent<Transform> ();
				float distance = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
					+ (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
				if (distance < rayon_effet) {
					Debug.Log ("degats toxine");
					go2.GetComponent<BarreDeVie> ().current_pv -= degats_toxiques;
				}
			}
		}
	}
}
