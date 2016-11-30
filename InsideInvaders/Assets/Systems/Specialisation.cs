using UnityEngine;
using FYFY;

public class Specialisation : FSystem {
	private Family _specialisableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Specialisable)));
	private Family _specialisantGO = FamilyManager.getFamily(new AllOfComponents(typeof(Specialisant)));
	public int seuil_specialisation = 200;

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
		float rayon_effet = 30;
		int pas_specialisation = 1;
		foreach (GameObject go1 in _specialisantGO) {
			Transform tr1 = go1.GetComponent<Transform> ();
			foreach (GameObject go2 in _specialisableGO) {
				Transform tr2 = go2.GetComponent<Transform> ();
				float distance = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
					+ (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
				if (distance < rayon_effet) {
					if (go1.tag == "Virus") {
						if (go2.GetComponent<Specialisable> ().progres_spec_viral < seuil_specialisation) {
							Debug.Log ("specialisation virale en cours");
							go2.GetComponent<Specialisable> ().progres_spec_viral += pas_specialisation;
						} else {
							Object.Instantiate(go2.GetComponent<Specialisable> ().LymphocyteBViral, tr2.position, Quaternion.identity);
							GameObjectManager.destroyGameObject(go2);
							Debug.Log ("specialisation virale completee");
						}
					}
					if(go1.tag == "Bacterie"){
						if (go2.GetComponent<Specialisable> ().progres_spec_bacterien < seuil_specialisation) {
							Debug.Log ("specialisation bacterienne en cours");
							go2.GetComponent<Specialisable> ().progres_spec_bacterien += pas_specialisation;
						} else {
							Object.Instantiate(go2.GetComponent<Specialisable> ().LymphocyteBBacterien, tr2.position, Quaternion.identity);
							GameObjectManager.destroyGameObject(go2);
							Debug.Log ("specialisation bacterienne completee");
						}
					}
				}
			}
		}
	}
}