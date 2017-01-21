using UnityEngine;

public class Recuperable : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	[HideInInspector]
	public bool recupere = false;
	public GameObject cible_poursuite = null;
	public GameObject cible_protection = null;
}