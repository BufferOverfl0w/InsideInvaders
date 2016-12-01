using UnityEngine;

public class Infectable : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public bool infecte = false;
	public float progres_infection = 0;
	public float timeForInfect = 5;  // in sec 
}