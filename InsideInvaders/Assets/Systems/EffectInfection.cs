using UnityEngine;
using FYFY;

public class EffectInfection : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _infectableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Infectable)),new AllOfComponents(typeof(BarreDeVie)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		float val;;
		Infectable infectable;
		BarreDeVie vie;
		MeshRenderer[] meshList;
		Material[] lastListMat;
		foreach (GameObject go in _infectableGO) {
			infectable = go.GetComponent<Infectable> ();
			vie = go.GetComponent<BarreDeVie> ();
			meshList = go.GetComponentsInChildren<MeshRenderer>(true );
			foreach (MeshRenderer mesh in meshList) {
				lastListMat = mesh.materials;
				if (infectable.infecte) {
					val = (vie.current_pv * 1) / vie.max_pv;
					if (mesh.materials.Length == 1) {
						mesh.materials = new Material[] { lastListMat [0], CreateMat (val) };
					} else {
						mesh.materials[1].SetFloat("_Level",val);
					}
						
				}
			}
		}
	}

	private Material CreateMat(float value) {
		Material newMat = new Material(Shader.Find("Unlit/DissolveEffectShader"));
		newMat.SetTexture("_MainTex", Resources.Load("Textures/black") as Texture);
		newMat.SetTexture("_NoiseTex", Resources.Load("Textures/noisezoomsmooth") as Texture);
		newMat.SetColor("_EdgeColour1",new Color32(25,41,14,150));
		newMat.SetColor("_EdgeColour2",new Color32(62,189,57,150));
		newMat.SetFloat("_Level",value);
		newMat.SetFloat("_Edges",1.0f);
		return newMat;
	}
}