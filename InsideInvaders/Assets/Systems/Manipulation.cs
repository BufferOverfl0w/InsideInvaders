using UnityEngine;
using FYFY;

public class Manipulation : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _recuperableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Manipulable)));
	private Family _playerGO = FamilyManager.getFamily(new AllOfComponents(typeof(ControllableByKeyboard)));
	private Family _cameraGO = FamilyManager.getFamily(new AllOfComponents(typeof(CameraPlayer)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
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
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {
			GameObject go_hit = hit.transform.gameObject;
			foreach (GameObject go in _recuperableGO) {
				if (go.Equals(go_hit)) {
					if (Input.GetMouseButton (0)) {
						Rigidbody rb = go_hit.GetComponent<Rigidbody>();
						Vector3 v = tr.position - hit.transform.position;
						rb.AddForce (v);
					}
					else if(Input.GetMouseButton (1)) {
						Rigidbody rb = go_hit.GetComponent<Rigidbody>();
						Vector3 v = hit.transform.position - tr.position;
						rb.AddForce (v);
					}
				}
			}
		}
	}
}