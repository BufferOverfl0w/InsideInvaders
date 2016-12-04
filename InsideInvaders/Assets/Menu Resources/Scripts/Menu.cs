using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public void NewGame(){
		Application.LoadLevel ("mission_01");
	}

	public void SelectMission(){
		Application.LoadLevel ("menu missions");
	}

	public void ExitGame(){
		Application.Quit();
	}
}
