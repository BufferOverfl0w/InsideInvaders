﻿using UnityEngine;
using FYFY;


public enum EnumBehaviour {
	Patrouille = 1,
	SuiviJoueur = 2,
	Attaque = 3,
	Protection = 4
}
public class ManageBehaviours : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.

	private Family _behaviourGO = FamilyManager.getFamily(new AllOfComponents(typeof(Behaviour)));
	//private Family _intrusGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamIntrus),typeof(Behaviour)));
	//private Family _defensesGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamDefense),typeof(Behaviour)),new NoneOfComponents(typeof(ControllableByKeyboard)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		EnumBehaviour last_index;

		//maj de LastBehaviour
		foreach (GameObject go in _behaviourGO) {
			Behaviour behav = go.GetComponent<Behaviour> ();
			last_index = behav.index_currentBehaviour;
			behav.index_lastBehaviour = last_index;
		}

//		foreach (GameObject go in _defensesGO) {
//			Behaviour behav = go.GetComponent<Behaviour> ();
//			last_index = behav.index_currentBehaviour;
//			behav.index_lastBehaviour = last_index;
//		}

		//1-Patrouille
		foreach (GameObject go in _behaviourGO) {
			//Quelques booléens préliminaires
			bool en_deplacement = true;
			bool pas_de_cible = false;
			Rigidbody go_rb = go.GetComponent<Rigidbody> ();
			Behaviour behavior = go.GetComponent<Behaviour> ();
			float velocity = Mathf.Abs (go_rb.velocity.x) + Mathf.Abs (go_rb.velocity.z);
			if ((velocity>= 0.5f) && (velocity<=12.0f)){
				en_deplacement = false;
			}
			if ((behavior.cible_poursuite == null) && (behavior.cible_protection == null)) {
				pas_de_cible = true;
			}

			if (/*(go.GetComponent<Recuperable> ().recupere == false)&&*/(!en_deplacement)&&(pas_de_cible)) {
				behavior.index_currentBehaviour = EnumBehaviour.Patrouille;
			} else if ((pas_de_cible)&&((behavior.index_lastBehaviour==EnumBehaviour.SuiviJoueur)||(behavior.index_lastBehaviour==EnumBehaviour.Attaque))){
				behavior.index_currentBehaviour = EnumBehaviour.Patrouille;
			}
		}

		//2-Suivi joueur
		foreach (GameObject go in _behaviourGO) {
			Recuperable recuperable = go.GetComponent<Recuperable> ();
			bool recup = (recuperable == null) ? false : recuperable.recupere; 
			// Si une unité n'est pas récuperable alors n'est pas récupéré 
			if (recup) {
				go.GetComponent<Behaviour> ().index_currentBehaviour = EnumBehaviour.SuiviJoueur;
			}
		}
		//3-Poursuite
		foreach (GameObject go in _behaviourGO) {
			if (go.GetComponent<Behaviour> ().cible_poursuite != null) {
				go.GetComponent<Behaviour> ().index_currentBehaviour = EnumBehaviour.Attaque;
			}
		}
		//4-Protection

		foreach (GameObject go in _behaviourGO) {
			if (go.GetComponent<Behaviour> ().cible_protection != null) {
				go.GetComponent<Behaviour> ().index_currentBehaviour = EnumBehaviour.Protection;
			}
		}

