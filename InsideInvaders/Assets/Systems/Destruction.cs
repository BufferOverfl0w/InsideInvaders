using UnityEngine;
using FYFY;

public class Destruction : FSystem {
	private Family _destructibleGO = FamilyManager.getFamily(new AllOfComponents(typeof(Infectable)));
	private Family _destructeurGO = FamilyManager.getFamily(new AllOfComponents(typeof(Destructeur)));

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
		foreach (GameObject go1 in _destructibleGO) {
			if (go1.tag == "Bacterie" && go1.GetComponent<Infectable> ().infecte == true) {
				Transform tr1 = go1.GetComponent<Transform> ();
				foreach (GameObject go2 in _destructeurGO) {
					float rayon_effet = go2.GetComponent<Destructeur> ().rayon_effet;

					Transform tr2 = go2.GetComponent<Transform> ();
					float distance = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
					                 + (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
					if (distance < rayon_effet) {
						GameObjectManager.destroyGameObject (go1);
						Debug.Log ("bacterie infectee detruite");
					}
				}
			}
		}
	}
}