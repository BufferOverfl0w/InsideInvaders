using UnityEngine;

public class CameraPlayer : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).

	public Color fogColor = new Color(0, 0.4f, 0.7f, 0.6f) ;
	public Color skyBoxColor = new Color(0, 0.4f, 0.7f, 0.6f) ;
	public float fogIntensity = 0.001f; 
	public Color LightColor = new Color(0, 0.4f, 0.7f, 0.6f) ;
}