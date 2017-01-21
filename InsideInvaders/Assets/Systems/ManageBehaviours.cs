using UnityEngine;
using FYFY;

public class ManageBehaviours : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.

	private Family _recuperableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Recuperable)));
	private Family _intrusGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamIntrus)));
	private Family _defensesGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamDefense)),new NoneOfComponents(typeof(ControllableByKeyboard)));



	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		int last_index = 0;
		//maj de LastBehaviour
		foreach (GameObject go in _intrusGO) {
			last_index = go.GetComponent<CurrentBehaviour> ().index_behaviour;
			go.GetComponent<LastBehaviour> ().index_behaviour = last_index;
		}
		foreach (GameObject go in _defensesGO) {
			Debug.Log ("go obtenu");
			CurrentBehaviour cb = go.GetComponent<CurrentBehaviour> ();
			if (cb == null)
				Debug.Log ("BLEMMME");
			//last_index = go.GetComponent<CurrentBehaviour> ().index_behaviour;
			go.GetComponent<LastBehaviour> ().index_behaviour = last_index;
		}
		//1-Patrouille
		//TODO

		//2-Suivi joueur
		foreach (GameObject go in _recuperableGO) {
			Debug.Log ("go obtenu");
			if (go.GetComponent<Recuperable> ().recupere == true) {
				go.GetComponent<CurrentBehaviour> ().index_behaviour = 2;
				Debug.Log ("index mis a 2");
			}
		}
	}
}