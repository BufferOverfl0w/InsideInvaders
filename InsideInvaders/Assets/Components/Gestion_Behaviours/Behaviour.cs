using UnityEngine;

public class Behaviour : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	[HideInInspector] 
	public int index_currentBehaviour = 0;
	[HideInInspector] 
	public int index_lastBehaviour = 0;

	[HideInInspector] 
	public GameObject cible_poursuite = null;
	[HideInInspector] 
	public GameObject cible_protection = null;
}