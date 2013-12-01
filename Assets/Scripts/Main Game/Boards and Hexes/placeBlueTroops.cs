using UnityEngine;
using System.Collections;

public class placeBlueTroops : MonoBehaviour {
	
	private CombatBoard combatBoard;
	private BoardMaker boardInfo;
	
	private int turn;
	
	public bool troopsPlaced;
	
	// Use this for initialization
	void Start () {
		combatBoard = GameObject.Find ("CombatBoard").GetComponent<CombatBoard>();
		boardInfo = GameObject.Find("BoardManager").GetComponent<BoardMaker>();
			
		troopsPlaced = false;
	}
	
	// Update is called once per frame
	void Update () {
		turn = boardInfo.getPlayerTurn();
		
		Debug.Log (boardInfo.inCombat + " " + turn + " " + troopsPlaced);
		
		if(boardInfo.inCombat && turn == 2 && !troopsPlaced)
		{
			combatBoard.highlightHexes("blue");
		}				
	}
	
	void OnGUI()
	{
		if(boardInfo.inCombat && turn == 2 && !troopsPlaced)
		{
			GUIContent[] generalContent = new GUIContent[10];
					
			for(int i = 0; i < GameObject.Find ("Blue General").GetComponent<GeneralUnits>().units.Length; i++)
			{
				generalContent[i] = GUIContent.none;
				if(GameObject.Find ("Blue General").GetComponent<GeneralUnits>().unitNames[i] != null)
				{
					switch(GameObject.Find ("Blue General").GetComponent<GeneralUnits>().unitNames[i])
					{
						case "Blue Grunt(Clone)":
							generalContent[i] = new GUIContent("Grunt"); //Add image here.
							break;
					case "Blue Tank(Clone)":
							generalContent[i] = new GUIContent("Tank");
							break;
						case "Blue Plane(Clone)":
							generalContent[i] = new GUIContent("Plane");
							break;
					}
				}
			}
			if(combatBoard.aHexIsSelected && generalContent[0] != GUIContent.none)
			{
				int selectedUnit = GUI.SelectionGrid (new Rect(Screen.width/2 - 200, Screen.height - 70,400,60),-1, generalContent, 5);
				if(selectedUnit != -1 && !combatBoard.spawnHexOccupied("blue"))
				{
					combatBoard.spawn(GameObject.Find ("Blue General").GetComponent<GeneralUnits>().units[selectedUnit], "blue");
					combatBoard.deselectAll();
					GameObject.Find ("Blue General").GetComponent<GeneralUnits>().removeUnit(selectedUnit);
					if(GameObject.Find ("Blue General").GetComponent<GeneralUnits>().counter == 0)
					{
						troopsPlaced = true;
					}
				}
				
			}
			
		}
		
	}
	
}
