using UnityEngine;
using System.Collections;

public class combatGUI : MonoBehaviour {
	
	private BoardMaker boardInfo;
	private int playerTurn;
	
	private GeneralUnits redGeneral;
	private GeneralUnits blueGeneral;
	
	public Texture redPixel;
	public Texture bluePixel;
	public Texture explosion;
	
	private bool redDeclareAttacker, blueDeclareAttacker;
	private bool redDeclareDefender, blueDeclareDefender;
	
	private GUIContent[] blueGeneralUnits;
	private GUIContent[] redGeneralUnits;
	
	public GameObject attackingUnit, defendingUnit;
	private int defenderInt;
	
	private bool damageDealt;
	private int damage;
	
	private bool redUnitDestroyed, blueUnitDestroyed;
	// Use this for initialization
	void Start () {
		boardInfo = GameObject.Find ("BoardManager").GetComponent<BoardMaker>();
		
		redDeclareAttacker = redDeclareDefender = blueDeclareAttacker = blueDeclareDefender = false;
		
		redGeneral = GameObject.Find ("Red General").GetComponent<GeneralUnits>();
		blueGeneral = GameObject.Find ("Blue General").GetComponent<GeneralUnits>();
		
		blueGeneralUnits = new GUIContent[10];
		redGeneralUnits = new GUIContent[10];
		
		attackingUnit = defendingUnit = null;
		damage = 0;
		damageDealt = false;
		
		redUnitDestroyed = blueUnitDestroyed = false;
	}
	
	// Update is called once per frame
	void Update () {
		playerTurn = boardInfo.getPlayerTurn();
		
		for(int i = 0; i < GameObject.Find ("Blue General").GetComponent<GeneralUnits>().units.Length; i++)
		{
			blueGeneralUnits[i] = GUIContent.none;
			if(blueGeneral.unitNames[i] != null)
			{
				switch(blueGeneral.unitNames[i])
				{
					case "Blue Grunt(Clone)":
						blueGeneralUnits[i] = new GUIContent("Grunt"); //Add image here.
						break;
				case "Blue Tank(Clone)":
						blueGeneralUnits[i] = new GUIContent("Tank");
						break;
					case "Blue Plane(Clone)":
						blueGeneralUnits[i] = new GUIContent("Plane");
						break;
				}
			}
		}
		
		for(int i = 0; i < GameObject.Find ("Red General").GetComponent<GeneralUnits>().units.Length; i++)
		{
			redGeneralUnits[i] = GUIContent.none;
			if(redGeneral.unitNames[i] != null)
			{
				switch(redGeneral.unitNames[i])
				{
					case "Red Grunt(Clone)":
						redGeneralUnits[i] = new GUIContent("Grunt"); //Add image here.
						break;
				case "Red Tank(Clone)":
						redGeneralUnits[i] = new GUIContent("Tank");
						break;
					case "Red Plane(Clone)":
						redGeneralUnits[i] = new GUIContent("Plane");
						break;
				}
			}
		}
		
	}
	
