using UnityEngine;
using System.Collections;

public class DebugPatrouille : MonoBehaviour {

	void OnDrawGizmos() {
		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
		foreach (GameObject obj in allObjects)
		{
			Behaviour behav = obj.GetComponent<Behaviour> ();
			if (behav == null) continue;
			if (behav.index_currentBehaviour != EnumBehaviour.Patrouille) continue;
			Vivant bdv = obj.GetComponent<Vivant> ();
			if (bdv != null) {
				Gizmos.color = new Color(255f,255f,0f,0.2f);
				Gizmos.DrawSphere (bdv.centreOfPatrouille, bdv.rayonPatrouille);  // on dessine le cercle de la patrouille

				Gizmos.color = new Color(0f,255f,0f,0.2f);
				Gizmos.DrawSphere (bdv.objectifCoord, 5);  // on dessine l'objectif

				Gizmos.color = new Color(0f,0.0f,255f,0.2f);
				Gizmos.DrawSphere (obj.transform.position, bdv.rayonVueAlerte);  // on dessine le cercle de vision.
			}

		}
	}
}
