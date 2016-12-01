using UnityEngine;
using FYFY;

public class GenerationToxiines : FSystem {
	private Family _genToxGO = FamilyManager.getFamily(new AllOfComponents(typeof(GenerateurToxines)));
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
		foreach(GameObject go1 in _genToxGO){
			go1.GetComponent<GenerateurToxines>().temps += Time.deltaTime;
			Debug.Log (go1.GetComponent<GenerateurToxines>().temps);
			Debug.Log (go1.GetComponent<GenerateurToxines>().periode);
			if (go1.GetComponent<GenerateurToxines> ().temps > go1.GetComponent<GenerateurToxines> ().periode) {
				go1.GetComponent<GenerateurToxines> ().temps = 0f;
				GameObject go2 = GameObjectManager.instantiatePrefab ("Prefabs/Toxine");
				go2.transform.position = new Vector3 (go1.transform.position.x + 10, go1.transform.position.y, go1.transform.position.z + 10);
			}
		}
	}
}