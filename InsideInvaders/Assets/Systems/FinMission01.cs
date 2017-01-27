using UnityEngine;
using UnityEngine.SceneManagement;
using FYFY;

public class FinMission01 : FSystem {
	private Family _intrusGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamIntrus)));
	private Family _defenseGO = FamilyManager.getFamily(new AllOfComponents(typeof(TeamDefense)));
	private Family _playerGO = FamilyManager.getFamily(new AllOfComponents(typeof(ControllableByKeyboard)));


	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		int count = 0;
		foreach (GameObject intrus in _intrusGO) {
			count += 1;
		}
		if (count == 0) {
			Debug.Log ("Mission Accomplie !!!");
			charger_mission_suivante ();
		} 
		count = 0;
		foreach (GameObject player in _playerGO){
			if (player.GetComponent<Vivant> ().current_pv <= 0) {
				Debug.Log ("Mission Echouée !!!");
				recommencer_mission ();
			}
		}
		count = 0;
		foreach (GameObject defenseur in _defenseGO) {
			count += 1;
		}
		if (count == 0) {
			Debug.Log ("Mission Echouée !!!");
			recommencer_mission ();
		} 
	}


	void charger_mission_suivante(){
		string scene_name = SceneManager.GetActiveScene().name;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		switch(scene_name){
		case "mission_01":
			
				Menu.mission_Name = "mission_02";
				break;
			case "mission_02":
				Menu.mission_Name = "mission_03";
				break;
			case "mission_03":
				Menu.mission_Name = "mission_04";
				break;
			case "mission_04":
				Menu.mission_Name = "mission_05";
				break;
			case "mission_05":
				Menu.mission_Name = "mission_06";
				break;
			case "mission_06":
				Menu.mission_Name = "mission_07";
				break;
			case "mission_07":
				Menu.mission_Name = "mission_08";
				break;
			case "mission_08":
				Menu.mission_Name = "fin";
				break;
		}
		SceneManager.LoadScene ("injection",LoadSceneMode.Single);
	}
	void recommencer_mission(){
		string scene_name = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(scene_name,LoadSceneMode.Single);
	}
}