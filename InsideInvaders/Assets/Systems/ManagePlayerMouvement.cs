using UnityEngine;
using FYFY;
using UnityStandardAssets.CrossPlatformInput;

public class ManagePlayerMouvement : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _controlableGO = FamilyManager.getFamily(new AllOfComponents(typeof(ControllableByKeyboard)));

	public float mouseSensitivity = 100.0f;
	public float clampAngle = 80.0f;

	private float rotY = 0.0f; // rotation around the up/y axis
	private float rotX = 0.0f; // rotation around the right/x axis
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		foreach (GameObject go in _controlableGO) {
			Transform tr = go.GetComponent<Transform> ();
			Vector3 rot = tr.localRotation.eulerAngles;
			rotY = rot.y;
			rotX = rot.x;
		}


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

		float speedTranslation = controlBKey.propulsionPower *100* rb.mass *Time.deltaTime ;
		float speedRotation = (controlBKey.propulsionPower *Time.deltaTime) ;

		if (Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.UpArrow)) {
			rb.AddForce (tr.up * speedTranslation);
		}
		if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			rb.AddForce (-tr.up * (speedTranslation/2));
		} 
		if (Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.LeftArrow)) {
			rb.AddForce (-tr.right * speedTranslation);
		}
		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			rb.AddForce (tr.right * speedTranslation);
		}
//		if (Input.GetKey (KeyCode.A)) {
//			tr.Rotate (new Vector3 (0, 0, 1) * speedRotation);
//		}
//		if (Input.GetKey (KeyCode.E)) {
//			tr.Rotate (new Vector3 (0, 0, -1) * speedRotation);
//		}
		if (Input.GetKey (KeyCode.Space)) {
			rb.AddForce (-tr.forward * speedTranslation);
		}
		if (rb.velocity.magnitude > controlBKey.maxSpeed) {
			rb.velocity = rb.velocity.normalized * controlBKey.maxSpeed;
		}
	}

	private void rotatePlayer(GameObject go ){
		Transform tr = go.GetComponent<Transform> ();
		ControllableByKeyboard controlBKey = go.GetComponent<ControllableByKeyboard> ();
//		float mouseX = Input.GetAxis("Mouse X");
//		float mouseY = Input.GetAxis("Mouse Y");
//		if (controlBKey.inverseMouse) {
//			mouseX = -mouseX;
//			mouseY = -mouseY;
//		}
//		mouseX *= Time.deltaTime * controlBKey.mouseSensibility;
//		mouseY *= Time.deltaTime * controlBKey.mouseSensibility;
//
//		//Vector3 mouseRotation = new Vector3 (mouseY, 0, mouseX);
//		//tr.Rotate(mouseRotation * controlBKey.mouseSensibility);
//
//		tr.Rotate (mouseY, 0, 0);
//		tr.Rotate (0, 0, mouseX);

		//tr.rotation = Quaternion.Euler(tr.eulerAngles.x, 0, tr.eulerAngles.z);


		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = -Input.GetAxis("Mouse Y");

		rotY += mouseX * mouseSensitivity * Time.deltaTime;
		rotX += mouseY * mouseSensitivity * Time.deltaTime;

		//rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

		Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
		tr.rotation = localRotation;

	}
		

}