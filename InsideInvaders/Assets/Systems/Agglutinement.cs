using UnityEngine;
using FYFY;

public class Agglutinement : FSystem {
	private Family _agglutinableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Agglutinable)));
	private Family _agglutineurGO = FamilyManager.getFamily(new AllOfComponents(typeof(Agglutineur)));
	public int seuil_agglutinement = 200;

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
		foreach (GameObject go1 in _agglutineurGO) {
			float rayon_effet = go1.GetComponent<Agglutineur> ().rayon_effet;
			int pas_agglutinement = go1.GetComponent<Agglutineur> ().vitesse_agglutinement;
			Transform tr1 = go1.GetComponent<Transform> ();
			foreach (GameObject go2 in _agglutinableGO) {
				Transform tr2 = go2.GetComponent<Transform> ();
				float distance = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
					+ (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
				if (distance < rayon_effet) {
					if (go2.GetComponent<Agglutinable> ().progres_agglutinement < seuil_agglutinement) {
						Debug.Log ("agglutinement en cours");
						go2.GetComponent<Agglutinable> ().progres_agglutinement += pas_agglutinement;
					}
				}
			}
		}
		foreach (GameObject go2 in _agglutinableGO) {
			if (go2.GetComponent<Agglutinable> ().progres_agglutinement >= seuil_agglutinement) {
				Transform tr2 = go2.GetComponent<Transform> ();
				GameObjectManager.destroyGameObject (go2);
				GameObject go = GameObjectManager.instantiatePrefab ("Prefabs/Virus Agglutine");
				go.transform.position = tr2.position;
			}
		}
	}
}