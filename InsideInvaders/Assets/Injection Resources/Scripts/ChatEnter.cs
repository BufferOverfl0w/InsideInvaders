using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChatEnter : MonoBehaviour {
	public InputField InputText;
	public GameObject TextPrefab;
	public GameObject BotTextPrefab;
	public Transform ChatContent;
	public Scrollbar Scroll;
	public Transform Aim;
	public Transform DangerZone;
	Vector3 newAimPosition;
	List<Vector3> zonePositions;

	void botReply() {
		string user_t = ChatContent.GetChild (ChatContent.GetChildCount () - 1).GetComponent<Text>().text;
		string t = user_t; //test
		//t = generateBotReply();
		t += '\n';
		GameObject newText = (GameObject)Instantiate(BotTextPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		newText.transform.SetParent(ChatContent.transform);
		newText.GetComponent<Text>().text = t;
		Scroll.value = 0;
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

	// Use this for initialization
	void Start () {
		newAimPosition = new Vector3 (Aim.position.x, Aim.position.y, Aim.position.z);
		zonePositions = new List<Vector3>();
		//zonePositions.Add(); -> pour ttes les positions possibles
		//DangerZone.position.Set() -> une des positions
	}

	// Update is called once per frame
	void Update () {
		if (InputText.isFocused && InputText.text != "" && (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.KeypadEnter)))
			chatEnter ();
		if (newAimPosition != Aim.position)
			Aim.position = Vector3.MoveTowards (Aim.position, newAimPosition, 1);
	}
}
