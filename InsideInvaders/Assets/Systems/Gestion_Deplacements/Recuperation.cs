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
					go_hit.GetComponent<Recuperable> ().cible_poursuite = null;
					go_hit.GetComponent<Recuperable> ().cible_protection = null;

				}
			} else if (_intrusGO.contains (go_hit.GetInstanceID ())) {
				img_Cursor.color = Color.red;

			}
		}
	}
	private void envoi(Camera camera){
		float force_envoi = 50f;

		//Si clic droit, envoi des unités vers pos curseur
		if (Input.GetMouseButton (1)) {
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width/2,(Screen.height/2)+20,0));
			if (Physics.Raycast (ray, out hit)) {
				GameObject go_hit = hit.transform.gameObject;
				if (_defensesGO.contains (go_hit.GetInstanceID ())) { // si on a ciblé une unité alliée
					foreach (GameObject go in _recuperableGO) {
						if (go.GetComponent<Recuperable> ().recupere == true) {

							//Test si le joueur à cliqué sur la même unité ( une unité ne peux pas se defendre seul
							if(!go_hit.Equals(go)){
								Rigidbody rb = go.GetComponent<Rigidbody> ();
								Vector3 v = hit.point - go.transform.position;
								go.GetComponent<Recuperable> ().recupere = false;
								go.GetComponent<Recuperable> ().cible_protection = go_hit;
								v.x = v.x * force_envoi;
								v.y = 0;
								v.z = v.z * force_envoi;
								float vitesse = Mathf.Sqrt (v.x * v.x + v.z * v.z);
								if (vitesse > 8000) {
									v.x /= (vitesse / 8000);
									v.z /= (vitesse / 8000);
								}
								rb.velocity = Vector3.zero;
								rb.AddForce (v);
							}
						}
					}
				} else if (_intrusGO.contains (go_hit.GetInstanceID ())) { // si on a ciblé une unité ennemie
					Debug.Log("Unité ennemie ciblée");
					foreach (GameObject go in _recuperableGO) {
						if (go.GetComponent<Recuperable> ().recupere == true) {

							Rigidbody rb = go.GetComponent<Rigidbody> ();
							Vector3 v = hit.point - go.transform.position;
							go.GetComponent<Recuperable> ().recupere = false;
							go.GetComponent<Recuperable> ().cible_poursuite = go_hit;
							v.x = v.x * force_envoi;
							v.y = 0;
							v.z = v.z * force_envoi;
							float vitesse = Mathf.Sqrt (v.x * v.x + v.z * v.z);
							if (vitesse > 8000) {
								v.x /= (vitesse / 8000);
								v.z /= (vitesse / 8000);
							}
							rb.velocity = Vector3.zero;
							rb.AddForce (v);
						}
					}
				} else {
					foreach (GameObject go in _recuperableGO) {
						if (go.GetComponent<Recuperable> ().recupere == true) {
							Rigidbody rb = go.GetComponent<Rigidbody> ();
							Vector3 v = hit.point - go.transform.position;
							go.GetComponent<Recuperable> ().recupere = false;
							v.x = v.x * force_envoi;
							v.y = 0;
							v.z = v.z * force_envoi;
							float vitesse = Mathf.Sqrt (v.x * v.x + v.z * v.z);
							if (vitesse > 8000) {
								v.x /= (vitesse / 8000);
								v.z /= (vitesse / 8000);
							}

							rb.velocity = Vector3.zero;
							rb.AddForce (v);
						}
					}
				}
			}
		}
	}
}
