using UnityEngine;

public class BarreDeVie : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public int max_pv = 200;
	public float current_pv = 200;


	[HideInInspector] 
	public Vector3 centreOfSphere = Vector3.zero;
	[HideInInspector] 
	public Vector3 objectif = Vector3.zero;
	public float speed = 20;
	public float angularSpeed = 5;
	public float acceleration = 15;
	public float rayonPatrouille=50;
	public float rayonVue = 50;

	[HideInInspector] 
	public NavMeshAgent agent;
	[HideInInspector] 
	public float lastDistance =0.0f;
	[HideInInspector] 
	public float nbSecBlocked =0.0f;
	[HideInInspector] 
	public bool waiForMaj = false;
	// Varibale à utiliser pour stoper le comportement patrouille, pour attaquer ça cible. 
	//public bool seeTarget = false;


}