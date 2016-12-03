using UnityEngine;

public class MouvantAleatoire : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).

	public float vitesse = 20;
	[HideInInspector]
	public Vector3 positionCible = Vector3.zero;
}