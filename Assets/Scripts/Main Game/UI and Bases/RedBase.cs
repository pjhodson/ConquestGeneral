﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RedBase : MonoBehaviour {
	
	private BoardMaker boardInfo;
	
	public Texture redPixel;
	
	public bool baseClickedOn;
	
	public GameObject redGrunt, redTank, redPlane;
	
	public GUIContent[] buttonDescriptions;
	public GUIContent[] buildingOptions;
	
	private Building_BASE_CLASS[] buildings;
	
	private bool displayChoices;
	private int buttonNum;
	
	private int currentTurn;
	
	public int totalUnitsAllowedOnBase;
	
	public List<GameObject> unitsOnBase;
	
	// Use this for initialization
	void Start () {
		boardInfo = GameObject.Find ("BoardManager").GetComponent<BoardMaker>();
		baseClickedOn = false;
		
		buildings = new Building_BASE_CLASS[6];
		
		displayChoices = false;
		buttonNum = -1;
	}
	
	void Update() {
		currentTurn = GameObject.Find ("BoardManager").GetComponent<BoardMaker>().getTurnNumber();
		
		for(int i = 0; i < buildings.Length; i++)
		{
			if(buildings[i] != null)
			{
				buildings[i].doneTraining(currentTurn);
			}
		}
	}
	
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
				
				
				for(int i = 0; i < buttonDescriptions.Length; i++)
				{
					
				
					if(buttonDescriptions[i].text  == "BUILD HERE")
					{
						if(isAnythingBuilding ())
						{
							GUI.enabled = false;
						}
						else GUI.enabled = true;
						
						if(GUI.Button (new Rect(10,10+ 35* i, 150,30), buttonDescriptions[i]))
						{
							displayChoices = true;
							buttonNum = i;
						}
					}
					
					if(displayChoices)
					{
						for(int j = 0; j < buildingOptions.Length; j++)
							{
								if(GUI.Button (new Rect(170, 10 + 35*j, 100,30),buildingOptions[j]))
								{
									buildHandler(buttonNum,j);
									displayChoices = false;
								}
							}
					}
				
				
					if(buttonDescriptions[i].text != "BUILD HERE") 
					{
						if(buildings[i].getCurrentlyTraining() || !buildings[i].doneBuilding(currentTurn))
						{
							GUI.enabled = false;
						}
						else GUI.enabled = true;
						
						
						if(GUI.Button (new Rect(10,10+ 35* i, 150,30), buttonDescriptions[i]))
						{
							buildings[i].startTraining(currentTurn);
						}
					
						/*if(buildings[i].doneTraining(currentTurn) && !buildings[i].getCurrentlyTraining() && buildings[i].getNumUnitsTrained() > 0 && unitsOnBase.Count < totalUnitsAllowedOnBase) //FIX THIS. YOU IDIOT.
						{
							string unitToAdd = buildings[i].getUnitName();
							switch(unitToAdd)
							{
								case "Grunt":
									unitsOnBase.Add (redGrunt);
									break;
								case "Tank":
									unitsOnBase.Add (redTank);
									break;
								case "Plane":
									unitsOnBase.Add (redPlane);
									break;
							}
						}*/
					}
				}
			
			
			
				GUI.EndGroup();
			GUI.EndGroup();
		}
	}
	
	bool isAnythingBuilding()
	{
		int sum = 0;
		for(int i = 0; i < buildings.Length; i++)
		{
			if(buildings[i] != null) {
				if(!buildings[i].doneBuilding(currentTurn)) { sum++; Debug.Log ("Yep"); }
			}
		}
		Debug.Log (sum);
		if(sum > 0) return true;
		else return false;
	}
	
	void buildHandler(int button, int building)
	{
		switch(building)
		{
			case 0:
				buildings[button] = new Barracks(currentTurn, this);
				buttonDescriptions[button] = buildings[button].getBuildingInfo();
				break;
			case 1:
				buildings[button] = new Factory(currentTurn, this);
				buttonDescriptions[button] = buildings[button].getBuildingInfo();
				break;
			case 2:
				buildings[button] = new Hangar(currentTurn, this);
				buttonDescriptions[button] = buildings[button].getBuildingInfo();
				break;
		}
	}
	
	public void addTroops(string name)
	{
		Debug.Log (name);
		
		switch(name)
		{
			case "Grunt":
				unitsOnBase.Add (redGrunt);
				break;
			case "Tank":
				unitsOnBase.Add (redTank);
				break;
			case "Plane":
				unitsOnBase.Add (redPlane);
				break;
		}
	}
}