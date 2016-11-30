using UnityEngine;
using FYFY;

public class DeplacementAleatoire : FSystem {
	private Family _maGO = FamilyManager.getFamily(new AllOfComponents(typeof(MouvantAleatoire)));

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
		foreach (GameObject go in _maGO) {
			if (go.GetComponent<MouvantAleatoire> ().positionCible == Vector3.zero || go.GetComponent<MouvantAleatoire> ().positionCible == go.transform.position) {
				float posX = go.transform.position.x + Random.Range (-200f, 200f);
				float posZ = go.transform.position.z + Random.Range (-200f, 200f);
				// limites du terrain
				if (posX < -200)
					posX = 200;
				if (posX > 290)
					posX = 290;
				if (posZ < -370)
					posZ = -370;
				if (posZ > 230)
					posZ = 230;
				go.GetComponent<MouvantAleatoire> ().positionCible = new Vector3 (posX, go.transform.position.y, posZ);
			}
			go.transform.position = Vector3.MoveTowards(go.transform.position, go.GetComponent<MouvantAleatoire> ().positionCible, go.GetComponent<MouvantAleatoire>().vitesse * Time.deltaTime);
		}
	}
}