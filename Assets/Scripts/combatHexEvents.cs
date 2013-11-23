using UnityEngine;
using System.Collections;

public class combatHexEvents : MonoBehaviour {
	
	public bool isSelected;
		
	private CombatBoard boardInfo;
	
	private bool mouseOver;
	public bool highlit;
	public Vector2 myCoords;
	
	// Use this for initialization
	void Start () {
		
		isSelected = false;
		highlit = false;
		boardInfo = GameObject.Find ("CombatBoard").GetComponent<CombatBoard>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!isSelected && !mouseOver && !highlit)
		{
			this.renderer.material.color = Color.white;
		}
	}
	
	public void highlight()
	{
		highlit = true;
		this.renderer.material.color = Color.yellow;
		
	}
	
	void OnMouseEnter()
	{
		mouseOver = true;
		if(!isSelected)
		{
			this.renderer.material.color = Color.red;
			
		}
	}
	
	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(0) && !isSelected)
		{
			isSelected = true;
			
			this.renderer.material.color = Color.blue;
			
			
			boardInfo.hexSelected(myCoords);
			
		}
		else if(Input.GetMouseButtonDown (0) && isSelected)
		{
			isSelected = false;
			boardInfo.hexDeselected(myCoords);
			this.renderer.material.color = Color.red;
		}
	}
	
	void OnMouseExit()
	{
		mouseOver = false;
		if(!isSelected)
		{
			this.renderer.material.color = Color.white;
		}
		if(highlit)
		{
			highlight();
		}
	}
}
