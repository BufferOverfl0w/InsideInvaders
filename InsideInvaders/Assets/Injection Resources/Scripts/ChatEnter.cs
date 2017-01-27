using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using inside_invaders_TAL;
using UnityEngine.SceneManagement;

public class ChatEnter : MonoBehaviour {
	public InputField InputText;
	public GameObject TextPrefab;
	public GameObject BotTextPrefab;
	public Transform ChatContent;
	public Scrollbar Scroll;
	public Transform Aim;
	public Transform DangerZone;
	Vector3 newAimPosition;
	//List<Vector3> zonePositions;

    void startText()
    {
        GameObject newText = (GameObject)Instantiate(BotTextPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newText.transform.SetParent(ChatContent.transform);
        newText.GetComponent<Text>().text = "Bonjour! Je suis votre operateur pour cette mission.\n";
        Scroll.value = 0;
    }

	void botReply() {
		string user_text = ChatContent.GetChild (ChatContent.childCount - 1).GetComponent<Text>().text;
		string t = "";
		//t = user_text; //test

		List<concept_ID> sentenceConcepts = Concept.getConcepts2(user_text);
		float[] d = Response.getMovement (sentenceConcepts, Aim.position.x, Aim.position.y);
        //Debug.Log ("test");
        newAimPosition = new Vector3 (d[0], d[1], 0f);
        bool hasMoved = d[2] == 1f;
		t = Response.getBestResponse (sentenceConcepts, hasMoved);

		t += '\n';
		GameObject newText = (GameObject)Instantiate(BotTextPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		newText.transform.SetParent(ChatContent.transform);
		newText.GetComponent<Text>().text = t;
		Scroll.value = 0;
		if(sentenceConcepts.Contains(concept_ID.INJECT)){
			Invoke("inject", 0.1f);
		}
	}

	public void chatEnter() {
		if (InputText.text == "")
			return;
		GameObject newText = (GameObject)Instantiate(TextPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		newText.transform.SetParent(ChatContent.transform);
		newText.GetComponent<Text>().text = InputText.text;
		InputText.text = "";
		Invoke("botReply", 0.5f);
		Scroll.value = 0;
	}

	void moveAimToPosition(float x, float y) {
		newAimPosition = new Vector3 (x, y, Aim.position.z);
	}

	void moveAim(float horizontal, float vertical) {
		newAimPosition = new Vector3 (Aim.position.x + horizontal, Aim.position.y + vertical, Aim.position.z);
	}

	public void inject(){
		if (Vector3.Distance (Aim.position, DangerZone.position) < 50) {
			Debug.Log ("Injection reussie");
			// start mission
			SceneManager.LoadScene (Menu.mission_Name);
		} else {
			Debug.Log ("Injection ratee");
			GameObject newText = (GameObject)Instantiate(BotTextPrefab, new Vector3(0, 0, 0), Quaternion.identity);
			newText.transform.SetParent(ChatContent.transform);
			newText.GetComponent<Text>().text = "Injection ratee \n";
			Scroll.value = 0;
		}
    }

	// Use this for initialization
	void Start ()
    {
        Invoke("startText", 0.5f);

        newAimPosition = new Vector3 (Aim.position.x, Aim.position.y, Aim.position.z);

		Concept.concepts_init();
		Response.responses_init ();

		int rdz = Random.Range(0, 8);
		DangerZone.position = new Vector3(Concept.concepts[rdz].posX, Concept.concepts[rdz].posY, DangerZone.position.z);
	}

	// Update is called once per frame
	void Update () {
		if (InputText.isFocused && InputText.text != "" && (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.KeypadEnter)))
			chatEnter ();
		if (newAimPosition != Aim.position)
			Aim.position = Vector3.MoveTowards (Aim.position, newAimPosition, 1);
	}
}
