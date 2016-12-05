using UnityEngine;
using FYFY;

public class DeplacementVirus : FSystem {
	private Family _mouvantGO = FamilyManager.getFamily(new AllOfComponents(typeof(MouvantCible)));
	private Family _infectableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Infectable)));

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
		float distMin = 10000;
		foreach (GameObject go1 in _mouvantGO){
			int rayon_detection = go1.GetComponent<MouvantCible> ().rayon_detection;
			go1.GetComponent<MouvantCible> ().temps += Time.deltaTime;
			if (go1.GetComponent<MouvantCible> ().positionCible == Vector3.zero || go1.GetComponent<MouvantCible> ().temps > go1.GetComponent<MouvantCible> ().periode) {
				if (collisionVirus (go1)) {
					positionCibleAleatoire (go1);
					go1.transform.position = Vector3.MoveTowards (go1.transform.position, go1.GetComponent<MouvantCible> ().positionCible, go1.GetComponent<MouvantCible> ().vitesse * Time.deltaTime);
					go1.GetComponent<MouvantCible> ().temps /= 2;
					break;
				}
				Transform tr1 = go1.GetComponent<Transform> ();
				float dist = 10000;
				foreach (GameObject go2 in _infectableGO) {
					if (go2.GetComponent<Infectable> ().infecte == true)
						continue;
					Transform tr2 = go2.GetComponent<Transform> ();
					dist = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
					+ (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
					if (dist < rayon_detection) {
						if (dist < distMin) {
							dist = distMin;
							go1.GetComponent<MouvantCible> ().positionCible = go2.transform.position;
							go1.GetComponent<MouvantCible> ().temps = 0f;
							//go1.transform.position = Vector3.MoveTowards (go1.transform.position, go1.GetComponent<MouvantCible> ().positionCible, go1.GetComponent<MouvantCible> ().vitesse * Time.deltaTime);
						}
					} else {
						if (go1.GetComponent<MouvantCible> ().positionCible == Vector3.zero || go1.GetComponent<MouvantCible> ().positionCible == go1.transform.position) {
							positionCibleAleatoire (go1);
							go1.GetComponent<MouvantCible> ().temps = 0f;
						}
						//go1.transform.position = Vector3.MoveTowards (go1.transform.position, go1.GetComponent<MouvantCible> ().positionCible, go1.GetComponent<MouvantCible> ().vitesse * Time.deltaTime);
					}
				}
			}
			go1.transform.position = Vector3.MoveTowards (go1.transform.position, go1.GetComponent<MouvantCible> ().positionCible, go1.GetComponent<MouvantCible> ().vitesse * Time.deltaTime);
		}
	}

	bool collisionVirus(GameObject go1){
		foreach (GameObject go2 in _mouvantGO) {
			if (go2 == go1)
				continue;
			Transform tr1 = go1.GetComponent<Transform> ();
			Transform tr2 = go2.GetComponent<Transform> ();
			float dist = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
				+ (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
			if (dist < 20) {
				Debug.Log ("Collision virus");
				return true;
			}
		}
		return false;
	}

	void positionCibleAleatoire(GameObject go1){
		float posX = go1.transform.position.x + Random.Range (-200f, 200f);
		float posZ = go1.transform.position.z + Random.Range (-200f, 200f);
		// limites du terrain
		if (posX < -200)
			posX = 200;
		if (posX > 290)
			posX = 290;
		if (posZ < -370)
			posZ = -370;
		if (posZ > 230)
			posZ = 230;
		go1.GetComponent<MouvantCible> ().positionCible = new Vector3 (posX, go1.transform.position.y, posZ);
	}
}