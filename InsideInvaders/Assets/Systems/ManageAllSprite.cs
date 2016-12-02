using UnityEngine;
using FYFY;
using UnityEngine.UI;
using ProgressBar;

public class ManageAllSprite : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.

	private Family _infectableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Infectable)));
	private Family _livingGO = FamilyManager.getFamily(new AllOfComponents(typeof(BarreDeVie)),new NoneOfComponents(typeof(Toxique)));
	Gradient g;
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
		GradientColorKey[] gck;
		GradientAlphaKey[] gak;

		g = new Gradient();
		gck = new GradientColorKey[2];
		gck[0].color = Color.green;
		gck[0].time = 1.0F;
		gck[1].color = Color.red;
		gck[1].time = 0.2F;

		gak = new GradientAlphaKey[2];
		gak[0].alpha = 100.0F;
		gak[0].time = 0.0F;
		gak[1].alpha = 100.0F;
		gak[1].time = 1.0F;
		g.SetKeys(gck, gak);
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		manageProgressInfections ();
		manageHealthBar ();
	}


	private void manageProgressInfections(){
		foreach (GameObject go in _infectableGO) {
			
			GameObject go_Progress = go.transform.Find ("Canvas/ProgressInfect").gameObject;
			ProgressRadialBehaviour progressInfect = go_Progress.GetComponent<ProgressRadialBehaviour> ();
			Image img_progress = progressInfect.GetComponent<Image> ();
			Infectable inf = go.GetComponent<Infectable> ();
			float infection = inf.progres_infection;

			if ((infection <= 0.0f) || (infection >= inf.timeForInfect))  {
				img_progress.enabled = false;
				progressInfect.Value = 0.0f;
			} else {
				img_progress.enabled = true;
				float val = infection;
				val = (val * 100 / inf.timeForInfect);
				progressInfect.Value = val;
			}
			//Debug.Log ("value bar : " + progressInfect.Value);

		}
	}

	private void manageHealthBar(){

		foreach (GameObject go in _livingGO) {
			float life = go.GetComponent<BarreDeVie> ().current_pv;
			int maxPv = go.GetComponent<BarreDeVie> ().max_pv;
			GameObject healthBar_Go = go.transform.Find ("Canvas/healthBar").gameObject;
			ProgressBarBehaviour healthBar = healthBar_Go.GetComponent<ProgressBarBehaviour> ();
			GameObject Filler  = healthBar_Go.transform.Find ("Filler").gameObject;
			Image img = Filler.GetComponent<Image> ();
			float val;
			if (life >=maxPv) {
				healthBar_Go.SetActive (false);
				healthBar.Value = 100.0F;
			} else {
				healthBar_Go.SetActive (true);
				val = (life * 100) / maxPv;
				//val = 100 - val;
				//Debug.Log ("val " + val);
				healthBar.Value = val;

				img.color =  g.Evaluate (val/100);
			}
		}
	}
}