using UnityEngine;
using System.Collections;

public class UnderWater : MonoBehaviour {

	//This script enables underwater effects. Attach to main camera.

	//Define variable
	public Transform waterTransform;
	public Color underWater_fogColor = new Color(0, 0.4f, 0.7f, 0.6f) ;
	public float underWater_fogIntensity = 0.04f; 
	private float underwaterLevel= 7;

	//The scene's default fog settings
	private bool defaultFog;
	private Color defaultFogColor;
	private float defaultFogDensity;
	//private Material defaultSkybox;

	void Start () {
		//Set the background color*
		defaultFog = RenderSettings.fog;
		defaultFogColor = RenderSettings.fogColor;
		defaultFogDensity = RenderSettings.fogDensity;
		//defaultSkybox = RenderSettings.skybox;

		underwaterLevel = waterTransform.localPosition.y;
	}

	void Update () {
		if (transform.position.y < underwaterLevel)
		{
			RenderSettings.fog = true;
			RenderSettings.fogColor = underWater_fogColor;
			RenderSettings.fogDensity = underWater_fogIntensity;
		}
		else
		{
			RenderSettings.fog = defaultFog;
			RenderSettings.fogColor = defaultFogColor;
			RenderSettings.fogDensity = defaultFogDensity;
		}
	}
}