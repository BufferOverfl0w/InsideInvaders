using UnityEngine;

public class PatrouilleCercle : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).

	public Vector3 centreOfSphere = Vector3.zero;
	public Vector3 objectif = Vector3.zero;
	public float speed = 5;
	public float angularSpeed = 5;
	public float acceleration = 5;
	public float rayon=50;
	private NavMeshAgent agent;

	// Varibale à utiliser pour stoper le comportement patrouille, pour attaquer ça cible. 
	public bool seeTarget = false;

	public NavMeshAgent getAgent(){
		return agent;
	}
	public void setAgent(NavMeshAgent newAgent){
		agent = newAgent;
	}
}