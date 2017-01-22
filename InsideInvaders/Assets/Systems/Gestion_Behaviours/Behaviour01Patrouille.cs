using UnityEngine;
using FYFY;
using System.Collections;
using System.Collections.Generic;

public class Behaviour01Patrouille : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _allUnitsBehaviourGO = FamilyManager.getFamily(new AllOfComponents(typeof(Behaviour),typeof(NavMeshAgent)));

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
			if (go.GetComponent<Behaviour> ().index_currentBehaviour != 1) continue;
			updateComponents (go);
			patrouille (go);
			scanView (go);
		}
	}

	private void updateComponents(GameObject go )
	{
		Vivant bdv = null;
		NavMeshAgent newAgent = null;
		bdv = go.GetComponent<Vivant> ();
		if (bdv == null) {
			Debug.Log ("FUCK");
		}

		if (bdv.centreOfPatrouille == Vector3.zero) {
			bdv.centreOfPatrouille = go.transform.position; // on mets à jours le centre du cerle
			bdv.objectifCoord = go.transform.position;
			newAgent = go.GetComponent<NavMeshAgent> ();
			newAgent.speed = bdv.speedAgent;
			newAgent.angularSpeed = bdv.angularSpeedAgent;
			newAgent.acceleration = bdv.accelerationAgent;
			newAgent.SetDestination (bdv.objectifCoord);
			bdv.agent = newAgent;
		}
		int last_Behav = go.GetComponent<Behaviour> ().index_lastBehaviour;
		if (last_Behav != 1) {
			// on vient de rentrer dans le Behaviour01Patrouille
			bdv.waiForMajCentreOfPatrouille = true;
		}
		if (bdv.waiForMajCentreOfPatrouille) {
			Rigidbody go_rb = go.GetComponent<Rigidbody> ();
			float velocity = Mathf.Abs (go_rb.velocity.x) + Mathf.Abs (go_rb.velocity.z);
			if ((velocity>= 0.5f) && (velocity<=25.0f)){
				bdv.centreOfPatrouille = go.transform.position; // on mets à jours le centre du cerle
				bdv.objectifCoord = go.transform.position;
				bdv.agent.SetDestination (bdv.objectifCoord);
				bdv.waiForMajCentreOfPatrouille = false;
			}
		}
	}

	private bool isBlocked(NavMeshAgent agent,Vivant bdv){
		//Debug.Log ("isBlocked");

		float distance = Mathf.Sqrt ((agent.transform.position.x - bdv.objectifCoord.x) * (agent.transform.position.x - bdv.objectifCoord.x)
			+ (agent.transform.position.z - bdv.objectifCoord.z) * (agent.transform.position.z - bdv.objectifCoord.z));
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
		Vivant bdv = go.GetComponent<Vivant> ();
		if (!bdv.agent.enabled) return;

		float distance = Mathf.Sqrt ((go.transform.position.x - bdv.objectifCoord.x) * (go.transform.position.x - bdv.objectifCoord.x)
			+ (go.transform.position.z - bdv.objectifCoord.z) * (go.transform.position.z - bdv.objectifCoord.z));
		//Debug.Log ("distance " + distance);
		if( isBlocked( bdv.agent,bdv) || (distance<=2.5f) ){
			//Debug.Log ("in");
			Vector3 pos = (Random.insideUnitSphere * bdv.rayonPatrouille) + bdv.centreOfPatrouille;
			bdv.objectifCoord =  new Vector3 (pos.x, go.transform.position.y, pos.z);
			//Debug.Log (spe.objectif);
			bdv.agent.SetDestination(bdv.objectifCoord);
		}
	}

	private void scanView(GameObject him){
		
		Vivant bdv = him.GetComponent<Vivant> ();
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
			if (distance < bdv.rayonVueAlerte) {
				int valueAttaque = ManageBehaviours.myTargetIs (him, otherGo);
				if (valueAttaque == 1) { // je peux attaquer
					him.GetComponent<Behaviour> ().cible_poursuite = otherGo;
					return;// on a trouvé une action pas besoin d'annalyser les autres
				}else if(valueAttaque == -1){ // je dois fuire
			
					bdv.objectifCoord = ManageBehaviours.poinBbackToEnnemi(him,otherGo,bdv.rayonVueAlerte) ;
					bdv.agent.SetDestination(bdv.objectifCoord);
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