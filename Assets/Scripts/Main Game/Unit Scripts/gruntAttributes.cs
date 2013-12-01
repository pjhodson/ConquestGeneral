using UnityEngine;
using System.Collections;

public class gruntAttributes : MonoBehaviour {

	public int HP;
	public int rangedAttackRange;
	public int meleeAirAttackRange;
	public int moveRange;
	
	public int meleeDieVal;
	public int rangedDieVal;
	
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
