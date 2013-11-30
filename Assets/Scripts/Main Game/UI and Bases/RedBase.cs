﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RedBase : MonoBehaviour {
	
	private BoardMaker boardInfo;
	private GameObject redGeneral;
	
	public Texture redPixel;
	public Texture generalImage;
	
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
	
	private GUIContent[] unitContent;
	
	// Use this for initialization
	void Start () {
		boardInfo = GameObject.Find ("BoardManager").GetComponent<BoardMaker>();
		redGeneral = GameObject.Find ("Red General");
		baseClickedOn = false;
		
		buildings = new Building_BASE_CLASS[6];
		
		unitContent = new GUIContent[20];
		
		displayChoices = false;
		buttonNum = -1;
		
		for(int i = 0; i < 20; i++)
		{
			unitContent[i] = GUIContent.none;
		}
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
				
				if(boardInfo.redGeneralHome)
				{
					GUI.DrawTexture(new Rect(Screen.width/2 - generalImage.width, Screen.height/2 - generalImage.height/2, generalImage.width, generalImage.height),generalImage);
					
					GUIContent[] generalContent = new GUIContent[10];
				
					for(int i = 0; i < redGeneral.GetComponent<GeneralUnits>().units.Length; i++)
					{
						generalContent[i] = GUIContent.none;
						if(redGeneral.GetComponent<GeneralUnits>().units[i] != null)
						{
							switch(redGeneral.GetComponent<GeneralUnits>().units[i].name)
							{
								case "Red Grunt":
									generalContent[i] = new GUIContent("Grunt"); //Add image here.
									break;
								case "Red Tank":
									generalContent[i] = new GUIContent("Tank");
									break;
								case "Red Plane":
									generalContent[i] = new GUIContent("Plane");
									break;
							}
						}
					}
					GUI.SelectionGrid (new Rect(Screen.width/2 - 200, Screen.height - 200,400,60),-1, generalContent, 5);			
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
					}
				}
				
				if(unitsOnBase.Count > 0)
				{
					GUI.enabled = true;
					int selectedUnit = 0;
					selectedUnit = GUI.SelectionGrid(new Rect(3*Screen.width/4, 10,100,300),-1,unitContent,2);
					GUI.enabled = false;
					
					
					if(selectedUnit > -1 && boardInfo.redGeneralHome && !redGeneral.GetComponent<GeneralUnits>().isFull()) //MAKE SURE THE GENERAL IS NOT FULL UP.
					{
						unitContent[selectedUnit] = GUIContent.none;
						redGeneral.GetComponent<GeneralUnits>().addUnit(unitsOnBase[selectedUnit]);
						unitsOnBase.RemoveAt(selectedUnit);
						
						Array.Sort(unitContent,delegate(GUIContent gc1, GUIContent gc2) { return gc2.text.CompareTo(gc1.text);});
						
					}
					
				}
				else
				{
					GUI.Label (new Rect(3*Screen.width/4, 10,100,300), "No Units on Base.");
				}
						
				GUI.Label (new Rect(10,450,400,30), GUI.tooltip);
			
			
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
		if(unitsOnBase.Count < 20)
		{
			switch(name)
			{
				case "Grunt":
					unitsOnBase.Add (redGrunt);
					unitContent[unitsOnBase.LastIndexOf(redGrunt)] = new GUIContent("Grunt", "Click to transfer to General.");
					break;
				case "Tank":
					unitsOnBase.Add (redTank);
					unitContent[unitsOnBase.LastIndexOf(redTank)] = new GUIContent("Tank");
					break;
				case "Plane":
					unitsOnBase.Add (redPlane);
					unitContent[unitsOnBase.LastIndexOf(redPlane)] = new GUIContent("Plane");
					break;
			}
		}
	}
}