using UnityEngine;
using FYFY;

public class ManageTimeSleep : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.


	// Permet de savoir si le jeu est en pause ou non.
	private bool isPaused = false; 


	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		// Si le joueur appuis sur Echap alors la valeur de isPaused devient le contraire.
		if(Input.GetKeyDown(KeyCode.Escape))
			isPaused = !isPaused;

		if (isPaused) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;

			Time.timeScale = 0f; // Le temps s'arrete

		}else{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			Time.timeScale = 1.0f; // Le temps reprend
		}
	}

	void OnGUI ()
	{
		if(isPaused)
		{

			// Si le bouton est présser alors isPaused devient faux donc le jeu reprend.
			if(GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 20, 80, 40), "Continuer"))
			{
				isPaused = false;
			}

			// Si le bouton est présser alors on ferme completement le jeu ou charge la scene "Menu Principal
			// Dans le cas du bouton quitter il faut augmenter sa postion Y pour qu'il soit plus bas
			if(GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 + 40, 80, 40), "Quitter"))
			{
				// Application.Quit(); 
				//Application.LoadLevel("Menu Principal"); 

			}

		}
	}
}