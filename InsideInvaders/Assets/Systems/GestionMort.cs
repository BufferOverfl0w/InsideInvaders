using UnityEngine;
using FYFY;

public class GestionMort : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _vivantsGO = FamilyManager.getFamily(new AllOfComponents(typeof(Vivant)), new NoneOfComponents(typeof(Infectable)));
	private Family _bacteries_puresGO = FamilyManager.getFamily(new AnyOfTags("Bacterie"));
	private Family _vivantsInfectablesGO = FamilyManager.getFamily(new AllOfComponents(typeof(Vivant),typeof(Infectable)));
	private Family _recuperablesGO = FamilyManager.getFamily(new AllOfComponents(typeof(Recuperable)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}


	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		Transform tr;
		foreach (GameObject go in _vivantsGO){
			if (go.GetComponent<Vivant>().current_pv <= 0){
				foreach (GameObject go2 in _recuperablesGO){
					if (go2.GetComponent<Recuperable> ().cible_poursuite != null) {
						if (go2.GetComponent<Recuperable> ().cible_poursuite.Equals (go)) {
							go2.GetComponent<Recuperable> ().cible_poursuite = null;
						}
					}

					if (go2.GetComponent<Recuperable> ().cible_protection != null) {
						if (go2.GetComponent<Recuperable> ().cible_protection.Equals (go)) {
							go2.GetComponent<Recuperable> ().cible_protection = null;
						}
					}
				}
				GameObjectManager.destroyGameObject(go);
			}
		}

		foreach (GameObject go in _vivantsInfectablesGO){
			if (go.GetComponent<Vivant>().current_pv <= 0){
				tr = go.GetComponent<Transform> ();

				if (go.GetComponent<Infectable> ().infecte == true) {
					GameObjectManager.destroyGameObject (go);
					GameObject new_go = GameObjectManager.instantiatePrefab ("Prefabs/Virus");
					new_go.transform.position = tr.position;
				} else {
					if (go.tag == "Bacterie") { 
						// on génére peut de déchet 
						GameObjectManager.destroyGameObject (go);
						GameObject new_go1 = GameObjectManager.instantiatePrefab ("Prefabs/Dechet");
						new_go1.transform.position = tr.position;
					} else {
						// les autres générent bc de déchet
						GameObjectManager.destroyGameObject (go);
						GameObject new_go1 = GameObjectManager.instantiatePrefab ("Prefabs/Dechet");
						GameObject new_go2 = GameObjectManager.instantiatePrefab ("Prefabs/Dechet");
						GameObject new_go3 = GameObjectManager.instantiatePrefab ("Prefabs/Dechet");
						new_go1.transform.position = tr.position;
						new_go2.transform.position = tr.position;
						new_go3.transform.position = tr.position;
					}
				}
			}
		}
	}
}