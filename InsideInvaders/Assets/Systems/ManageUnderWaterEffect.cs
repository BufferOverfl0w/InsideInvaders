using UnityEngine;
using FYFY;

public class ManageUnderWaterEffect : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.

	private Family _cameraGO = FamilyManager.getFamily(new AllOfComponents(typeof(CameraPlayer)));

	//The scene's default fog settings
	private bool defaultFog;
	private Color defaultFogColor;
	private float defaultFogDensity;
	//private Material defaultSkybox;


	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){

		//Set the background color*
		defaultFog = RenderSettings.fog;
		defaultFogColor = RenderSettings.fogColor;
		defaultFogDensity = RenderSettings.fogDensity;
		//defaultSkybox = RenderSettings.skybox;

	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		
		CameraPlayer component = null;
		foreach (GameObject go in _cameraGO) {
			component = go.GetComponent<CameraPlayer> ();
			RenderSettings.fog = true;
			RenderSettings.fogColor =component.fogColor;
			RenderSettings.fogDensity =component.fogIntensity;
			//RenderSettings.skybox = null;
		}
			
	}
}