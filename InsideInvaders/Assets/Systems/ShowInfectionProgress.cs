using UnityEngine;
using FYFY;

using UnityEngine.UI;
using ProgressBar;

public class ShowInfectionProgress : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.

	private Family _infectableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Infectable)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject go1 in _infectableGO) {
			GameObject go_Progress = go1.transform.Find ("Canvas/ProgressInfect").gameObject;
			ProgressRadialBehaviour progressInfect = go_Progress.GetComponent<ProgressRadialBehaviour> ();
			Image img_progress = progressInfect.GetComponent<Image> ();
			Infectable inf = go1.GetComponent<Infectable> ();
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
}