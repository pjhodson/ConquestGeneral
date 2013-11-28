using UnityEngine;
using System.Collections;

public class Hangar : Building_BASE_CLASS{

	public Hangar(int turn, MonoBehaviour caller) : base(caller)
	{
		buildTime = 7;
		trainTime = 7;
		
		unitName = "Plane";
		
		buildingInfo = new GUIContent("HANGAR","Click here to build a plane");
		
		startBuilding(turn);
	}
	
	
}
