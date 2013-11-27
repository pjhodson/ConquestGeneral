using UnityEngine;
using System.Collections;

public class UnitsInBase : MonoBehaviour{
	
	private string unitType;
	private int turnStarted;
	
	public UnitsInBase(int turn, string type)
	{
		turnStarted=turn;
		unitType=type;
	}
	
	public string getUnitType(){return unitType;}
	public int getTurnStarted(){return turnStarted;}
	
	public void setUnitType(string type){unitType=type;}
	public void setTurnStarted(int turn){turnStarted=turn;}
}
