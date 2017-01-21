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
			CurrentBehaviour cb = go.GetComponent<CurrentBehaviour> ();

			last_index = go.GetComponent<CurrentBehaviour> ().index_behaviour;
			go.GetComponent<LastBehaviour> ().index_behaviour = last_index;
		}
		//1-Patrouille
		foreach (GameObject go in _recuperableGO) {
			//Quelques booléens préliminaires
			bool en_deplacement = true;
			bool pas_de_cible = false;
			Rigidbody go_rb = go.GetComponent<Rigidbody> ();
			if ((Mathf.Abs(go_rb.velocity.x)+Mathf.Abs(go_rb.velocity.z)) < 15){
				en_deplacement = false;
			}
			if ((go.GetComponent<Recuperable> ().cible_poursuite == null) && (go.GetComponent<Recuperable> ().cible_protection == null)) {
				pas_de_cible = true;
			}

			if (/*(go.GetComponent<Recuperable> ().recupere == false)&&*/(!en_deplacement)&&(pas_de_cible)) {
				go.GetComponent<CurrentBehaviour> ().index_behaviour = 1;
			} else if ((pas_de_cible)&&((go.GetComponent<LastBehaviour> ().index_behaviour==2)||(go.GetComponent<LastBehaviour> ().index_behaviour==3))){
				go.GetComponent<CurrentBehaviour> ().index_behaviour = 1;
			}
		}

		//2-Suivi joueur
		foreach (GameObject go in _recuperableGO) {
			if (go.GetComponent<Recuperable> ().recupere == true) {
				go.GetComponent<CurrentBehaviour> ().index_behaviour = 2;
			}
		}
		//3-Poursuite
		foreach (GameObject go in _recuperableGO) {
			if (go.GetComponent<Recuperable> ().cible_poursuite != null) {
				go.GetComponent<CurrentBehaviour> ().index_behaviour = 3;
			}
		}
		//4-Protection
		foreach (GameObject go in _recuperableGO) {
			if (go.GetComponent<Recuperable> ().cible_protection != null) {
				go.GetComponent<CurrentBehaviour> ().index_behaviour = 4;
			}
		}
	}
}