using UnityEngine;
using FYFY;

public class ManagePlayerMouvement : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _controlableGO = FamilyManager.getFamily(new AllOfComponents(typeof(ControllableByKeyboard)));
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
		Cursor.visible = false;
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		foreach (GameObject go in _controlableGO) {
			movePlayer (go);
			rotatePlayer (go);
		}
	}


	private void movePlayer(GameObject go){
		Rigidbody rb = go.GetComponent<Rigidbody> ();
		Transform tr = go.GetComponent<Transform> ();
		ControllableByKeyboard controlBKey = go.GetComponent<ControllableByKeyboard> ();

		float speedTranslation = controlBKey.speed *100* rb.mass *Time.deltaTime ;
		float speedRotation = controlBKey.speed *Time.deltaTime*2 ;

		if (Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.UpArrow)) {
			rb.AddForce (tr.up * speedTranslation);;
		}
		if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			rb.AddForce (-tr.up * speedTranslation);
		} 
		if (Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.LeftArrow)) {
			tr.Rotate (new Vector3 (0, 1, 0) * speedRotation);
		}
		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			tr.Rotate (new Vector3 (0, -1, 0) * speedRotation);
		}
		if (rb.velocity.magnitude > controlBKey.maxSpeed) {
			rb.velocity = rb.velocity.normalized * controlBKey.maxSpeed;
		}
	}

	private void rotatePlayer(GameObject go ){
		Transform tr = go.GetComponent<Transform> ();
		ControllableByKeyboard controlBKey = go.GetComponent<ControllableByKeyboard> ();
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");
		if (controlBKey.inverseMouse) {
			mouseX = -mouseX;
			mouseY = -mouseY;
		}
		Vector3 mouseRotation = new Vector3 (mouseY, 0, mouseX);
		tr.Rotate(mouseRotation * controlBKey.mouseSensibility);
	}

}