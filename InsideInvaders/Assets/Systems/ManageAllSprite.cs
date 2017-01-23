using UnityEngine;
using FYFY;
using UnityEngine.UI;
using ProgressBar;
using System.Collections;
using System.Collections.Generic;
public class ManageAllSprite : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.

	private Family _infectableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Infectable)));
	private Family _agglutinableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Agglutinable)));
	private Family _specialisableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Specialisable)));
	private Family _livingGO = FamilyManager.getFamily(new AllOfComponents(typeof(Vivant)),new NoneOfComponents(typeof(Toxique)), 
		new NoneOfTags("Dechet"),new NoneOfTags("Anticorps"));

	private Family _recuperableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Recuperable)));
	private Family _intrusGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamIntrus)));
	private Family _defensesGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamDefense)));
	private Family _ralentissableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Ralentissable)));
	private Family _behaviourGO = FamilyManager.getFamily(new AllOfComponents(typeof(Behaviour)));

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
		manageCercleSelection();
		manageCercleCible ();
		manageShield ();
		manageSlowingImg ();
		manageProgressAgglutinement ();
		manageProgressSpecialisation ();
	}

	private void manageCercleSelection(){
		foreach (GameObject go in _recuperableGO) {
			bool recup = go.GetComponent<Recuperable> ().recupere;
			Transform transform_Go = go.transform.Find("Canvas/cercle");
			if (transform_Go == null) continue;
			GameObject cercle_Go = transform_Go.gameObject;
			if (cercle_Go == null) continue;
			cercle_Go.SetActive (recup);
			Component halo = go.GetComponent("Halo"); 
			if (halo == null) continue;
			halo.GetType().GetProperty("enabled").SetValue(halo, recup, null);

		}
	}
	private void manageCercleCible(){
		List<GameObject> listPoursuivit = new List<GameObject>();
		foreach (GameObject go in _behaviourGO) {
			if (go.CompareTag ("Anticorps"))continue;
			GameObject poursuivit = go.GetComponent<Behaviour> ().cible_poursuite;
			if (poursuivit != null) {
				listPoursuivit.Add(poursuivit);
			}
		}
		foreach (GameObject intrus in _intrusGO) {
			Transform transform_Go = intrus.transform.Find("Canvas/cible");
			if (transform_Go == null) continue;
			GameObject cible_Go = transform_Go.gameObject;
			if (cible_Go == null) continue;
			cible_Go.SetActive (listPoursuivit.Contains(intrus));
		}
	}

	private void manageShield(){
		List<GameObject> listProtege = new List<GameObject>();
		foreach (GameObject go in _behaviourGO) {
			GameObject protege = go.GetComponent<Behaviour> ().cible_protection;
			if (protege != null) {
				listProtege.Add(protege);
			}
		}
		foreach (GameObject defense in _defensesGO) {
			Transform transform_Go = defense.transform.Find("Canvas/bouclier");
			if (transform_Go == null) continue;
			GameObject Shield_Go = transform_Go.gameObject;
			if (Shield_Go == null) continue;
			Shield_Go.SetActive (listProtege.Contains(defense));
		}
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
	private void manageProgressAgglutinement(){
		foreach (GameObject go in _agglutinableGO) {
			GameObject go_Progress = go.transform.Find ("Canvas/ProgressAgglutinement").gameObject;
			ProgressRadialBehaviour progressAggl = go_Progress.GetComponent<ProgressRadialBehaviour> ();
			Image img_progress = progressAggl.GetComponent<Image> ();
			Agglutinable inf = go.GetComponent<Agglutinable> ();
			float agglutinement = inf.progres_agglutinement;

			if (agglutinement <= 0.0f)  {
				img_progress.enabled = false;
				progressAggl.Value = 0.0f;
			} else {
				img_progress.enabled = true;
				float val = agglutinement;
				val = (val * 100 / Agglutinement.seuil_agglutinement);
				progressAggl.Value = val;
			}
			//Debug.Log ("value bar : " + progressInfect.Value);

		}
	}
	private void manageProgressSpecialisation(){
		foreach (GameObject go in _specialisableGO) {
			GameObject go_Progress = go.transform.Find ("Canvas/ProgressSpe").gameObject;
			ProgressRadialBehaviour progressSpe = go_Progress.GetComponent<ProgressRadialBehaviour> ();
			Image img_progress = progressSpe.GetComponent<Image> ();
			Specialisable inf = go.GetComponent<Specialisable> ();
			float spec_bact = inf.progres_spec_bacterien;
			float spec_viral = inf.progres_spec_viral;
			float value = (spec_bact >= spec_viral) ? spec_bact : spec_viral; // On affiche le plus avancé 

			if (value <= 0.0f)  {
				img_progress.enabled = false;
				progressSpe.Value = 0.0f;
			} else {
				img_progress.enabled = true;
				float val = value;
				val = (val * 100 / Specialisation.seuil_specialisation);
				progressSpe.Value = val;
			}
			//Debug.Log ("value bar : " + progressInfect.Value);

		}
	}
	private void manageHealthBar(){

		foreach (GameObject go in _livingGO) {
			float life = go.GetComponent<Vivant> ().current_pv;
			int maxPv = go.GetComponent<Vivant> ().max_pv;
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
	private void manageSlowingImg(){
		foreach (GameObject go in _ralentissableGO) {
			Transform transform_Go = go.transform.Find("Canvas/slow");
			if (transform_Go == null) continue;
			GameObject slow_Go = transform_Go.gameObject;
			if (slow_Go == null) continue;
			slow_Go.SetActive (go.GetComponent<Ralentissable>().ralenti);
		}

	}
}