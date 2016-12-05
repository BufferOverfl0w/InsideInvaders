using UnityEngine;
using System.Collections;
using WindowsInput;

public class Menu : MonoBehaviour {

	public void SelectMission(){
		Application.LoadLevel ("menu missions");
	}

	public void Quitter(){
		Application.Quit();
	}

	public void m1(){
		Application.LoadLevel ("mission_01");
	}

	public void m2(){
		Application.LoadLevel ("mission_02");
	}

	public void m3(){
		Application.LoadLevel ("mission_03");
	}

	public void m4(){
		Application.LoadLevel ("mission_04");
	}

	public void m5(){
		Application.LoadLevel ("mission_05");
	}

	public void m6(){
		Application.LoadLevel ("mission_06");
	}

	public void m7(){
		Application.LoadLevel ("mission_07");
	}

	public void m8(){
		Application.LoadLevel ("mission_08");
	}

	public void Retour(){
		Application.LoadLevel ("menu");
	}

	public void TogglePause(){
		InputSimulator.SimulateKeyPress (VirtualKeyCode.ESCAPE);
	}
}
