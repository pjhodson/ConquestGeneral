using UnityEngine;
using System.Collections;

public class mainMenu : MonoBehaviour {

	public GUITexture gameLogo;
	public GUITexture fadeBar;
	
	public GameObject musician;
	
	public Texture2D transpGray;
	
	public GUIContent[] menuContent;
	
	private GUIStyle centeredText;
	private GUIStyle darkerBox;
	private bool howTo;
	
	// Use this for initialization
	void Start () {
		howTo = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.Find ("Musician(Clone)") == null)
		{
			Instantiate(musician);
		}
	
	}
	
	void OnGUI()
	{
		centeredText = GUI.skin.GetStyle("Label");
		centeredText.alignment = TextAnchor.UpperCenter;
		centeredText.fontSize = 18;
		
		darkerBox = GUI.skin.GetStyle("Box");
		darkerBox.normal.background = transpGray;
				
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
			GUI.Box (new Rect(30, 50, Screen.width - 60, Screen.height - 100), "How To Play");
			GUI.Label (new Rect (30, 80, Screen.width - 60, Screen.height - 100), "Starting the Game: " +
				"\nSelect the units for your starting army" +
				"\nYou have 30 points to spend and each units subtracts from that life pool\nYou may have a maximum of 10 units" +
				"\n\nBeginning the game:" +
				"\nThe game will pick whos turn it is first" +
				"\nYou may move your general 1 time per turn" +
				"\nMoving your general onto another general will initiate Combat" +
				"\nClicking on your base will move you into the base management screen where you can build and train units" +
				"\n\nPowerups: " +
				"\nThere are powerups dotted around the map. Every 15 turns a supply drop will occur to bring more powerups." +
				"\nPowerups only affect the units on your general, so plan carefully." +
				"\n\nWinning the Game:" +
				"\nMoving the general onto the opposing players base will win you the game.");
			
			
			
			if (GUI.Button (new Rect (Screen.width - 180, Screen.height - 80, 150, 30), "Exit"))
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
