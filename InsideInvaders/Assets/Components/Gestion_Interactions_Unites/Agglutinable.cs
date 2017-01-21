using UnityEngine;

public class Agglutinable : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	[HideInInspector]
	public Transform VirusAgglutine;
	[HideInInspector]
	public float progres_agglutinement = 0;
}