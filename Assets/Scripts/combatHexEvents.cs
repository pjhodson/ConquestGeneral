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
		if(!highlit)
		{
			this.renderer.material.color = Color.yellow;
			highlit = true;
		}
		
	}
	
	public void highlightLR()
	{
		if(!highlit)
		{
			this.renderer.material.color = new Color(0.99f,0.4f,.98f,1);
			highlit = true;
		}
	}
	
	public void highlightLB()
	{
		
		if(!highlit)
		{
			this.renderer.material.color = new Color(0.4f,0.4f,.98f,1);
			highlit = true;
		}
		
	}
	
	void OnMouseEnter()
	{
		mouseOver = true;
		if(!isSelected && !highlit)
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
			
			
			RaycastHit hit;
			Debug.DrawRay (transform.position,transform.up);
			if(Physics.Raycast(transform.position,transform.up,out hit))
			{
				switch(hit.collider.gameObject.name)
				{
				case "Red Grunt(Clone)":
				case "Blue Grunt(Clone)":
					hit.collider.gameObject.GetComponent<gruntAttributes>().selected = true;
					break;
				case "Red Tank(Clone)":
				case "Blue Tank(Clone)":
					hit.collider.gameObject.GetComponent<tankAttributes>().selected = true;
					break;
				case "Red Plane(Clone)":
				case "Blue Plane(Clone)":
					hit.collider.gameObject.GetComponent<planeAttributes>().selected = true;
				 	break;
				}
			}
			
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
		if(!isSelected && !highlit)
		{
			this.renderer.material.color = Color.white;
		}
		if(highlit)
		{
			highlight();
		}
	}
}
