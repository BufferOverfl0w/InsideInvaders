using UnityEngine;

public class ControllableByKeyboard : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public float propulsionPower = 50.0f;
    public float maxSpeed = 100f;
	public float mouseSensibility = 150.0f;
	public bool inverseMouse = false;

}