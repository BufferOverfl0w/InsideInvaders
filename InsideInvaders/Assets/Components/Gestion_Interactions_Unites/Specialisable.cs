﻿using UnityEngine;

public class Specialisable : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	//public Transform LymphocyteBViral;
	//public Transform LymphocyteBBacterien;
	public float rayon_effet = 30;
	public float vitesse_specialisation = 1 ;
	[HideInInspector]
	public float progres_spec_viral = 0;
	[HideInInspector]
	public float progres_spec_bacterien = 0;
}