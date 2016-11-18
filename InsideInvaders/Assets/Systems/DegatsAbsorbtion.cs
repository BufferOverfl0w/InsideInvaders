﻿using UnityEngine;
using FYFY;

public class DegatsAbsorbtion : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _absorbableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Absorbable)));
	private Family _absorbeurGO = FamilyManager.getFamily(new AllOfComponents(typeof(Absorbeur)));
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		float rayon_effet = 30;
		int degats_absorbtion = 1;
		foreach (GameObject go1 in _absorbeurGO) {
			Transform tr1 = go1.GetComponent<Transform> ();
			foreach (GameObject go2 in _absorbableGO) {
				Transform tr2 = go2.GetComponent<Transform> ();
				float distance = Mathf.Sqrt ((tr1.position.x - tr2.position.x) * (tr1.position.x - tr2.position.x)
					+ (tr1.position.z - tr2.position.z) * (tr1.position.z - tr2.position.z));
				if (distance < rayon_effet) {
					Debug.Log ("je fais des degats");
					go2.GetComponent<BarreDeVie> ().pv -= degats_absorbtion;
				}
			}
		}
	}
}