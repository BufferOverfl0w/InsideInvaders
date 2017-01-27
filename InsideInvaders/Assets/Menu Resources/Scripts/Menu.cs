using UnityEngine;
using System.Collections;
using WindowsInput;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public static string mission_Name;
	public void SelectMission(){
		SceneManager.LoadScene ("menu missions");
	}

	public void Quitter(){
		Application.Quit();
	}

	public void m1(){
		mission_Name = "mission_01";
		//ChatEnter chat = new ChatEnter ();
		SceneManager.LoadScene ("injection");
	}

	public void m2(){
		mission_Name = "mission_02";
		SceneManager.LoadScene ("injection");
	}

	public void m3(){
		mission_Name = "mission_03";
		SceneManager.LoadScene ("injection");
	}

	public void m4(){
		mission_Name = "mission_04";
		SceneManager.LoadScene ("injection");
	}

	public void m5(){
		mission_Name = "mission_05";
		SceneManager.LoadScene ("injection");
	}

	public void m6(){
		mission_Name = "mission_06";
		SceneManager.LoadScene ("injection");
	}

	public void m7(){
		mission_Name = "mission_07";
		SceneManager.LoadScene ("injection");
	}

	public void m8(){
		mission_Name = "mission_08";
		SceneManager.LoadScene ("injection");
	}

	public void Retour(){
		SceneManager.LoadScene ("menu");
	}

	public void TogglePause(){
		InputSimulator.SimulateKeyPress (VirtualKeyCode.ESCAPE);
	}
}
