using UnityEngine;
using FYFY;
using System.Collections;
using System.Collections.Generic;

public class Behaviour01Patrouille : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _allUnitsBehaviourGO = FamilyManager.getFamily(new AllOfComponents(typeof(CurrentBehaviour),typeof(LastBehaviour),typeof(NavMeshAgent)));

	private Family _intrusGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamIntrus)));
	private Family _allUnitsGO = FamilyManager.getFamily(new AnyOfComponents(typeof(TeamIntrus),typeof(TeamDefense)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
		

	}


	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {


		foreach (GameObject go in _allUnitsBehaviourGO) {
			if (go.GetComponent<CurrentBehaviour> ().index_behaviour != 1) continue;
			updateComponents (go);
			patrouille (go);
			scanView (go);
		}
	}

	private void updateComponents(GameObject go )
	{
		BarreDeVie bdv = null;
		NavMeshAgent newAgent = null;
		bdv = go.GetComponent<BarreDeVie> ();
		if (bdv == null) {
			Debug.Log ("FUCK");
		}

		if (bdv.centreOfSphere == Vector3.zero) {
			bdv.centreOfSphere = go.transform.position; // on mets à jours le centre du cerle
			bdv.objectif = go.transform.position;
			newAgent = go.GetComponent<NavMeshAgent> ();
			newAgent.speed = bdv.speed;
			newAgent.angularSpeed = bdv.angularSpeed;
			newAgent.acceleration = bdv.acceleration;
			newAgent.SetDestination (bdv.objectif);
			bdv.agent = newAgent;
		}
		int last_Behav = go.GetComponent<LastBehaviour> ().index_behaviour;
		if (last_Behav != 1) {
			// on vient de rentrer dans le Behaviour01Patrouille
			bdv.waiForMaj = true;
		}
		if (bdv.waiForMaj) {
			Rigidbody go_rb = go.GetComponent<Rigidbody> ();
			float velocity = Mathf.Abs (go_rb.velocity.x) + Mathf.Abs (go_rb.velocity.z);
			if ((velocity>= 0.5f) && (velocity<=25.0f)){
				bdv.centreOfSphere = go.transform.position; // on mets à jours le centre du cerle
				bdv.objectif = go.transform.position;
				bdv.agent.SetDestination (bdv.objectif);
				bdv.waiForMaj = false;
			}
		}
	}

	private bool isBlocked(NavMeshAgent agent,BarreDeVie bdv){
		//Debug.Log ("isBlocked");

		float distance = Mathf.Sqrt ((agent.transform.position.x - bdv.objectif.x) * (agent.transform.position.x - bdv.objectif.x)
			+ (agent.transform.position.z - bdv.objectif.z) * (agent.transform.position.z - bdv.objectif.z));
		bool sameDist = Mathf.Approximately (distance, bdv.lastDistance);
		bdv.lastDistance =  distance;
		if (sameDist) 
		{
			//Debug.Log ("Approximately");
			bdv.nbSecBlocked = bdv.nbSecBlocked + Time.deltaTime;
			//Debug.Log (" nbSecBlocked "+ spe.nbSecBlocked );
			if (bdv.nbSecBlocked >= 2.0f) {// 2 sec
				bdv.nbSecBlocked = 0.0f;
				return true;
			} else {
				return false;
			}
		}

		bdv.nbSecBlocked = 0.0f;
		return false;
	}

	private void patrouille(GameObject go ){
		BarreDeVie bdv = go.GetComponent<BarreDeVie> ();
		if (!bdv.agent.enabled) return;

		float distance = Mathf.Sqrt ((go.transform.position.x - bdv.objectif.x) * (go.transform.position.x - bdv.objectif.x)
			+ (go.transform.position.z - bdv.objectif.z) * (go.transform.position.z - bdv.objectif.z));
		//Debug.Log ("distance " + distance);
		if( isBlocked( bdv.agent,bdv) || (distance<=2.5f) ){
			//Debug.Log ("in");
			Vector3 pos = (Random.insideUnitSphere * bdv.rayonPatrouille) + bdv.centreOfSphere;
			bdv.objectif =  new Vector3 (pos.x, go.transform.position.y, pos.z);
			//Debug.Log (spe.objectif);
			bdv.agent.SetDestination(bdv.objectif);
		}
	}

	private void scanView(GameObject him){
		
		BarreDeVie bdv = him.GetComponent<BarreDeVie> ();
		// Une unité de defense n'a besoin de voir que la TeamIntrus, alors que les intrus ont besoin de voir la TeamIntrus et Defense
		// Car un virus peut attaquer une bacterie
		bool AIsDef = him.GetComponent<TeamDefense> () != null;
		Family analyseGo = (AIsDef) ? _intrusGO : _allUnitsGO;

		Dictionary<GameObject, float> dico = new Dictionary<GameObject, float>();
		foreach (GameObject otherGo in analyseGo) {
			dico.Add(otherGo,getDistance(him,otherGo));
		}
			
		List<KeyValuePair<GameObject, float>> List = new List<KeyValuePair<GameObject, float>>(dico);
		List.Sort(delegate(KeyValuePair<GameObject, float> firstPair,
				KeyValuePair<GameObject, float> nextPair){
				return firstPair.Value.CompareTo(nextPair.Value);
			}
		);
		//List est une list composé des unités trié par les plus proche en 1er

		foreach (KeyValuePair<GameObject, float> value in List) {
			GameObject otherGo = value.Key;
			float distance = value.Value;
			if (distance < bdv.rayonVue) {
				int valueAttaque = ManageBehaviours.myTargetIs (him, otherGo);
				if (valueAttaque == 1) { // je peux attaquer
					him.GetComponent<Recuperable> ().cible_poursuite = otherGo;
					return;// on a trouvé une action pas besoin d'annalyser les autres
				}else if(valueAttaque == -1){ // je dois fuire
					// TODO ??? 
					bdv.objectif = ManageBehaviours.poinBbackToEnnemi(him,otherGo,bdv.rayonVue) ;
					bdv.agent.SetDestination(bdv.objectif);
					return;// on a trouvé une action pas besoin d'annalyser les autres
				}
				// else même equipe on ne fait rien.
			}
		}

	}
	
	private float getDistance(GameObject A, GameObject B){
		Transform tr1 = A.GetComponent<Transform> ();
		Transform tr2 = B.GetComponent<Transform> ();
		return Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
			+ (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
	}
}