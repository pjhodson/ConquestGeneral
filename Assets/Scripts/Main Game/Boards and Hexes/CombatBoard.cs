using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatBoard : MonoBehaviour {

//following public variable is used to store the hex model prefab;
    //instantiate it by dragging the prefab on this variable using unity editor
    public GameObject Hex;
		
    //next two variables can also be instantiated using unity editor
    public int gridWidthInHexes = 10;
    public int gridHeightInHexes = 10;
	
	public List<Vector2> selectedHexes;
	
	private GameObject[,] hexArray;
 
	public bool aHexIsSelected;
    //Hexagon tile width and height in game world
    private float hexWidth;
    private float hexHeight;
	
	public int selectedUnit;
	
	private BoardMaker boardInfo;
	
	public List<GameObject> redUnits, blueUnits;
	private List<Vector2> redUnitPositions, blueUnitPositions;
	
	//The grid should be generated on game start
    void Awake()
    {
		selectedUnit = -1;
		aHexIsSelected = false;
		hexArray = new GameObject[gridWidthInHexes,gridHeightInHexes];
        setSizes();
        createGrid();
		
		redUnits = new List<GameObject>();
		blueUnits = new List<GameObject>();
		
		redUnitPositions = new List<Vector2>();
		blueUnitPositions = new List<Vector2>();
		
		boardInfo = GameObject.Find ("BoardManager").GetComponent<BoardMaker>();
	}
	
    //Method to initialise Hexagon width and height
    void setSizes()
    {
        //renderer component attached to the Hex prefab is used to get the current width and height
        hexWidth = Hex.renderer.bounds.size.x;
        hexHeight = Hex.renderer.bounds.size.z;
    }
 
    //Method to calculate the position of the first hexagon tile
    //The center of the hex grid is (0,0,0)
    Vector3 calcInitPos()
    {
        Vector3 initPos;
        //the initial position will be in the left upper corner
        initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2 + this.transform.position.x, 0, gridHeightInHexes / 2f * hexHeight - hexHeight / 2 + this.transform.position.z);
        return initPos;
    }
 
    //method used to convert hex grid coordinates to game world coordinates
    public Vector3 calcWorldCoord(Vector2 gridPos)
    {
		//Position of the first hex tile
        Vector3 initPos = calcInitPos();
        //Every second row is offset by half of the tile width
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;
 
        float x =  initPos.x + offset + gridPos.x * hexWidth;
        //Every new line is offset in z direction by 3/4 of the hexagon height
        float z = initPos.z - gridPos.y * hexHeight * 0.75f;
        return new Vector3(x, 0, z);
    }
 
    //Finally the method which initialises and positions all the tiles
    void createGrid()
    {
        //Game object which is the parent of all the hex tiles
        GameObject hexGridGO = new GameObject("HexGrid");
 
        for (int y = 0; y < gridHeightInHexes; y++)
        {
            for (int x = 0; x < gridWidthInHexes; x++)
            {
                //GameObject assigned to Hex public variable is cloned
                hexArray[x,y] = (GameObject)Instantiate (Hex);
				hexArray[x,y].GetComponent<combatHexEvents>().myCoords = new Vector2(x,y);
                //Current position in grid
                Vector2 gridPos = new Vector2(x, y);
                hexArray[x,y].transform.position = calcWorldCoord(gridPos);
                hexArray[x,y].transform.parent = hexGridGO.transform;
            }
        }
    }
 
	int calculateHexDistance(Vector2 firstHex, Vector2 secondHex)
	{
		//Translate Array coords into hex coords http://www-cs-students.stanford.edu/~amitp/Articles/HexLOS.html
		int fhHexValx = ((int)firstHex.x - Mathf.FloorToInt(firstHex.y/2));
		int fhHexValy = ((int)firstHex.x + Mathf.CeilToInt (firstHex.y/2));
		
		int shHexValx = ((int)secondHex.x - Mathf.FloorToInt (secondHex.y/2));
		int shHexValy = ((int)secondHex.x + Mathf.CeilToInt(secondHex.y/2));
		
		//Calculate Deltas	
		int deltaX = fhHexValx - shHexValx;
		int deltaY = fhHexValy - shHexValy;
		
		//Calculate distance
		if(Mathf.Sign (deltaX) == Mathf.Sign (deltaY))
		{
			return Mathf.Max (Mathf.Abs (deltaX),Mathf.Abs (deltaY));
		}
		else return Mathf.Abs(deltaX) + Mathf.Abs(deltaY);
	}
		
	public void deselectAll()
	{
		selectedHexes.Clear ();
		for(int x = 0; x < gridWidthInHexes; x++)
		{
			for(int y = 0; y < gridHeightInHexes; y++)
			{
				hexArray[x,y].GetComponent<combatHexEvents>().isSelected = false;
				hexArray[x,y].GetComponent<combatHexEvents>().highlit = false;
			}
		}
		aHexIsSelected = false;
	}
	
	public void spawn (GameObject spawnMe, string theColor)
	{
		spawnMe.SetActive(true);
		if(theColor == "red")
		{
			redUnits.Add (spawnMe);
			spawnMe.transform.position = calcWorldCoord(selectedHexes[0]);
			redUnitPositions.Add (selectedHexes[0]);
		}
		else if(theColor == "blue")
		{
			blueUnits.Add (spawnMe);
			spawnMe.transform.position = calcWorldCoord(selectedHexes[0]);
			Debug.Log ("Spawning at " + calcWorldCoord(selectedHexes[0]));
			blueUnitPositions.Add (selectedHexes[0]);
		}
		
	}
	
	public bool spawnHexOccupied(string theColor)
	{
		if(theColor == "red")
		{
			for(int i = 0; i < redUnitPositions.Count; i++)
			{
				if(selectedHexes[0] == redUnitPositions[i])
				{
					return true;
				}
			}
			return false;
		}
		else if(theColor == "blue")
		{
			for(int i = 0; i < blueUnitPositions.Count; i++)
			{
				if(selectedHexes[0] == blueUnitPositions[i])
				{
					return true;
				}
			}
			return false;
		}
		else return false;
	}
	
	public void hexSelected(Vector2 SelectedCoords)
	{
		aHexIsSelected = true;
		selectedHexes.Add (SelectedCoords);
		Debug.Log (selectedHexes[0]);
	}
	
	public void hexDeselected(Vector2 DeselectCoords)
	{
		for(int i = 0; i < selectedHexes.Count; i++)
		{
			if(selectedHexes[i].x == DeselectCoords.x && selectedHexes[i].y == DeselectCoords.y)
			{
				selectedHexes.RemoveAt(i);
			}
		}
	}
	
	void highlightMoves(Vector2 center, int range, Color highlightColor)
	{

	}
	
	
	void moveHandler(string whoseTurn)
	{

	}
	
	public void returnUnits()
	{
		Debug.Log (redUnits.Count);
		foreach (GameObject go in redUnits)
		{
			GameObject.Find ("Red General").GetComponent<GeneralUnits>().returnUnit(go);
		}
		redUnitPositions.Clear ();
		redUnits.Clear ();
		
		foreach (GameObject go in blueUnits)
		{
			GameObject.Find ("Blue General").GetComponent<GeneralUnits>().returnUnit(go);
		}
		blueUnitPositions.Clear();
		blueUnits.Clear ();
		
		this.GetComponent<placeRedTroops>().troopsPlaced = false;
		this.GetComponent<placeBlueTroops>().troopsPlaced = false;
	}
		
	
	public void highlightHexes(string theColor)
	{
		if(theColor == "red")
		{
			for(int i = 0; i < 3; i++)
			{
				for(int j = 0; j < gridHeightInHexes; j++)
				{
					hexArray[i,j].GetComponent<combatHexEvents>().highlightLR();
				}
			}
		}
		else if (theColor == "blue")
		{
			for(int i = gridWidthInHexes - 3; i < gridWidthInHexes; i++)
			{
				for(int j = 0; j < gridHeightInHexes; j++)
				{
					hexArray[i,j].GetComponent<combatHexEvents>().highlightLB();
				}
			}
		}
	}
	
	void Update()
	{
				
	}

}