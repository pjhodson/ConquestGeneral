using UnityEngine;
using System.Collections;

public class winner : MonoBehaviour {
	
	public string theWinner;
	private bool displayMessage;
	// Use this for initialization
	void Start () {
		theWinner = "";
		displayMessage = false;
	}
	
	// Update is called once per frame
	void Update () {
		DontDestroyOnLoad(this.gameObject);
	}
	
	void OnLevelWasLoaded(int level)
	{
		if(level == 2)
		{
			displayMessage = true;
		}
	}
	
	void OnGUI()
	{
		if(displayMessage)
		{
			GUI.Box (new Rect(Screen.width/2 - 305, Screen.height/2-25,610,50),"");
			GUI.Label (new Rect(Screen.width/2 - 300, Screen.height/2-20,600,40),"Congratulations, " + theWinner + "! Your foe flies the white flag of surrender.");
			
			if(GUI.Button (new Rect(Screen.width/2 - 75, Screen.height/2 + 30, 150, 60), "PLAY AGAIN")){
				Destroy (this.gameObject);
				Application.LoadLevel("ConquestGeneral");
			}
			
			if(GUI.Button (new Rect(Screen.width/2 - 75, Screen.height/2 + 95, 150, 60), "MAIN MENU")){
				Destroy (this.gameObject);
				Application.LoadLevel("MainMenu");
			}
		}
	}
}
