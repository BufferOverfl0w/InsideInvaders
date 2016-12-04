using UnityEngine;
using FYFY;

public class GestionMort : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _vivantsGO = FamilyManager.getFamily(new AllOfComponents(typeof(BarreDeVie)), new NoneOfComponents(typeof(Infectable)));
	private Family _bacteries_puresGO = FamilyManager.getFamily(new AnyOfTags("Bacterie"));
	private Family _vivantsInfectablesGO = FamilyManager.getFamily(new AllOfComponents(typeof(BarreDeVie),typeof(Infectable)));
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
			if (go.GetComponent<BarreDeVie>().current_pv <= 0){
				GameObjectManager.destroyGameObject(go);
			}
		}

		foreach (GameObject go in _vivantsInfectablesGO){
			if (go.GetComponent<BarreDeVie>().current_pv <= 0){
				if (go.GetComponent<Infectable> ().infecte == true) {
					tr = go.GetComponent<Transform> ();
					GameObjectManager.destroyGameObject (go);
					GameObject new_go = GameObjectManager.instantiatePrefab("Prefabs/Virus");
					new_go.transform.position = tr.position;
				} 
				else {
					if (go.tag == "Bacterie") {
						tr = go.GetComponent<Transform> ();
						GameObjectManager.destroyGameObject (go);
						GameObject new_go1 = GameObjectManager.instantiatePrefab ("Prefabs/Dechet");
						GameObject new_go2 = GameObjectManager.instantiatePrefab ("Prefabs/Dechet");
						GameObject new_go3 = GameObjectManager.instantiatePrefab ("Prefabs/Dechet");
						new_go1.transform.position = tr.position;
						new_go2.transform.position = tr.position;
						new_go3.transform.position = tr.position;
					} else {
						GameObjectManager.destroyGameObject(go);
					}

				}


			}
		}
	}
}