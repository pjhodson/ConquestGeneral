using UnityEngine;
using System.Collections;

public class HexEvents : MonoBehaviour {
	
	public bool isSelected;
	public bool redGeneralOnMe, blueGeneralOnMe;
	
	private BoardMaker boardInfo;
	
	private bool mouseOver;
	public bool highlit;
	public Vector2 myCoords;
	
	// Use this for initialization
	void Start () {
		isSelected = false;
		redGeneralOnMe = false;
		blueGeneralOnMe = false;
		highlit = false;
		boardInfo = GameObject.Find ("BoardManager").GetComponent<BoardMaker>();
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
			
			
			RaycastHit hit;
			Debug.DrawRay (transform.position + new Vector3(.25f,0,.25f),transform.up);
			if(Physics.Raycast(transform.position + new Vector3(.25f,0,.25f),transform.up,out hit))
			{
				if(hit.collider.gameObject.tag == "red general")
				{
					redGeneralOnMe = true;
				}
				else if(hit.collider.gameObject.tag == "blue general")
				{
					blueGeneralOnMe = true;
				}
				else
				{
					redGeneralOnMe = false; 
					blueGeneralOnMe = false;
				}
			}
			
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
