using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class Manipulation : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _recuperableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Manipulable)));
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

		RaycastHit hit;
		foreach (GameObject go in _playerGO) {
			tr = go.GetComponent<Transform> ();
		}
		foreach (GameObject go in _cameraGO) {
			camera = go.GetComponent<Camera> ();
		}
		Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width/2,(Screen.height/2)+20,0));

		if (Physics.Raycast (ray, out hit)) {
			GameObject go_hit = hit.transform.gameObject;
			foreach (GameObject go in _recuperableGO) {
				if (go.Equals (go_hit)) {
					img_Cursor.color = Color.green;
					if (Input.GetMouseButton (0)) {
						Rigidbody rb = go_hit.GetComponent<Rigidbody> ();
						Vector3 v = tr.position - hit.transform.position;
						rb.AddForce (v);
					} else if (Input.GetMouseButton (1)) {
						Rigidbody rb = go_hit.GetComponent<Rigidbody> ();
						Vector3 v = hit.transform.position - tr.position;
						rb.AddForce (v);
					}
				} else {
					img_Cursor.color = Color.red;
				}
			}
		}
	}
}