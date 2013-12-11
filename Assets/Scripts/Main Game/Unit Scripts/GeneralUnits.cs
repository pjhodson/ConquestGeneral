using UnityEngine;
using System.Collections;
using System;

public class GeneralUnits : MonoBehaviour {
	
	public GameObject[] units;
	public string[] unitNames;
	public int counter;
	
	// Use this for initialization
	void Start () {
		units = new GameObject[10];
		unitNames = new string[10];
		counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void addHPToAll()
	{
		for(int i = 0; i < counter; i++)
		{
			if(units[i] != null)
			{
				units[i].GetComponent<attributes>().hp += 1;	
			}
		}
	}
	
	public void addADToAll()
	{
		for(int i = 0; i < counter; i++)
		{
			if(units[i] != null)
			{
				units[i].GetComponent<attributes>().dmgMod += 1;	
			}
		}
	}
	
	
	public void addUnit(GameObject unit)
	{
		units[counter] = (GameObject)Instantiate(unit);
		unitNames[counter] = units[counter].name;
		units[counter].SetActive(false);
		counter++;
	}
	
	public void returnUnit(GameObject unit)
	{
		units[counter] = unit;
		unitNames[counter] = unit.name;
		units[counter].SetActive(false);
		counter++;
	}

	public void removeUnit(int unitRemoved)
	{
		Destroy(units[unitRemoved]);
		units[unitRemoved] = null;
		unitNames[unitRemoved] = null;
		Array.Sort(units, delegate(GameObject go1, GameObject go2) 
			{ if(go1 == null && go2 != null) 
				{ return 1; } 
				else if((go1 == null && go2 == null) || (go1 != null && go2 != null))
				{
					return 0;
				}
				else 
				{
					return -1;
				}
			}
		);
		Array.Sort(unitNames, delegate(string go1, string go2) 
			{ if(go1 == null && go2 != null) 
				{ return 1; } 
				else if((go1 == null && go2 == null) || (go1 != null && go2 != null))
				{
					return 0;
				}
				else 
				{
					return -1;
				}
			}
		);
		counter--;
	}
	
	public bool isFull()
	{
		if(counter == 10)
		{
			return true;
		}
		else return false;
	}
}
