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
			//if (!go.name.Equals("Macrophage")) continue;
			newUnite (go);
		}
	}



	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		foreach (GameObject go in _patrGO) {
			//if (!go.name.Equals("Macrophage")) continue;
			PatrouilleCercle spe = go.GetComponent<PatrouilleCercle> ();
			if (spe.seeTarget) continue;
			if (spe.agent == null) newUnite (go);
			//NavMeshAgent agentTmp = spe.agent;
			//float distance = Vector3.Distance (agentTmp.destination, agentTmp.transform.position);
			float distance = Mathf.Sqrt ((go.transform.position.x - spe.objectif.x) * (go.transform.position.x - spe.objectif.x)
				+ (go.transform.position.z - spe.objectif.z) * (go.transform.position.z - spe.objectif.z));
			//Debug.Log ("distance " + distance);
			if( isBlocked( spe.agent,spe) || (distance<=2.5f) ){
				//Debug.Log ("in");
				Vector3 pos = (Random.insideUnitSphere * spe.rayon) + spe.centreOfSphere;
				spe.objectif =  new Vector3 (pos.x, go.transform.position.y, pos.z);
				//Debug.Log (spe.objectif);
				spe.agent.SetDestination(spe.objectif);
			}
		}
	}

	private void newUnite(GameObject go )
	{
		//Debug.Log ("NewUnit " +go.name);
		PatrouilleCercle spe = go.GetComponent<PatrouilleCercle> ();
		spe.centreOfSphere = go.transform.position;
		spe.objectif = go.transform.position;
		NavMeshAgent newAgent =  go.GetComponent<NavMeshAgent>();
//		if (newAgent == null) {
//			go.AddComponent (typeof(NavMeshAgent));
//			newAgent = go.GetComponent<NavMeshAgent>();
//		}
		newAgent.speed = spe.speed;
		newAgent.angularSpeed = spe.angularSpeed;
		newAgent.acceleration = spe.acceleration;
		newAgent.SetDestination (spe.objectif);
		spe.agent = newAgent;
	}
	private bool isBlocked(NavMeshAgent agent,PatrouilleCercle spe){
		//Debug.Log ("isBlocked");

		float distance = Mathf.Sqrt ((agent.transform.position.x - spe.objectif.x) * (agent.transform.position.x - spe.objectif.x)
			+ (agent.transform.position.z - spe.objectif.z) * (agent.transform.position.z - spe.objectif.z));
		bool sameDist = Mathf.Approximately (distance, spe.lastDistance);
		spe.lastDistance =  distance;
		if (sameDist) 
		{
			//Debug.Log ("Approximately");
			spe.nbSecBlocked = spe.nbSecBlocked + Time.deltaTime;
			//Debug.Log (" nbSecBlocked "+ spe.nbSecBlocked );
			if (spe.nbSecBlocked >= 2.0f) {// 2 sec
				spe.nbSecBlocked = 0.0f;
				return true;
			} else {
				return false;
			}
		}

		spe.nbSecBlocked = 0.0f;
		return false;
	}

}