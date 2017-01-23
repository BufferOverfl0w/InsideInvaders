using UnityEngine;
using FYFY;
using System.Collections;
using System.Collections.Generic;

public class Behaviour03Attaque : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _allUnitsGO = FamilyManager.getFamily(new AllOfComponents(typeof(Behaviour)));
	private Family _distanceUnitsGO = FamilyManager.getFamily(new AnyOfTags("longRange"));
	private static float delayShoot = 2.0f; // 2 sec

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) 
	{
		foreach (GameObject go in _allUnitsGO) 
		{
			if (go.GetComponent<Behaviour> ().index_currentBehaviour == EnumBehaviour.Attaque) 
			{
				//Debug.Log ("Dans la behaviour 3");
				if(_distanceUnitsGO.contains(go.GetInstanceID())){
					// l'unité attaque à distance
					attaqueDistance(go);
				}else
					poursuivre(go);
			}
		}
	}


	private void poursuivre(GameObject go){
		float distance_de_suivi = 0;
		Vector3 posEnnemi = go.GetComponent<Behaviour> ().cible_poursuite.transform.position;
		Vector3 myPostion = go.transform.position;
		float distance = Mathf.Sqrt ((posEnnemi.x - myPostion.x) * (posEnnemi.x - myPostion.x)
			+ (posEnnemi.z - myPostion.z) * (posEnnemi.z - myPostion.z));
		Rigidbody rb = go.GetComponent<Rigidbody> ();
		//Debug.Log ("test distance " + distance);

		if (distance > distance_de_suivi) {

			Vector3 v = posEnnemi - myPostion;
			//v.x = v.x * (distance-distance_de_suivi)/100;
			//v.y = v.y * (distance-distance_de_suivi)/100;
			//v.z = v.z * (distance-distance_de_suivi)/100;
			//rb.AddForce (v);
			rb.velocity = v;
		} else {
			rb.velocity = Vector3.zero;
		}
	}

	private void attaqueDistance(GameObject go){
		Behaviour myBehaviour = go.GetComponent<Behaviour> ();
		EnumBehaviour last_Behav = myBehaviour.index_lastBehaviour;
		if (last_Behav != EnumBehaviour.Attaque) {
			// on vient de rentrer dans le Behaviour03Attaque
			// pour ne lancer qu'un Anticorps par attaque ennemis
			GameObject newAnticorps=null;
			GameObject cible = myBehaviour.cible_poursuite;
			bool isSpeBact = (go.GetComponent<FactoryAnticorpsBact> () != null) ? true : false;
			string namePrefabs = (isSpeBact) ? "Prefabs/Anticorps SpeBacterien" : "Prefabs/Anticorps SpeViral";
			Dictionary<GameObject,float> dico = (isSpeBact) ? go.GetComponent<FactoryAnticorpsBact> ().listTime:
				go.GetComponent<FactoryAnticorpsViral> ().listTime;
			bool existe = dico.ContainsKey (cible);
			float lastTime = -1.0f;
			if (existe) 
				lastTime = dico[cible];

			if (!existe) {
				newAnticorps = GameObjectManager.instantiatePrefab (namePrefabs);
				if (isSpeBact)
					go.GetComponent<FactoryAnticorpsBact> ().listTime.Add (cible, 0.0f);
				else
					go.GetComponent<FactoryAnticorpsViral> ().listTime.Add (cible, 0.0f);
			} else {
				if (lastTime > delayShoot) {
					newAnticorps = GameObjectManager.instantiatePrefab (namePrefabs);
					if (isSpeBact)
						go.GetComponent<FactoryAnticorpsBact> ().listTime [cible] = 0.0f;
					else
						go.GetComponent<FactoryAnticorpsViral> ().listTime [cible] = 0.0f;
				} else {
					if (isSpeBact)
						go.GetComponent<FactoryAnticorpsBact> ().listTime [cible] = lastTime + Time.deltaTime;
					else
						go.GetComponent<FactoryAnticorpsViral> ().listTime [cible] = lastTime + Time.deltaTime;
				}
			}
			
			if (newAnticorps != null) {
				Vector3 vect = go.transform.position;
				vect.x = vect.x + ((Random.value < 0.50f) ? -5.5f :5.5f);
				vect.z = vect.z + ((Random.value < 0.50f) ? -5.5f :5.5f);
				newAnticorps.transform.position = vect;

				// on stop l'attaque des Lymphocyte, les anticorps ferons le reste
				newAnticorps.GetComponent<Behaviour>().cible_poursuite = myBehaviour.cible_poursuite;
			}
			myBehaviour.cible_poursuite = null; 

		}

	

	}
}