using UnityEngine;
using FYFY;
using FYFY_plugins;

public class PlayerParticules : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _controlableGO = FamilyManager.getFamily(new AllOfComponents(typeof(ControllableByKeyboard)));
	GameObject obj_Back_L;
	GameObject obj_Back_R;
	GameObject obj_Front;
	GameObject obj_L;
	GameObject obj_R;
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
		foreach (GameObject go in _controlableGO) {
			obj_Back_L = go.transform.Find("Afterburner_Back_L").gameObject;
			obj_Back_R =  go.transform.Find("Afterburner_Back_R").gameObject;
			obj_Front =  go.transform.Find("Afterburner_Front").gameObject;
			obj_L =  go.transform.Find("Afterburner_Left").gameObject;
			obj_R =  go.transform.Find("Afterburner_Right").gameObject;
//			if ((obj_L == null) || (obj_R == null) || (obj_B == null)) {
//				this.Pause = true;
//			}
			stopAllReactorEffect ();
		}
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		updatetReactorEffect ();


	}

	private void updatetReactorEffect(){

		stopAllReactorEffect ();
		if (ManagePlayerMouvement.playerPressForward ){
			obj_Back_L.GetComponent<ParticleSystem> ().Play();
			obj_Back_R.GetComponent<ParticleSystem> ().Play();
		}
		if (ManagePlayerMouvement.playerPressBackward) {
			obj_Front.GetComponent<ParticleSystem> ().Play ();
		}
		if ( ManagePlayerMouvement.playerPressRight) {
			obj_L.GetComponent<ParticleSystem> ().Play ();
		}
		if (ManagePlayerMouvement.playerPressLeft) {
			obj_R.GetComponent<ParticleSystem> ().Play ();
		}
	}

	private void stopAllReactorEffect(){
		obj_Back_L.GetComponent<ParticleSystem> ().Stop();
		obj_Back_R.GetComponent<ParticleSystem> ().Stop();
		obj_Front.GetComponent<ParticleSystem> ().Stop();
		obj_L.GetComponent<ParticleSystem> ().Stop();
		obj_R.GetComponent<ParticleSystem> ().Stop();
	}
}