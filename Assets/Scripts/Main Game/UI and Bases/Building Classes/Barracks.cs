using UnityEngine;
using System.Collections;

public class Barracks : Building_BASE_CLASS{
	
	public Barracks(int turn, MonoBehaviour caller) : base(caller)
	{
		buildTime = 3;
		trainTime = 2;
		
		unitName = "Grunt";
		
		buildingInfo = new GUIContent("BARRACKS","Click here to build a grunt");
		
		startBuilding(turn);
	}
	
}
