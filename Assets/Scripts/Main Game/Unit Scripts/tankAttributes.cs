using UnityEngine;
using System.Collections;

public class tankAttributes : MonoBehaviour {
	
	public int HP;
	public int rangedLandAttackRange;
	public int rangedAirAttackRange;
	public int moveRange;
	
	public int rangedLandDieVal;
	public int rangedAirDieVal;
	
	public bool selected;
	
	void Start()
	{
		selected = false;
	}
	
	void OnGUI()
	{
		if(selected)
		{
			Vector3 toScreenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
			GUI.Box (new Rect(toScreenPoint.x + 10, Screen.height - toScreenPoint.y, 100,100),"");
			
		}
	}
}
