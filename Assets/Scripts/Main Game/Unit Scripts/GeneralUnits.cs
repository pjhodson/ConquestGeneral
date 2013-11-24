using UnityEngine;
using System.Collections;

public class GeneralUnits : MonoBehaviour {
	
	public GameObject[] units;
	private int counter;
	
	// Use this for initialization
	void Start () {
		units = new GameObject[10];
		counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public void addUnit(GameObject unit)
	{
		units[counter] = unit;
		counter++;
	}

	public void removeUnit(int unitRemoved)
	{
		units[unitRemoved] = null;
		while(unitRemoved < 10)
		{
			units[unitRemoved] = units[unitRemoved+1];
			unitRemoved++;
		}
		counter--;
	}
}
