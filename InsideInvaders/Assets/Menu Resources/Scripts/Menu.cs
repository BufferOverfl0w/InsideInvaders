
using UnityEngine;
using System.Collections;
using WindowsInput;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public void SelectMission(){
		SceneManager.LoadScene ("menu missions");
	}

	public void Quitter(){
		Application.Quit();
	}

	public void m1(){
		SceneManager.LoadScene ("mission_01");
	}

	public void m2(){
		SceneManager.LoadScene ("mission_02");
	}

	public void m3(){
		SceneManager.LoadScene ("mission_03");
	}

	public void m4(){
		SceneManager.LoadScene ("mission_04");
	}

	public void m5(){
		SceneManager.LoadScene ("mission_05");
	}

	public void m6(){
		SceneManager.LoadScene ("mission_06");
	}

	public void m7(){
		SceneManager.LoadScene ("mission_07");
	}

	public void m8(){
		SceneManager.LoadScene ("mission_08");
	}

	public void Retour(){
		SceneManager.LoadScene ("menu");
	}

	public void TogglePause(){
		InputSimulator.SimulateKeyPress (VirtualKeyCode.ESCAPE);
	}
}