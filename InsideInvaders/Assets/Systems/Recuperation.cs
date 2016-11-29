using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class Recuperation : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _recuperableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Recuperable)));
	private Family _playerGO = FamilyManager.getFamily(new AllOfComponents(typeof(ControllableByKeyboard)));
	private Family _cameraGO = FamilyManager.getFamily(new AllOfComponents(typeof(CameraPlayer)));
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
		suivi (tr);
	}

	private void saisie(Transform tr, Camera camera){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,(Screen.height/2)+20,0));

		img_Cursor.color = Color.red;
		foreach (GameObject go in _recuperableGO)
			if (go.GetComponent<Recuperable> ().recupere == true){
				img_Cursor.color = Color.yellow;
					break;
			}
		if (Physics.Raycast (ray, out hit)) {
			GameObject go_hit = hit.transform.gameObject;
			if (_recuperableGO.contains (go_hit.GetInstanceID ())) { // test if is a Recuperable Object
				img_Cursor.color = Color.green;
				if (Input.GetMouseButton (0)) {
					go_hit.GetComponent<Recuperable> ().recupere = true;
				}
			}

//			foreach (GameObject go in _recuperableGO) {
//				if (go.Equals (go_hit)) {
//					img_Cursor.color = Color.green;
//					if (Input.GetMouseButton (0)) {
//						go_hit.GetComponent<Recuperable> ().recupere = true;
//					}
//				} else {
//					img_Cursor.color = Color.red;
//				}
//			}
		}
	}
	private void envoi(Camera camera){
		float force_envoi = 50f;

		//Si clic droit, envoi des unités vers pos curseur
		if (Input.GetMouseButton (1)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,(Screen.height/2)+20,0));
			if (Physics.Raycast (ray, out hit)) {

				foreach (GameObject go in _recuperableGO) {
					if (go.GetComponent<Recuperable> ().recupere == true) {
						Rigidbody rb = go.GetComponent<Rigidbody> ();
						Vector3 v = hit.point - go.transform.position;
						go.GetComponent<Recuperable> ().recupere = false;
						v.x = v.x * force_envoi;
						v.y = v.y * force_envoi;
						v.z = v.z * force_envoi;
						rb.AddForce (v);
					}
				}
			}
		}
	}
	private void suivi(Transform tr){
		float distance_de_suivi = 20;
		foreach (GameObject go in _recuperableGO) {
			if (go.GetComponent<Recuperable> ().recupere == true) {
				Transform tr2 = go.transform;
					
				float distance = Mathf.Sqrt ((tr.position.x - tr2.position.x) * (tr.position.x - tr2.position.x)
				               			 	 + (tr.position.z - tr2.position.z) * (tr.position.z - tr2.position.z));
				Rigidbody rb = go.GetComponent<Rigidbody> ();
				//Debug.Log ("test distance " + distance);

				if (distance > distance_de_suivi) {

					Vector3 v = tr.position - tr2.position;
					//v.x = v.x * (distance-distance_de_suivi)/100;
					//v.y = v.y * (distance-distance_de_suivi)/100;
					//v.z = v.z * (distance-distance_de_suivi)/100;
					//rb.AddForce (v);
					rb.velocity = v;
				}
				else {
					rb.velocity = Vector3.zero;
				}
			}
		}
	}
}
