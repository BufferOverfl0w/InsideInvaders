using UnityEngine;
using FYFY;
using System.Collections;

public class ManagePatrouille : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.

	private Family _patrGO = FamilyManager.getFamily(new AllOfComponents(typeof(PatrouilleCercle)));
	//private float altY;
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){

		//altY = GameObject.FindGameObjectWithTag ("NavMesh").transform.position.y;
		foreach (GameObject go in _patrGO) {
			newUnite (go);
		}
	}



	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		foreach (GameObject go in _patrGO) {
			PatrouilleCercle spe = go.GetComponent<PatrouilleCercle> ();
			if (spe.seeTarget) continue;
			if (spe.agent == null) newUnite (go);
			NavMeshAgent agentTmp = spe.agent;
			float distance = Vector3.Distance (agentTmp.destination, agentTmp.transform.position);
			if( (distance<=10.0f) || isBlocked(agentTmp,spe) ){
				Vector3 pos = (Random.insideUnitSphere * spe.rayon) + spe.centreOfSphere;
				spe.objectif =  new Vector3 (pos.x, go.transform.position.y, pos.z);
				//Debug.Log (spe.objectif);
				agentTmp.SetDestination(spe.objectif);
			}
		}
	}

	private void newUnite(GameObject go )
	{
		PatrouilleCercle spe = go.GetComponent<PatrouilleCercle> ();
		spe.centreOfSphere = go.transform.position;
		spe.objectif = go.transform.position;
		NavMeshAgent newAgent =  go.GetComponent<NavMeshAgent>();
		if (newAgent == null) {
			go.AddComponent (typeof(NavMeshAgent));
			newAgent = go.GetComponent<NavMeshAgent>();
		}
		newAgent.speed = spe.speed;
		newAgent.angularSpeed = spe.angularSpeed;
		newAgent.acceleration = spe.acceleration;
		spe.agent = newAgent;
	}
	private bool isBlocked(NavMeshAgent agent,PatrouilleCercle spe){

		float distance = Vector3.Distance (agent.destination, agent.transform.position);
		if (Mathf.Approximately (distance, spe.lastDistance)) 
		{
			spe.nbSecBlocked = spe.nbSecBlocked + Time.deltaTime;
			return (spe.nbSecBlocked >= 2); // 2 sec
		}
		spe.nbSecBlocked = 0.0f;
		spe.lastDistance =  distance;
		return false;
	}

}