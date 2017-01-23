using UnityEngine;

public class Vivant : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public int max_pv = 200;
	public float current_pv = 200;
	public string type = "";

	//Parametre 
	[HideInInspector] 
	public Vector3 centreOfPatrouille = Vector3.zero;
	[HideInInspector] 
	public Vector3 objectifCoord = Vector3.zero;
	public float speedAgent = 20;
	public float angularSpeedAgent = 5;
	public float accelerationAgent = 15;
	public float rayonPatrouille=50;
	public float rayonVueAlerte = 50;

	[HideInInspector] 
	public NavMeshAgent agent;
	[HideInInspector] 
	public float lastDistance =0.0f;
	[HideInInspector] 
	public float nbSecBlocked =0.0f;
	[HideInInspector] 
	public bool waiForMajCentreOfPatrouille = false;
	// Varibale à utiliser pour stoper le comportement patrouille, pour attaquer ça cible. 
	//public bool seeTarget = false;


}