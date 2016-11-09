using UnityEngine;

public class ControllableByKeyboard : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public float speed = 40.0f;
	public float maxSpeed = 60f;
	public float mouseSensibility = 10.0f;
	public bool inverseMouse = false;
}