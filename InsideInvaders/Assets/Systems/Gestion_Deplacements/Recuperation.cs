using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class Recuperation : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _recuperableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Recuperable)));
	private Family _playerGO = FamilyManager.getFamily(new AllOfComponents(typeof(ControllableByKeyboard)));
	private Family _cameraGO = FamilyManager.getFamily(new AllOfComponents(typeof(CameraPlayer)));
	private Family _intrusGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamIntrus)));
	private Family _defensesGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamDefense)));

	private Image img_Cursor;
	private static float maxSpeed = 300.0f;

	protected override void onPause(int currentFrame) {

	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
		img_Cursor = GameObject.FindGameObjectWithTag ("cursor_Image").GetComponent<Image>();
		img_Cursor.color = Color.red;
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		Camera camera = null;
		Transform tr = null; //position player


		foreach (GameObject go in _playerGO) {
			tr = go.GetComponent<Transform> ();
		}
		foreach (GameObject go in _cameraGO) {
			camera = go.GetComponent<Camera> ();
		}

		saisie (tr, camera);
		envoi (camera);
	}

	private void saisie(Transform tr, Camera camera){
		RaycastHit hit;
		Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width/2,(Screen.height/2)+20,0));

		img_Cursor.color = Color.yellow;
		if (Physics.Raycast (ray, out hit)) {
			GameObject go_hit = hit.transform.gameObject;
			if (_recuperableGO.contains (go_hit.GetInstanceID ())) { // test if is a Recuperable Object
				img_Cursor.color = Color.green;
				if (Input.GetMouseButton (0)) {
					go_hit.GetComponent<Recuperable> ().recupere = true;
					go_hit.GetComponent<Behaviour> ().cible_poursuite = null;
					go_hit.GetComponent<Behaviour> ().cible_protection = null;

				}
			} else if (_intrusGO.contains (go_hit.GetInstanceID ())) {
				img_Cursor.color = Color.red;

			}
		}
	}
	private void envoi(Camera camera){
		//float force_envoi = 50f;

		//Si clic droit, envoi des unités vers pos curseur
		if (Input.GetMouseButton (1)) {
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width/2,(Screen.height/2)+20,0));
			if (Physics.Raycast (ray, out hit)) {
				GameObject go_hit = hit.transform.gameObject;
				if (_defensesGO.contains (go_hit.GetInstanceID ())) { // si on a ciblé une unité alliée
					foreach (GameObject go in _recuperableGO) {
						if (go.GetComponent<Recuperable> ().recupere == true) {

							//Test si le joueur à cliqué sur la même unité ( une unité ne peux pas se defendre elle même
							if(!go_hit.Equals(go)){
								go.GetComponent<Recuperable> ().recupere = false;
								go.GetComponent<Behaviour> ().cible_protection = go_hit;
								propulse (go, hit.point);
							}
						}
					}
				} else if (_intrusGO.contains (go_hit.GetInstanceID ())) { // si on a ciblé une unité ennemie
					//Debug.Log("Unité ennemie ciblée");
					foreach (GameObject go in _recuperableGO) {
						if (go.GetComponent<Recuperable> ().recupere == true) {
							go.GetComponent<Recuperable> ().recupere = false;
							bool isLymphSpe = go.CompareTag ("longRange");
							bool canWin = (ManageBehaviours.myTargetIs (go, go_hit) == 1);

							if ((!isLymphSpe) || (isLymphSpe && (canWin))) {
								// on donne une cible si go n'est pas un Lynphosite Spé
								// ou, on lance un Anticorps si c'est la bonne cible
								go.GetComponent<Behaviour> ().cible_poursuite = go_hit;
							}

							if ((!isLymphSpe) || (isLymphSpe&&(!canWin))) {
								// on lance l'unité si go n'est pas un Lynphosite Spé
								// ou, on lance l'unité si go est un Lynphosite Spé avec une mauvaise cible 
								// ( pas d'anticorps de crée) 
								propulse (go, hit.point);
							}

						}
					}
				} else {
					foreach (GameObject go in _recuperableGO) {
						if (go.GetComponent<Recuperable> ().recupere == true) {
							go.GetComponent<Recuperable> ().recupere = false;
							propulse (go, hit.point);
						}
					}
				}
			}
		}
	}
	private static void propulse(GameObject him, Vector3 target){
		Rigidbody rb = him.GetComponent<Rigidbody> ();
		if (rb == null) return;
		Vector3 v = target - him.transform.position;
		rb.velocity = v;
		if (rb.velocity.magnitude > Recuperation.maxSpeed) { // trop rapide !
			rb.velocity = rb.velocity.normalized * Recuperation.maxSpeed;
		}
	
		//Vector3 v = (hit.point + hit.normal) - go.transform.position;
		//							foreach (GameObject player in _playerGO) {
		//								player.GetComponent<ControllableByKeyboard> ().point = (hit.point + hit.normal);
		//							}
		//v.x = v.x * force_envoi;
		//v.y = 0;
		//v.z = v.z * force_envoi;
		//float vitesse = Mathf.Sqrt (v.x * v.x + v.z * v.z);
		//if (vitesse > 8000) {
		//	v.x /= (vitesse / 8000);
		//	v.z /= (vitesse / 8000);
		//}
		//rb.velocity = Vector3.zero;
		//rb.AddForce (v);

	}
}
