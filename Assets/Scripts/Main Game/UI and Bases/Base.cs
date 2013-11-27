using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour {

	private BoardMaker boardInfo;
	
	//red is first, blue is second
	private bool[,] buildingStruct, buildingUnit; //row is player, col is building type
	private bool[,] structBuilt;
	private int[,] structTurnStarted;
	private List<UnitsInBase>[,] unitQueue;
	private List<UnitsInBase>[] units;
	
	private int structBuildTime, unitBuildTime;
	private int structTimeLeft, unitTimeLeft;
	
	private int queueCap;
	private int unitCap;
	
	private bool guiEnabled;
	private int playerTurn;
	
	private string[] structMsg;
	private string[] unitMsg;
	private int totalUnits;
	
	// Use this for initialization
	void Start () {
		boardInfo = GameObject.Find ("BoardManager").GetComponent<BoardMaker>();
		unitQueue = new List<UnitsInBase>[2,3];
		units = new List<UnitsInBase>[2];
		
		structBuildTime = 4;
		unitBuildTime = 4;
		queueCap = 5;
		unitCap = 20;
		
		buildingStruct = new bool[2,3];
		structTurnStarted = new int[2,3];
		structBuilt = new bool[2,3];
		
		buildingUnit = new bool[2,3];
		
		for(int i=0;i<2;i++)
		{
			units[i] = new List<UnitsInBase>();
			for(int j=0;j<3;j++)
			{
				buildingStruct[i,j] = false;
				structBuilt[i,j] = false;
				buildingUnit[i,j] = false;
				unitQueue[i,j] = new List<UnitsInBase>();
			}
		}
		
		structMsg = new string[]{"Barracks","Factory","Air Base"};
		unitMsg = new string[]{"Grunt","Tank","Plane"};
	}
	
	//BUG: Buttons can be press twice the amount that they should be able to be pressed
	void OnGUI(){
		//checks if player is on their base and sets player turn
		if(boardInfo.redGeneralHome == true && boardInfo.getPlayerTurn() == 1){
			guiEnabled = true;;
			playerTurn=0;
		}
		else if(boardInfo.blueGeneralHome == true && boardInfo.getPlayerTurn() == 2){
			guiEnabled = true;
			playerTurn=1;
		}
		else
		{
			guiEnabled = false;
		}
		
		if(guiEnabled == true){
			
			for(int i=0;i<3;i++)
			{
				GUI.enabled=true;
				//buttons for building structures
				if(structBuilt[playerTurn,i]==false && buildingStruct[playerTurn,i]==false)
				{
					if(GUI.Button(new Rect(Screen.width-100, Screen.height-100 -((2-i)*50),100,50), "Build "+structMsg[i]+"\n"+structBuildTime+" turns"))
					{
						buildingStruct[playerTurn,i]=true;
						structTurnStarted[playerTurn,i]=boardInfo.getTurnNumber();
					}
				}
				
				//display boxes for build time
				else if(buildingStruct[playerTurn,i]==true)
				{
					structTimeLeft = structBuildTime - (boardInfo.getTurnNumber() - structTurnStarted[playerTurn,i]);
					if(structTimeLeft <=0)
					{
						structBuilt[playerTurn,i]=true;
						buildingStruct[playerTurn,i]=false;
					}
					else
						GUI.Box(new Rect(Screen.width-100, Screen.height-100 -((2-i)*50),100,50), "Building\n"+structMsg[i]+"\nTurns left: " + structTimeLeft);
				}
				
				//buttons for building units
				if(structBuilt[playerTurn,i]==true)
				{
					totalUnits=0;
					
					if(unitQueue[playerTurn,i].Count>0)
					{
						if(boardInfo.getTurnNumber() - unitQueue[playerTurn,i][0].getTurnStarted() >= unitBuildTime)
						{
							units[i].Add(unitQueue[playerTurn,i][0]);
							unitQueue[playerTurn,i].RemoveAt(0);
						}
					}
					
					for(int j=0;j<3;j++)
					{
						totalUnits+=unitQueue[playerTurn,j].Count;
					}
					totalUnits+=units[playerTurn].Count;
					
					if(totalUnits>=unitCap || unitQueue[playerTurn,i].Count>=queueCap)
					{
						GUI.enabled=false;
					}
					
					if(GUI.Button(new Rect(Screen.width-100, Screen.height-100 -((2-i)*50),100,50), "Build "+unitMsg[i]+"\n"+unitBuildTime+" turns"))
					{
						unitQueue[playerTurn,i].Add(new UnitsInBase(boardInfo.getTurnNumber(), unitMsg[i]));
					}
				}
			}
			
		}
	}
}

