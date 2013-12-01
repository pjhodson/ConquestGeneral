using UnityEngine;
using System.Collections;

public class placeRedTroops : MonoBehaviour {
	
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
		
		if(boardInfo.inCombat && turn == 1 && !troopsPlaced)
		{
			combatBoard.highlightHexes("red");
			
		}
			
	}
	
	void OnGUI()
	{
		if(boardInfo.inCombat && turn == 1 && !troopsPlaced)
		{
			GUIContent[] generalContent = new GUIContent[10];
					
			for(int i = 0; i < GameObject.Find ("Red General").GetComponent<GeneralUnits>().units.Length; i++)
			{
				generalContent[i] = GUIContent.none;
				if(GameObject.Find ("Red General").GetComponent<GeneralUnits>().unitNames[i] != null)
				{
					switch(GameObject.Find ("Red General").GetComponent<GeneralUnits>().unitNames[i])
					{
						case "Red Grunt(Clone)":
							generalContent[i] = new GUIContent("Grunt"); //Add image here.
							break;
					case "Red Tank(Clone)":
							generalContent[i] = new GUIContent("Tank");
							break;
						case "Red Plane(Clone)":
							generalContent[i] = new GUIContent("Plane");
							break;
					}
				}
			}
			if(combatBoard.aHexIsSelected && generalContent[0] != GUIContent.none)
			{
				int selectedUnit = GUI.SelectionGrid (new Rect(Screen.width/2 - 200, Screen.height - 70,400,60),-1, generalContent, 5);
				if(selectedUnit != -1 && !combatBoard.spawnHexOccupied("red"))
				{
					combatBoard.spawn(GameObject.Find ("Red General").GetComponent<GeneralUnits>().units[selectedUnit], "red");
					combatBoard.deselectAll();
					GameObject.Find ("Red General").GetComponent<GeneralUnits>().removeUnit(selectedUnit);
					if(GameObject.Find ("Red General").GetComponent<GeneralUnits>().counter == 0)
					{
						troopsPlaced = true;
					}
				}
				
			}
			
		}
		
	}
	
}
