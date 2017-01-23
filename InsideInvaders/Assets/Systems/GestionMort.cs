using UnityEngine;
using FYFY;

public class GestionMort : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _vivantsGO = FamilyManager.getFamily(new AllOfComponents(typeof(Vivant)), new NoneOfComponents(typeof(Infectable)));
	private Family _bacteries_puresGO = FamilyManager.getFamily(new AnyOfTags("Bacterie"));
	private Family _vivantsInfectablesGO = FamilyManager.getFamily(new AllOfComponents(typeof(Vivant),typeof(Infectable)));
	private Family _behaviourGO = FamilyManager.getFamily(new AllOfComponents(typeof(Behaviour)));
	private Family _anticorpsGO = FamilyManager.getFamily(new AnyOfTags("Anticorps"));
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

//	Debug.Log ("go2 " + go2.name +"cible ; "+behav.cible_poursuite.name);
//	Debug.Log ("go " + go.name);
//
//		if (go2.CompareTag ("Anticorps")) {
//			Debug.Log ("Anticorps");
//			GameObjectManager.destroyGameObject (go2);
//	}
	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		Transform tr;
		foreach (GameObject go in _vivantsGO){
			if (go.GetComponent<Vivant>().current_pv <= 0){
				foreach (GameObject go2 in _behaviourGO){
					Behaviour behav = go2.GetComponent<Behaviour> ();
					if (behav.cible_poursuite != null) {
						if (behav.cible_poursuite.Equals (go)){
							behav.cible_poursuite = null;
						}
					}

					if (behav.cible_protection != null) {
						if (behav.cible_protection.Equals (go)) {
							behav.cible_protection = null;
						}
					}
					if (behav.cible_a_fuir != null) {
						if (behav.cible_a_fuir.Equals (go)) {
							behav.cible_a_fuir = null;
						}
					}
				}
				GameObjectManager.destroyGameObject(go);
			}
		}

		foreach (GameObject go in _vivantsInfectablesGO){
			if (go.GetComponent<Vivant>().current_pv <= 0){
				tr = go.GetComponent<Transform> ();

				foreach (GameObject anticorps in _anticorpsGO){
					Behaviour behav = anticorps.GetComponent<Behaviour> ();
					if (behav.cible_poursuite != null) {
						if (behav.cible_poursuite.Equals (go)){
							behav.cible_poursuite = null;
							GameObjectManager.destroyGameObject(anticorps);
						}
					}
				}
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