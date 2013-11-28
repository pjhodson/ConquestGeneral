using UnityEngine;
using System.Collections;

public class Factory : Building_BASE_CLASS{
	
	public Factory(int turn, MonoBehaviour caller) : base(caller)
	{
		buildTime = 5;
		trainTime = 4;
		
		unitName = "Tank";
		
		buildingInfo = new GUIContent("FACTORY","Click here to build a tank");
		
		startBuilding(turn);
	}
	
}
