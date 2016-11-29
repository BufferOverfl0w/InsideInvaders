using UnityEngine;
using FYFY;

public class BloodEffect : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.

	private Family _cameraGO = FamilyManager.getFamily(new AllOfComponents(typeof(CameraPlayer)));
	GameObject Go_light = GameObject.FindGameObjectWithTag("Light");
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){

		CameraPlayer component = null;
		Camera cam = null;
		foreach (GameObject go in _cameraGO) {
			component = go.GetComponent<CameraPlayer> ();

			cam = go.GetComponent<Camera> ();
			cam.clearFlags = CameraClearFlags.SolidColor;
			cam.backgroundColor = component.skyBoxColor;

			RenderSettings.fog = true;
			RenderSettings.fogColor =component.fogColor;
			RenderSettings.fogDensity =component.fogIntensity;

			Light light = Go_light.GetComponent<Light> ();
			light.color = component.LightColor;
			light.intensity = 0.3f;
		}
	}

}