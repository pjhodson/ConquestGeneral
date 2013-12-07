using UnityEngine;
using System.Collections;

public class mainMenu : MonoBehaviour {

	public GUITexture gameLogo;
	public GUITexture fadeBar;
	
	public GUIContent[] menuContent;
	
	private GUIStyle centeredText;
	private bool howTo;
	
	// Use this for initialization
	void Start () {
		howTo = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		centeredText = GUI.skin.GetStyle("Label");
		centeredText.alignment = TextAnchor.UpperCenter;
		centeredText.fontSize = 18;
				
		Rect inset = new Rect(0, Screen.height - gameLogo.pixelInset.height, gameLogo.pixelInset.width,gameLogo.pixelInset.height);
		gameLogo.pixelInset = inset;
		
		inset = new Rect(Screen.width/2 - 300, 0, fadeBar.pixelInset.width,fadeBar.pixelInset.height);
		fadeBar.pixelInset = inset;
		
		for(int i = 0; i < menuContent.Length; i++)
		{
			if(GUI.Button (new Rect(Screen.width/2 - 75, Screen.height/2 - 15 + 35*i, 150,30),menuContent[i]))
			{
				handleMenu(i);
			}
			GUI.Label (new Rect(Screen.width/2 - 200, Screen.height - 30, 400, 36), GUI.tooltip, centeredText);
		}
		
		if (howTo == true)
		{
			GUI.Box (new Rect(20, 20, Screen.width - 40, Screen.height - 40), "How To Play");
			GUI.Label (new Rect (30, 50, Screen.width - 60, Screen.height - 100), "Add how to play");
			
			if (GUI.Button (new Rect (Screen.width/4 * 3, Screen.height - 60, 150, 30), "Exit"))
			{
				howTo = false;
			}
		}
	}
	void handleMenu(int i)
	{
		switch(i)
		{
		case 0: //play
			Application.LoadLevel("ConquestGeneral");
			break;
		case 1: //how-to
			Debug.Log ("COMING SOON");
			howTo = true;
			break;
		case 2:
			Application.Quit();
			break;
		default:
			break;
		}
	}
}
