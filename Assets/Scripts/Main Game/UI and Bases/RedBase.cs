using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RedBase : MonoBehaviour {
	
	private BoardMaker boardInfo;
	
	public Texture redPixel;
	
	public bool baseClickedOn;
	
	public int barracksBuildTime, factoryBuildTime, hangarBuildTime; //Set in inspector for balance
	
	
	private Queue<GameObject> barracksBuildQueue;
	private Queue<GameObject> factoryBuildQueue;
	private Queue<GameObject> hangarBuildQueue;
	
	// Use this for initialization
	void Start () {
		boardInfo = GameObject.Find ("BoardManager").GetComponent<BoardMaker>();
		baseClickedOn = false;
	}
	
	// Update is called once per frame
	void OnGUI () {
		if(((boardInfo.redEnterBase && boardInfo.getPlayerTurn() == 1) || (baseClickedOn && boardInfo.getPlayerTurn() == 1)))
		{
			GUI.BeginGroup(new Rect(20,20,Screen.width-40,Screen.height-40));
				GUI.DrawTexture (new Rect(0,0,Screen.width,Screen.height),redPixel);
				GUI.BeginGroup (new Rect(20,20,Screen.width-60,Screen.height-60));
				
				if(GUI.Button (new Rect(Screen.width - 180, Screen.height - 110,100,30),"EXIT"))
				{
					boardInfo.redEnterBase = false;
					baseClickedOn = false;
				}
			
					
				GUI.EndGroup();
			GUI.EndGroup();
		}
	}
}