using UnityEngine;
using FYFY;

public class RayDraw : FSystem {

	private Family _playerGO = FamilyManager.getFamily(new AllOfComponents(typeof(ControllableByKeyboard)));
	private Family _cameraGO = FamilyManager.getFamily(new AllOfComponents(typeof(CameraPlayer)));
	private Family _recuperableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Recuperable)));
	private Family _intrusGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamIntrus)));

	RaycastHit hit;
	LineRenderer line;

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
		foreach (GameObject go in _playerGO) {
			
			line = go.GetComponent<LineRenderer>();
			if (line == null)
				line = go.AddComponent<LineRenderer> ();
			line.SetVertexCount(2);
			line.SetWidth(0.20f, 0.20f);
			line.material = new Material(Shader.Find("Particles/Additive"));
			line.SetColors (Color.yellow, Color.yellow);
			line.enabled = true;
		}

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

		Ray ray = camera.ScreenPointToRay (new Vector3 (Screen.width / 2, (Screen.height / 2) + 20, 0));
		Vector3 position;

		line.SetColors (Color.yellow, Color.yellow);
		position = ray.GetPoint (1000);

		if (Physics.Raycast (ray, out hit)) {

			position = hit.point + hit.normal;
			GameObject go_hit = hit.transform.gameObject;
			if (_recuperableGO.contains (go_hit.GetInstanceID ())) { // test if is a Recuperable Object
				line.SetColors (new Color32(20,175,20,255), new Color32(20,175,20,255));
			} else if (_intrusGO.contains (go_hit.GetInstanceID ())) {
				line.SetColors (Color.red, Color.red);

			}
		}
		line.SetPosition(0, tr.position);
		line.SetPosition(1, position);
	}
}