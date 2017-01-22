using UnityEngine;
using FYFY;

public class Behaviour03Attaque : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _allUnitsGO = FamilyManager.getFamily(new AllOfComponents(typeof(Behaviour)));
	private Family _distanceUnitsGO = FamilyManager.getFamily(new AnyOfTags("longRange"));
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) 
	{
		foreach (GameObject go in _allUnitsGO) 
		{
			if (go.GetComponent<Behaviour> ().index_currentBehaviour == EnumBehaviour.Attaque) 
			{
				//Debug.Log ("Dans la behaviour 3");
				if(_distanceUnitsGO.contains(go.GetInstanceID())){
					// l'unité attaque à distance
					attaqueDistance(go);
				}else
					poursuivre(go);
			}
		}
	}


	private void poursuivre(GameObject go){
		float distance_de_suivi = 0;
		Transform tr1 = go.GetComponent<Behaviour> ().cible_poursuite.transform;
		Transform tr2 = go.transform;
		float distance = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
		                 + (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
		Rigidbody rb = go.GetComponent<Rigidbody> ();
		//Debug.Log ("test distance " + distance);

		if (distance > distance_de_suivi) {

			Vector3 v = tr1.position - tr2.position;
			//v.x = v.x * (distance-distance_de_suivi)/100;
			//v.y = v.y * (distance-distance_de_suivi)/100;
			//v.z = v.z * (distance-distance_de_suivi)/100;
			//rb.AddForce (v);
			rb.velocity = v;
		} else {
			rb.velocity = Vector3.zero;
		}
	}

	private void attaqueDistance(GameObject go){

		EnumBehaviour last_Behav = go.GetComponent<Behaviour> ().index_lastBehaviour;
		if (last_Behav != EnumBehaviour.Attaque) {
			// on vient de rentrer dans le Behaviour03Attaque
			// pour ne lancer qu'un Anticorps par ennemis
			Debug.Log ("Pop");
			GameObject newAnticorps;
			if (go.GetComponent<Ralentisseur> () != null) // je suis un Lymphocyte B SpeBacterien
				newAnticorps = GameObjectManager.instantiatePrefab ("Prefabs/Anticorps SpeBacterien");
			else
				newAnticorps = GameObjectManager.instantiatePrefab ("Prefabs/Anticorps SpeViral");

			Vector3 vect = go.transform.position;
			newAnticorps.transform.position = new Vector3 (vect.x + 1, vect.y + 3, vect.z + 1);
			Vivant vivant = newAnticorps.GetComponent<Vivant> ();
			vivant.objectifCoord = go.transform.position;
			NavMeshAgent agent = go.GetComponent<NavMeshAgent> ();
			agent.speed = vivant.speedAgent;
			agent.angularSpeed = vivant.angularSpeedAgent;
			agent.acceleration = vivant.accelerationAgent;
			agent.SetDestination (vivant.objectifCoord);
			vivant.agent = agent;
		}

	

	}
}