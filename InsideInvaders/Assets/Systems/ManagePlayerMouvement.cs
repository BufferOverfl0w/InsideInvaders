using UnityEngine;
using FYFY;
using UnityStandardAssets.CrossPlatformInput;

public class ManagePlayerMouvement : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	private Family _controlableGO = FamilyManager.getFamily(new AllOfComponents(typeof(ControllableByKeyboard)));
	private float rotY = 0.0f; // rotation around the up/y axis
	private float rotX = 0.0f; // rotation around the right/x axis

	// Varibles pour le system "PlayerParticules" 
	static public bool playerPressForward =false;
	static public bool playerPressBackward =false;
	static public bool playerPressRight =false;
	static public bool playerPressLeft =false;
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

//		checkPause ();
//		if (pause)
//			return;
		//Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,(Screen.height/2)+20,0));
		//Debug.DrawLine (ray.origin, ray.GetPoint (1000), Color.red, 1);
		foreach (GameObject go in _controlableGO) {
			movePlayer (go);
			rotatePlayer (go);
		}
	}
//	private void checkPause (){
//		if(Input.GetKeyDown(KeyCode.Escape)){
//			pause = !pause;
//		}
//	}

	private void movePlayer(GameObject go){
		Rigidbody rb = go.GetComponent<Rigidbody> ();
		Transform tr = go.GetComponent<Transform> ();
		ControllableByKeyboard controlBKey = go.GetComponent<ControllableByKeyboard> ();

		float speedTranslation = controlBKey.propulsionPower *100* rb.mass *Time.deltaTime ;
		//float speedRotation = (controlBKey.propulsionPower *Time.deltaTime) ;
		playerPressForward =false;
		playerPressBackward = false;
		playerPressLeft = false;
		playerPressRight = false;
		if (Input.GetKey (KeyCode.W) ||Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.UpArrow)) {
			rb.AddForce (tr.up * speedTranslation);
			playerPressForward =true;
		}
		if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			rb.AddForce (-tr.up * (speedTranslation/2));
			playerPressBackward =true;
		} 
		if (Input.GetKey (KeyCode.A) ||Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.LeftArrow)) {
			rb.AddForce (-tr.right * speedTranslation);
			playerPressLeft =true;
		}
		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			rb.AddForce (tr.right * speedTranslation);
			playerPressRight =true;
		}
		if (Input.GetKey (KeyCode.Space)) {
			rb.AddForce (-tr.forward * speedTranslation);
			playerPressForward =true;
		}
		if (rb.velocity.magnitude > controlBKey.maxSpeed) {
			rb.velocity = rb.velocity.normalized * controlBKey.maxSpeed;
			playerPressForward =true;
		}
	}

	private void rotatePlayer(GameObject go ){
		Transform tr = go.GetComponent<Transform> ();
		ControllableByKeyboard controlBKey = go.GetComponent<ControllableByKeyboard> ();

		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = -Input.GetAxis("Mouse Y");
		if (controlBKey.inverseMouse) {
			mouseX = -mouseX;
			mouseY = -mouseY;
		}

		rotY += mouseX * controlBKey.mouseSensibility * Time.deltaTime;
		rotX += mouseY * controlBKey.mouseSensibility * Time.deltaTime;

		Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
		tr.rotation = localRotation;

	}
		

}