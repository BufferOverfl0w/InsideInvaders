using UnityEngine;

public class MouvantCible : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public float vitesse = 20;
	[HideInInspector]
	public Vector3 positionCible = Vector3.zero;
	public int rayon_detection = 100;
	public float periode = 3;
	public float temps = 4;
}