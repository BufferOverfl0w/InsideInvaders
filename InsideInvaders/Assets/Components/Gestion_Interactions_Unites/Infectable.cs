using UnityEngine;

public class Infectable : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).

	[HideInInspector]
	public bool infecte = false;
	[HideInInspector]
	public float progres_infection = 0;
	public float timeForInfect = 5;  // in sec 
}