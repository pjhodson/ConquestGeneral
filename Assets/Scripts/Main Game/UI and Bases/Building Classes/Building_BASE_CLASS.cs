using UnityEngine;
using System.Collections;

public class Building_BASE_CLASS {

	protected int buildTime;
	
	protected int trainTime;
	protected int startTrain; 
	
	protected int startBuild;
	
	protected string unitName;
	
	protected bool currentlyTraining;
	
	protected GUIContent buildingInfo;
	
	protected int numUnitsTrained;
	
	protected MonoBehaviour caller;
	
	public Building_BASE_CLASS(MonoBehaviour callingScript)
	{
		currentlyTraining = false;
		numUnitsTrained = 0;
		caller = callingScript;
	}
		
	public void startBuilding(int currentTurn)
	{
		startBuild = currentTurn;
		//Debug.Log ("Started Building at turn " + startBuild + ". Expected Completion turn " + startBuild + buildTime);
	}
	
	public bool doneBuilding(int currentTurn) //Check if done.
	{
		if((startBuild + buildTime)<= currentTurn)
		{
			return true;
		}
		else return false;
	}
	
	public void startTraining(int currentTurn)
	{
		startTrain = currentTurn;
		currentlyTraining = true;
	}
	
	public void doneTraining(int currentTurn)
	{
		if(currentlyTraining && (startTrain + trainTime) <= currentTurn)
		{
			if(caller.GetComponent<RedBase>())
			{
				caller.GetComponent<RedBase>().addTroops(unitName);
				currentlyTraining = false;
			}
			else if(caller.GetComponent<BlueBase>())
			{
				caller.GetComponent<BlueBase>().addTroops(unitName);
				currentlyTraining = false;
			}
		}
	}
	
	public GUIContent getBuildingInfo()
	{
		return buildingInfo;
	}
	
	public string getUnitName()
	{
		return unitName;
	}

	public bool getCurrentlyTraining()
	{
		return currentlyTraining;
	}
	
	public int getNumUnitsTrained()
	{
		return numUnitsTrained;
	}
}
