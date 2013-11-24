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
 
    //Hexagon tile width and height in game world
    private float hexWidth;
    private float hexHeight;
	
	//The grid should be generated on game start
    void Awake()
    {
		hexArray = new GameObject[gridWidthInHexes,gridHeightInHexes];
        setSizes();
        createGrid();
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
		
	void deselectAll()
	{
		for(int x = 0; x < gridWidthInHexes; x++)
		{
			for(int y = 0; y < gridHeightInHexes; y++)
			{
				hexArray[x,y].GetComponent<combatHexEvents>().isSelected = false;
				hexArray[x,y].GetComponent<combatHexEvents>().highlit = false;
			}
		}
	}
	
	public void hexSelected(Vector2 SelectedCoords)
	{
		/*if(selectedHexes.Count == 0 && 
			(hexArray[(int)SelectedCoords.x,(int)SelectedCoords.y].GetComponent<combatHexEvents>().redGeneralOnMe == true) && playerTurn == 1 && !generalMoved)
			{
				selectedHexes.Add (SelectedCoords);
				highlightMoves(SelectedCoords);
			}
		else if(selectedHexes.Count == 0 &&
			(hexArray[(int)SelectedCoords.x,(int)SelectedCoords.y].GetComponent<combatHexEvents>().blueGeneralOnMe == true) && playerTurn == 2 && !generalMoved)
			{
				selectedHexes.Add (SelectedCoords);
				highlightMoves(SelectedCoords);
			}
		else if(selectedHexes.Count > 0)
		{
			selectedHexes.Add (SelectedCoords);
		}
		else
		{
			Debug.Log ("Invalid Move");
			deselectAll();
		}*/
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
	
	void highlightMoves(Vector2 center)
	{
		//This needs to be optimized because it slows the game down to ~ 13FPS at the moment. Get coords of general and limit search to a 12x12 radius. 144 < 1250.
		for(int x = 0; x < gridWidthInHexes; x++)
		{
			for(int y = 0; y < gridHeightInHexes; y++)
			{
				if(calculateHexDistance(center,new Vector2(x,y)) > 0 && calculateHexDistance(center,new Vector2(x,y)) <= 6)
				{
					Debug.Log ("highlighting " + x + " " + y);
					hexArray[x,y].GetComponent<combatHexEvents>().highlight();
				}
			}
		}
	}
	
	
	void moveHandler(string whoseTurn)
	{
		/*if(whoseTurn == "red") 
		{
			if(selectedHexes.Count == 2)
			{
				if(calculateHexDistance(selectedHexes[0],selectedHexes[1]) <= 6 && !generalMoved) {
					string selectedGeneral = "";
					if(hexArray[(int)selectedHexes[0].x,(int)selectedHexes[0].y].GetComponent<combatHexEvents>().redGeneralOnMe == true)
					{
						selectedGeneral = "red";
					}
					
					Debug.Log ("Moving General");
					StartCoroutine (moveGeneral(selectedGeneral, calcWorldCoord(selectedHexes[1]) + new Vector3(0,0.5f,0)));
					generalMoved = true;
				}
				else Debug.Log ("Invalid Move");
			
				selectedHexes.Clear();
				deselectAll();
				
			}
		}
		else if(whoseTurn == "blue")
		{
			if(selectedHexes.Count == 2)
			{
				if(calculateHexDistance(selectedHexes[0],selectedHexes[1]) <= 6 && !generalMoved) {
					string selectedGeneral = "";
					if(hexArray[(int)selectedHexes[0].x,(int)selectedHexes[0].y].GetComponent<combatHexEvents>().blueGeneralOnMe == true)
					{
						selectedGeneral = "blue";
					}
					
					Debug.Log ("Moving General");
					StartCoroutine (moveGeneral(selectedGeneral, calcWorldCoord(selectedHexes[1]) + new Vector3(0,0.5f,0)));
					generalMoved = true;					
				}
				else Debug.Log ("Invalid Move");
			
				selectedHexes.Clear();
				deselectAll();
			}
		}*/
	}
	
	void Update()
	{
				
	}
	
	IEnumerator camLerp(Vector3 movePos)
	{
		while(Camera.main.transform.position != movePos)
		{
			Camera.main.transform.position = Vector3.MoveTowards (Camera.main.transform.position, movePos, 50 * Time.deltaTime);
			yield return null;
		}
	}

}