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
			if (spe.getAgent() == null) newUnite (go);
			float distance = Mathf.Sqrt ((go.transform.position.x - spe.objectif.x) * (go.transform.position.x - spe.objectif.x)
				+ (go.transform.position.z - spe.objectif.z) * (go.transform.position.z - spe.objectif.z));
			NavMeshAgent agentTmp = spe.getAgent ();
			if((distance) <= (agentTmp.radius*2)+10.0f){
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
		spe.setAgent(newAgent);
	}
		
}