using UnityEngine;
using System.Collections;

public class DebugPatrouille : MonoBehaviour {

	void OnDrawGizmos() {
		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
		foreach (GameObject obj in allObjects)
		{
			PatrouilleCercle spe = obj.GetComponent<PatrouilleCercle> ();
			if (spe != null) {
				Gizmos.color = new Color(255f,255f,0f,0.2f);
				Gizmos.DrawSphere (spe.centreOfSphere, spe.rayon);

				Gizmos.color = new Color(0f,255f,0f,0.2f);
				Gizmos.DrawSphere (spe.objectif, 5);
			}

		}
	}
}