//		foreach (GameObject go in _behaviourGO) {
//			Debug.Log ("Current : "+go.GetComponent<Behaviour> ().index_currentBehaviour);
//			Debug.Log ("Last : "+go.GetComponent<Behaviour> ().index_lastBehaviour);
//		}

	}

	//-----------------------Tools for all Behaviors-----------------------
	/***
		 *  return 0 si A et B sont allié
		 * return 1 si A peut attaquer B
		 * return -1 si A doit fuire B
		 * ***/
	public static int myTargetIs(GameObject goA, GameObject goB){
		
		return whatIs (goA, goB, -1);
	}

	private static int whatIs(GameObject goA, GameObject goB,int val){
		/***
		 *  return 0 si A et B sont allié
		 * return 1 si A peut attaquer B
		 * return -1 si A doit fuire B
		 * ***/
		bool AIsAbsorbable = goA.GetComponent<Absorbable> () != null;
		bool BIsAbsorbeur = goB.GetComponent<Absorbeur> () != null;
		if (AIsAbsorbable && BIsAbsorbeur)
			return val;
		//Debug.Log ("1 ");
		bool AIsAgglutinable = goA.GetComponent<Agglutinable> () != null;
		bool BIsAgglutineur = goB.GetComponent<Agglutineur> () != null;
		if (AIsAgglutinable && BIsAgglutineur)
			return val;
		//Debug.Log ("2 ");
		bool AIsInfectable = goA.GetComponent<Infectable> () != null;
		bool BIsInfectieux = goB.GetComponent<Infectieux> () != null;
		if (AIsInfectable && BIsInfectieux)
			return val;
		//Debug.Log ("3");
		bool AIsRalentissable = goA.GetComponent<Ralentissable> () != null;
		bool BIsRalentisseur = goB.GetComponent<Ralentisseur> () != null;
		if (AIsRalentissable && BIsRalentisseur)
			return val;
		//Debug.Log ("4");
		bool AIsSpecialisant = goA.GetComponent<Specialisant> () != null;
		bool BIsSpecialisable = goB.GetComponent<Specialisable> () != null;
		if (AIsSpecialisant && BIsSpecialisable)
			return val;
		//Debug.Log ("5");
		bool AIsDestructable = ((goA.GetComponent<Infectable>() != null)&& (goA.GetComponent<Infectable>().infecte==true));
		bool BIsDestructeur = goB.GetComponent<Destructeur> () != null;
		if (AIsDestructable && BIsDestructeur)
			return val;
		//Debug.Log ("6");
		bool AIsToxiquable = ((goA.GetComponent<TeamDefense>() != null)&& (goA.GetComponent<Absorbeur>()== null)); 
		bool BIsToxique = goB.GetComponent<Toxique> () != null;
		if (AIsToxiquable && BIsToxique) {
			//Debug.Log ("val : " + val);
			if (val == -1) {
				// on laisse l'IA aller dans les toxines
			}else
				return val; 
		}
		//Debug.Log ("7");
		// Les bacteries non ralenti doivent fuire le Lymphocyte B Bactérien
		bool AIsRalentissable2 = (goA.GetComponent<Ralentissable> () != null)&&(!goA.GetComponent<Ralentissable> ().ralenti);
		bool BIsRalentisseur2 = goB.GetComponent<FactoryAnticorpsBact> () != null ;
		if (AIsRalentissable2 && BIsRalentisseur2) {
			//Debug.Log ("ici " + val);
			return val;
		}
			
	
		// Les virus doivent fuire le Lymphocyt B Viral
		bool AIsAgglutinable2 = (goA.GetComponent<Agglutinable> () != null)&&(goA.GetComponent<Agglutinable> ().progres_agglutinement==0);
		bool BIsAgglutineur2 = goB.GetComponent<FactoryAnticorpsViral> () != null ;
		if (AIsAgglutinable2 && BIsAgglutineur2) {
			//Debug.Log ("ici2 " + val);
			return val;
		}
			
		
		//Si on arrive ici c'est que A n'a pas besoin de fuire B, olors on test si il peut l'attaquer
		if (val == -1) { // test si on est à la 1er récursion
			return whatIs (goB, goA, 1); // A devient B et B devient A
		}

		// Si on arrive ici c'est que l'on est à la 2eme récursion et que A n'a pas besoin de fuire B, mais que A ne peut attaquer B 
		return 0; // A et B sont allié
	}

	public static Vector3 poinBbackToEnnemi(GameObject him, GameObject ennemi,float distanceBack){
		Vector3 tr1 = him.GetComponent<Transform>().position;
		Vector3 tr2 = ennemi.GetComponent<Transform>().position;

		Vector3 heading = tr1 - tr2;  // direction opposé à l'ennemi
		Ray r = new Ray (tr1, heading);
		Vector3 tpoint = r.GetPoint(distanceBack);
		return new Vector3 (tpoint.x, tr1.y, tpoint.z);
	}
}