	public void resetAllOnTurnEnd()
	{
		attackingUnit = defendingUnit = null;
		redDeclareAttacker = redDeclareDefender = blueDeclareAttacker = blueDeclareDefender = false;
		damageDealt = false;
		
		defenderInt = -1;
		damage = 0;
		redUnitDestroyed = blueUnitDestroyed = false;
	}
		
	
	void OnGUI()
	{
		GUI.depth  = 10;
		if(boardInfo.inCombat)
		{
				//RED SIDE
			GUI.BeginGroup(new Rect(100,30,Screen.width/2,Screen.height-60));
				GUI.DrawTexture (new Rect(0,0,Screen.width/2,Screen.height),redPixel);
					
					if(playerTurn == 1 && !redDeclareAttacker)
					{
						if(redGeneralUnits[0] != GUIContent.none)
						{
							GUI.Label (new Rect(10, 20, 200,40),"DECLARE ATTACKER");
					
							int selectedUnit = GUI.SelectionGrid (new Rect(10, 50,50,300),-1, redGeneralUnits, 1);
							
							if(selectedUnit > -1)
							{
								if(redGeneral.units[selectedUnit] != null)
								{
									attackingUnit = redGeneral.units[selectedUnit];
									redDeclareAttacker = true;
								}
							}
						}
						else
						{
							GUI.Label (new Rect(10,50,150,300),"GENERAL, WE HAVE NO UNITS! RETREAT!");
						}
						
					}
					if(attackingUnit != null && playerTurn == 1)
					{
						GUI.Box (new Rect(100,100,200,300),"ATTACKER");
						GUI.Label (new Rect(100,130,200,40), attackingUnit.GetComponent<attributes>().unitName);
						GUI.DrawTexture(new Rect(150,180,100,100),attackingUnit.GetComponent<attributes>().unitImage);
						GUI.Label (new Rect(100, 300, 200, 40), "HP: " + attackingUnit.GetComponent<attributes>().hp);
					}
				
				
					//DEFENDING UNIT SELECTION
					if(playerTurn == 2 && blueDeclareAttacker && !redDeclareDefender)
					{
						if(redGeneralUnits[0] != GUIContent.none)
						{
							GUI.Label (new Rect(10, 20, 200,40),"DECLARE DEFENDER");
					
							int selectedUnit = GUI.SelectionGrid (new Rect(10, 50,50,300),-1, redGeneralUnits, 1);
							defenderInt = selectedUnit;
							if(selectedUnit > -1)
							{
								if(redGeneral.units[selectedUnit] != null)
								{
									defendingUnit = redGeneral.units[selectedUnit];
									redDeclareDefender = true;
								}
							}
						}
						else
						{
							GUI.Label (new Rect(10,50,150,300),"ALL UNITS DESTROYED! RETREAT!");
						}	
						
					}
					if(defendingUnit != null && playerTurn == 2)
					{
						GUI.Box (new Rect(100,100,200,300),"DEFENDER");
						GUI.Label (new Rect(100,130,200,40), defendingUnit.GetComponent<attributes>().unitName);
						GUI.DrawTexture(new Rect(150,180,100,100),defendingUnit.GetComponent<attributes>().unitImage);
						GUI.Label (new Rect(100, 300, 200, 40), "HP: " + defendingUnit.GetComponent<attributes>().hp);
					}
				
					if(redUnitDestroyed)
					{
						GUI.DrawTexture(new Rect(150,180,100,100),explosion);
					}
			
		
			GUI.EndGroup();
			
			//BLUE SIDE
			GUI.BeginGroup(new Rect(Screen.width/2, 30, Screen.width/2 - 100, Screen.height - 60));
				GUI.DrawTexture(new Rect(0,0,Screen.width/2, Screen.height), bluePixel);
					
					//ATTACKING UNIT SELECTION
					if(playerTurn == 2 && !blueDeclareAttacker)
					{
						if(blueGeneralUnits[0] != GUIContent.none)
						{
							GUI.Label (new Rect(10, 20, 200,40),"DECLARE ATTACKER");
					
							int selectedUnit = GUI.SelectionGrid (new Rect(10, 50,50,300),-1, blueGeneralUnits, 1);
							
							if(selectedUnit > -1)
							{
								if(blueGeneral.units[selectedUnit] != null)
								{
									attackingUnit = blueGeneral.units[selectedUnit];
									blueDeclareAttacker = true;
								}
							}
							
						}
						else
						{
							GUI.Label (new Rect(10,50,150,300),"GENERAL, WE HAVE NO UNITS! RETREAT!");
						}
					}
					
					if(attackingUnit != null && playerTurn == 2)
					{
						GUI.Box (new Rect(100,100,200,300),"ATTACKER");
						GUI.Label (new Rect(100,130,200,40), attackingUnit.GetComponent<attributes>().unitName);
						GUI.DrawTexture(new Rect(150,180,100,100),attackingUnit.GetComponent<attributes>().unitImage);
						GUI.Label (new Rect(100, 300, 200, 40), "HP: " + attackingUnit.GetComponent<attributes>().hp);
					}
				
					//DEFENDING UNIT SELECTION
					if(playerTurn == 1 && redDeclareAttacker && !blueDeclareDefender)
					{
						
						if(blueGeneralUnits[0] != GUIContent.none)
						{
							GUI.Label (new Rect(10, 20, 200,40),"DECLARE DEFENDER");
							int selectedUnit = GUI.SelectionGrid (new Rect(10, 50,50,300),-1, blueGeneralUnits, 1);
							defenderInt = selectedUnit;
							if(selectedUnit > -1)
							{
								if(blueGeneral.units[selectedUnit] != null)
								{
									defendingUnit = blueGeneral.units[selectedUnit];
									blueDeclareDefender = true;
								}
							}
						}
						else
						{
							GUI.Label (new Rect(10,50,150,300),"ALL UNITS DESTROYED! RETREAT!");
						}	
					}
					if(defendingUnit != null && playerTurn == 1)
					{
						GUI.Box (new Rect(100,100,200,300),"DEFENDER");
						GUI.Label (new Rect(100,130,200,40), defendingUnit.GetComponent<attributes>().unitName);
						GUI.DrawTexture(new Rect(150,180,100,100),defendingUnit.GetComponent<attributes>().unitImage);
						GUI.Label (new Rect(100, 300, 200, 40), "HP: " + defendingUnit.GetComponent<attributes>().hp);
					}
			
					if(blueUnitDestroyed)
					{
						GUI.DrawTexture(new Rect(150,180,100,100),explosion);
					}
			GUI.EndGroup();
			
			GUI.Label (new Rect(Screen.width/2 - 50, 30,100,40),"COMBAT");
			
			if(attackingUnit != null && defendingUnit != null)
			{
				if(!damageDealt)
				{
					damage = attackingUnit.GetComponent<attributes>().rollDamage();
					defendingUnit.GetComponent<attributes>().hp -= damage;
					if(defendingUnit.GetComponent<attributes>().hp <= 0)
					{
						if(playerTurn == 1) //if red is attacking and destroys blue
						{
							blueGeneral.GetComponent<GeneralUnits>().removeUnit(defenderInt);
							blueUnitDestroyed = true;
						}
						else if(playerTurn == 2)
						{
							redGeneral.GetComponent<GeneralUnits>().removeUnit(defenderInt);
							redUnitDestroyed = true;
						}
					}
						damageDealt = true;
				}
				GUI.Label (new Rect(Screen.width/2 - 300, Screen.height - 120,600,60),"Attacker deals " + damage.ToString() + " damage to defender. End your turn.");
			}
			if(redUnitDestroyed || blueUnitDestroyed)
			{
				GUI.Label (new Rect(Screen.width/2 - 300, Screen.height - 120,600,60),"DEFENDER DESTROYED BY " + damage.ToString() + " DAMAGE FROM ATTACKER. End your turn.");
			}
			
			
		}
	}
}