using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	
	public BoardMaker theBoard;
	
	public int scrollSensitivity;
	public int maxZoomIn;
	public int maxZoomOut;
	public int initZoom;
	
	// Use this for initialization
	void Start () {
		this.transform.position = theBoard.calcWorldCoord (new Vector2(0,0)) + new Vector3(0,initZoom,0);
	}
	
	// Update is called once per frame
	void Update () {
		
		
		//NOTE SCREEN MEASURED 0,0 IN BOTTOM LEFT
		
		//Camera Left/Right Movement
		if(Input.mousePosition.x >= Screen.width - 5 && (this.transform.position.x <= theBoard.calcWorldCoord(new Vector2(theBoard.gridWidthInHexes-1,0)).x))
		{			
			this.transform.Translate(new Vector3(scrollSensitivity, 0, 0) * Time.deltaTime);
		}
		if(Input.mousePosition.x <= 5 && (this.transform.position.x >= theBoard.calcWorldCoord(new Vector2(0,0)).x))
		{
			this.transform.Translate (new Vector3(-scrollSensitivity,0,0) * Time.deltaTime);
		}
		
		//Camera Up/Down Movement
		if(Input.mousePosition.y >= Screen.height - 5 && (this.transform.position.z <= theBoard.calcWorldCoord(new Vector2(0,0)).z))
		{			
			this.transform.Translate(new Vector3(0,scrollSensitivity,0) * Time.deltaTime);
		}
		
		if(Input.mousePosition.y <= 5 && (this.transform.position.z >= theBoard.calcWorldCoord(new Vector2(0,theBoard.gridHeightInHexes-1)).z))
		{
			
			this.transform.Translate (new Vector3(0,-scrollSensitivity,0) * Time.deltaTime);
		}
		
		//Zooming
		if(Input.GetAxis ("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize >= maxZoomIn)
		{
			Camera.main.orthographicSize -= scrollSensitivity*Time.deltaTime;
			
		}
		else if(Input.GetAxis ("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize <= maxZoomOut)
		{
			Camera.main.orthographicSize += scrollSensitivity*Time.deltaTime;
		}
		
	
	}
}