//OLD CODE
/*
 * private BoardMaker boardInfo;
	
	//red is first, blue is second
	private bool[] buildingBarracks, buildingUnit;
	private bool[] barracksBuilt;
	private int[] buildStartTurn;
	private int structBuildTime, soldierBuildTime, structTimeLeft;
	private List<int> soldierQueue;
	private int soldierQueueCap;
	private int soldierCount;
	private int soldiersCap;
	
	private bool guiEnabled;
	
	// Use this for initialization
	void Start () {
		boardInfo = GameObject.Find ("BoardManager").GetComponent<BoardMaker>();
		soldierQueue = new List<int>();
		
		structBuildTime = 4;
		soldierBuildTime = 10;
		soldierQueueCap = 5;
		soldierCount = 0;
		soldiersCap = 20;
		
		buildingBarracks = new bool[2];
		barracksBuilt = new bool[2];
		buildStartTurn = new int[2];
		buildingUnit = new bool[2];
		for(int i=0;i<2;i++)
		{
			buildingBarracks[i] = false;
			barracksBuilt[i] = false;
			buildingUnit[i] = false;
		}
		
		
		guiEnabled = false;
	}
	
	void OnGUI(){
		if(boardInfo.redGeneralHome == true && boardInfo.getPlayerTurn() == 1){
			guiEnabled = true;
		}
		else if(boardInfo.blueGeneralHome == true && boardInfo.getPlayerTurn() == 2){
			guiEnabled = true;
		}
		else
		{
			guiEnabled = false;
		}
		if(guiEnabled == true){
			if(buildingBarracks[boardInfo.getPlayerTurn()-1] == false && barracksBuilt[boardInfo.getPlayerTurn()-1] == false){
				if(GUI.Button(new Rect(Screen.width-100, Screen.height-100,100,50), "Build Barracks\n"+structBuildTime+" turns")){
					buildingBarracks[boardInfo.getPlayerTurn()-1] = true;
					buildStartTurn[boardInfo.getPlayerTurn()-1] = boardInfo.getTurnNumber();
				}
			}
			else if(buildingBarracks[boardInfo.getPlayerTurn()-1] == true){
				structTimeLeft = structBuildTime - (boardInfo.getTurnNumber() - buildStartTurn[boardInfo.getPlayerTurn()-1]);
				GUI.Box (new Rect(Screen.width-100, Screen.height-100,100,50), "Building\nBarracks\nTurns left: " + structTimeLeft);
				if((boardInfo.getTurnNumber() - buildStartTurn[boardInfo.getPlayerTurn()-1]) >= structBuildTime)
				{
					buildingBarracks[boardInfo.getPlayerTurn()-1] = false;
					barracksBuilt[boardInfo.getPlayerTurn()-1] = true;
				}
			}
			else if(barracksBuilt[boardInfo.getPlayerTurn()-1]){
				int sel=0;
				string[] selStr = {"a","b","c"};
				GUI.SelectionGrid(new Rect(Screen.width-100,Screen.height-125,100,30),sel,selStr,3);
				
				if(soldierQueue.Count >= soldierQueueCap || soldierCount >= soldiersCap){
					GUI.enabled = false;
				}
				if(GUI.Button(new Rect(Screen.width-100, Screen.height-100,100,50), "Train Soldier\n"+soldierBuildTime+" turns")){
					soldierQueue.Add(boardInfo.getTurnNumber());
					Debug.Log(soldierQueue[0]);
				}
				if(soldierQueue.Count>0 && ((boardInfo.getTurnNumber() - soldierQueue[0]) >= soldierBuildTime)){
					soldierCount++;
					soldierQueue.RemoveAt(0);
				}
			}
		}
	}
 */