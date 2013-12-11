using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardMaker : MonoBehaviour {

//following public variable is used to store the hex model prefab;
    //instantiate it by dragging the prefab on this variable using unity editor
    public GameObject Hex;
	public GameObject General;
	public GameObject Base;
	
	public int numHPPower, numADPower;
	public GameObject HPPower, ADPower;
	
	//public CombatBoard theCombatBoard;
	
	private int playerTurn;
	private int turnNumber;
	
    //next two variables can also be instantiated using unity editor
    public int gridWidthInHexes = 10;
    public int gridHeightInHexes = 10;
	
	public List<Vector2> selectedHexes;
	
	private GameObject[,] hexArray;
 
    //Hexagon tile width and height in game world
    private float hexWidth;
    private float hexHeight;
	
	private GameObject RedGeneral;
	private GameObject BlueGeneral;
	
	private GameObject RedBase;
	private GameObject BlueBase;
	
	public bool redGeneralHome,redGeneralBlueBase,blueGeneralHome,blueGeneralRedBase;
	
	public bool inCombat;
	//private bool combatCamMoved;
	
	public bool redEnterBase, blueEnterBase;
	
	private bool idiotStartMessages;
	
	private bool displayInvalid, displaySupply;
	private bool suppliesDropped;
	private bool displayMenu;
	
	//The grid should be generated on game start
    void Awake()
    {
		idiotStartMessages = true;
		displayInvalid = displaySupply = false;
		suppliesDropped = false;
		displayMenu = false;
		
		redGeneralHome = redGeneralBlueBase = blueGeneralHome = blueGeneralRedBase = false;
		inCombat = false;
		//combatCamMoved = false;
		
		redEnterBase = false;
		blueEnterBase = false;
		
		playerTurn = Random.Range (1,4);
		if(playerTurn % 2 == 0)
		{
			playerTurn = 1; //1 RED
		}
		else playerTurn = 2; //2 BLUE
		
		
		
		turnNumber = 1;
		
		hexArray = new GameObject[gridWidthInHexes,gridHeightInHexes];
        setSizes();
        createGrid();
		
		
		supplyDrop();
		
		//Instantiate Generals
		RedGeneral = (GameObject)Instantiate (General,calcWorldCoord (new Vector2(0,0))+ new Vector3(0,0.5f,0),Quaternion.identity);
		RedGeneral.name = "Red General";
		RedGeneral.tag = "red general";
		RedGeneral.renderer.material.color = Color.red;
		RedGeneral.GetComponent<GeneralEvents>().myCoords = new Vector2(0,0);
		
		BlueGeneral = (GameObject)Instantiate (General, calcWorldCoord (new Vector2(gridWidthInHexes-1,gridHeightInHexes-1))+ new Vector3(0,0.5f,0), Quaternion.identity);
		BlueGeneral.name = "Blue General";
		BlueGeneral.tag = "blue general";
		BlueGeneral.renderer.material.color = Color.blue;
		BlueGeneral.GetComponent<GeneralEvents>().myCoords = new Vector2(gridWidthInHexes-1,gridHeightInHexes-1);
		
		//Instantiate Bases
		RedBase = (GameObject)Instantiate (Base,calcWorldCoord (new Vector2(1,1)) + new Vector3(0,.8f,0),Quaternion.identity);
		RedBase.name = "Red Base";
		RedBase.tag = "red base";
		RedBase.renderer.material.color = new Color(.5f,0,0,0);
		
		BlueBase = (GameObject)Instantiate (Base,calcWorldCoord (new Vector2(gridWidthInHexes - 2, gridHeightInHexes -2)) + new Vector3(0,.8f,0),Quaternion.identity);
		BlueBase.name = "Blue Base";
		BlueBase.tag = "blue base";
		BlueBase.renderer.material.color = new Color(0,0,.5f,0);
		
		RedGeneral.rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
		BlueGeneral.rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
	}
	
	void Start()
	{
		
		if(playerTurn == 2)
		{
			Camera.main.transform.position = new Vector3(BlueGeneral.transform.position.x, Camera.main.transform.position.y, BlueGeneral.transform.position.z);
		}
	}
	
	void supplyDrop()
	{
		if(suppliesDropped) return;
		//POWERUP SPAWNING
		for(int i = 0; i < numHPPower; i++)
		{
			Instantiate (HPPower, calcWorldCoord( new Vector2(Random.Range (7,gridWidthInHexes - 7), Random.Range (0, gridHeightInHexes))) + new Vector3(0,0.5f,0), Quaternion.Euler(0,180,0)); //This has the potential to spawn powerups in the same square.
		}
		for(int i = 0; i < numADPower; i++)
		{
			Instantiate (ADPower, calcWorldCoord( new Vector2(Random.Range (7,gridWidthInHexes - 7), Random.Range (0, gridHeightInHexes))) + new Vector3(0,0.5f,0), Quaternion.Euler(0,180,0)); //This has the potential to spawn powerups in the same square.
		}
		suppliesDropped = true;
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
        initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2, 0, gridHeightInHexes / 2f * hexHeight - hexHeight / 2);
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
        return new Vector3(x, 1.5f, z);
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
				hexArray[x,y].GetComponent<HexEvents>().myCoords = new Vector2(x,y);
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
				hexArray[x,y].GetComponent<HexEvents>().isSelected = false;
				hexArray[x,y].GetComponent<HexEvents>().highlit = false;
			}
		}
	}
	
	public void hexSelected(Vector2 SelectedCoords)
	{
		if(selectedHexes.Count == 0 && 
			(hexArray[(int)SelectedCoords.x,(int)SelectedCoords.y].GetComponent<HexEvents>().redGeneralOnMe == true) && playerTurn == 1 && !generalMoved)
			{
				
				selectedHexes.Add (SelectedCoords);
				highlightMoves(SelectedCoords);
			}
		else if(selectedHexes.Count == 0 &&
			(hexArray[(int)SelectedCoords.x,(int)SelectedCoords.y].GetComponent<HexEvents>().blueGeneralOnMe == true) && playerTurn == 2 && !generalMoved)
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
			displayInvalid = true;
			deselectAll();
		}
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
					//Debug.Log ("highlighting " + x + " " + y);
					hexArray[x,y].GetComponent<HexEvents>().highlight();
				}
			}
		}
	}
	
	IEnumerator moveGeneral(GameObject which, Vector3 movePos)
	{
		if(which.name == "Red General")
		{
			General = RedGeneral;
		}
		else General = BlueGeneral;
		
		while(General.transform.position != movePos)
		{
			General.transform.position = Vector3.MoveTowards (General.transform.position, movePos, 10 * Time.deltaTime);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		General.transform.position = movePos;
	}
	
	private bool generalMoved = false;
	void moveHandler(string whoseTurn)
	{
		if(whoseTurn == "red") 
		{
			if(selectedHexes.Count == 2)
			{
				if(calculateHexDistance(selectedHexes[0],selectedHexes[1]) <= 6 && !generalMoved) {
					GameObject selectedGeneral = null;
					if(hexArray[(int)selectedHexes[0].x,(int)selectedHexes[0].y].GetComponent<HexEvents>().redGeneralOnMe == true)
					{
						selectedGeneral = RedGeneral;
						RedGeneral.GetComponent<GeneralEvents>().myCoords = selectedHexes[1];
					}
					
					StartCoroutine (moveGeneral(selectedGeneral, calcWorldCoord(selectedHexes[1])+ new Vector3(0,0.5f,0)));
					generalMoved = true;
				}
				else displayInvalid = true;
			
				selectedHexes.Clear();
				deselectAll();
				
			}
		}
		else if(whoseTurn == "blue")
		{
			if(selectedHexes.Count == 2)
			{
				if(calculateHexDistance(selectedHexes[0],selectedHexes[1]) <= 6 && !generalMoved) {
					GameObject selectedGeneral = null;
					if(hexArray[(int)selectedHexes[0].x,(int)selectedHexes[0].y].GetComponent<HexEvents>().blueGeneralOnMe == true)
					{
						selectedGeneral = BlueGeneral;
						BlueGeneral.GetComponent<GeneralEvents>().myCoords = selectedHexes[1];
					}
					
					StartCoroutine (moveGeneral(selectedGeneral, calcWorldCoord(selectedHexes[1])+ new Vector3(0,0.5f,0)));
					generalMoved = true;					
				}
				else displayInvalid = true;
			
				selectedHexes.Clear();
				deselectAll();
			}
		}
	}
	
	void Update()
	{
		if(playerTurn == 1) 
		{
			moveHandler ("red");
		}
		else moveHandler ("blue");	
		
		if (turnNumber % 15 == 0)
		{
			if(!suppliesDropped){
				displaySupply = true;
			}
			supplyDrop();
			StartCoroutine(removeSupplyAfter3());
		}
		if(turnNumber % 15 == 14)
		{
			suppliesDropped = false;
		}
			
		if(redGeneralBlueBase || blueGeneralRedBase)
		{
			string youWon;
			if(redGeneralBlueBase) youWon = "Red";
			else youWon = "Blue";
			GameObject.Find ("A stupid and essentially useless game object that will persist to win state to let new level know who won.").GetComponent<winner>().theWinner = youWon;
			Application.LoadLevel ("grats");
		}
		if(inCombat)
		{
			/*Debug.Log ("LERPY LERP LERP");
			StartCoroutine(camLerp (new Vector3(theCombatBoard.calcWorldCoord(new Vector2(0,0)).x, Camera.main.transform.position.y, theCombatBoard.calcWorldCoord(new Vector2(0,0)).z)));
			combatCamMoved = true;*/
		}
	}
	
	IEnumerator camLerp(Vector3 movePos)
	{
		Camera.main.transform.position = movePos;
		yield return null;
	}
	
	public int getPlayerTurn()
	{
		return playerTurn;
	}
	
	public int getTurnNumber()
	{
		return turnNumber;
	}
	
	IEnumerator removeInvalidAfter3()
	{
		yield return new WaitForSeconds(3.0f);
		displayInvalid = false;
	}
	
	IEnumerator removeSupplyAfter3()
	{
		yield return new WaitForSeconds(3.0f);
		displaySupply = false;
	}
	
	//GUI STUFF
	void OnGUI() {
		if(displayInvalid)
		{
			GUI.Box (new Rect(Screen.width/2 - 200, Screen.height - 30, 400, 30), "");
			GUI.Label (new Rect(Screen.width/2 - 200, Screen.height - 30, 400, 30), "INVALID MOVE");
			StartCoroutine (removeInvalidAfter3());
		}
		if(displaySupply)
		{
			GUI.Box (new Rect(Screen.width/2 - 200, Screen.height - 30, 400, 30), "");
			GUI.Label (new Rect(Screen.width/2 - 200, Screen.height - 30, 400, 30), "SUPPLY DROP!");
			//StartCoroutine (removeSupplyAfter3());
		}
		if(idiotStartMessages)
		{
			
			if(playerTurn == 1)
			{
				Vector3 toScreenPoint = Camera.main.WorldToScreenPoint(RedGeneral.transform.position);
				GUI.Label (new Rect(toScreenPoint.x -330, Screen.height - toScreenPoint.y, 300, 100),"Click the Hex beneath the general to select him. Then click any highlighted hex to move there. You only get one move per turn. Be careful!");
			}
			else if (playerTurn == 2)
			{
				Vector3 toScreenPoint = Camera.main.WorldToScreenPoint(BlueGeneral.transform.position);
				GUI.Label (new Rect(toScreenPoint.x + 30, Screen.height - toScreenPoint.y, 300, 100),"Click the Hex beneath the general to select him. Then click any highlighted hex to move there. You only get one move per turn. Be careful!");
			}
			
		}
		
		if(GUI.Button (new Rect(0,0,150,30),"MENU"))
		{
			displayMenu = true;
		}
		
		if(displayMenu)
		{
			GUI.BeginGroup(new Rect(Screen.width/2- 100, Screen.height/2 - 150, 200, 300));
			GUI.Box (new Rect(0, 0, 200, 300), "CONQUEST GENERAL");
			if(GUI.Button (new Rect(25,50,150, 30), "MAIN MENU"))
			{
				Application.LoadLevel("MainMenu");
			}
			if(GUI.Button (new Rect(25,85,150,30), "QUIT"))
			{
				Application.Quit();
			}
			
			if(GUI.Button (new Rect(25, 200,150,30), "RETURN TO GAME"))
			{
				displayMenu = false;
			}
			GUI.EndGroup();
		}
		
		string turnNotifier;
		if(playerTurn == 1)
		{
			turnNotifier = "Red's Turn";
		}
		else turnNotifier = "Blue's Turn";
		
		GUI.Box (new Rect(Screen.width/4, 0, Screen.width/2, 25), "");
		GUI.Label (new Rect(Screen.width/4+10, 0, 100,25), turnNotifier);
		GUI.Label (new Rect(Screen.width/4+110, 0, 100,25), "Turn " + turnNumber.ToString ());
		
		if(redGeneralHome && playerTurn == 1 || inCombat && playerTurn == 1)
		{
			if(GUI.Button (new Rect(Screen.width-100, Screen.height-100,100,50), "Enter Base"))
			{
				redEnterBase = true;
			}
		}
		
		if(blueGeneralHome && playerTurn == 2 || inCombat && playerTurn == 2)
		{
			if(GUI.Button (new Rect(Screen.width-100, Screen.height-100,100,50),"Enter Base"))
			{
				blueEnterBase = true;
			}
		}
		
		if(inCombat)
		{
			if(GUI.Button (new Rect(Screen.width-100, Screen.height-150,100,50),"Retreat"))
			{
				if(playerTurn == 2) 
				{
					BlueGeneral.transform.position = calcWorldCoord(new Vector2(gridWidthInHexes-1,gridHeightInHexes -1)) + new Vector3(0,0.5f,0);
				}
				else if(playerTurn == 1)
				{
					RedGeneral.transform.position = calcWorldCoord(new Vector2(0,0))+ new Vector3(0,0.5f,0);
				}
				
				RedGeneral.rigidbody.detectCollisions = true;
				RedGeneral.rigidbody.isKinematic = false;
				BlueGeneral.rigidbody.detectCollisions = true;
				BlueGeneral.rigidbody.isKinematic = false;
				
				//combatCamMoved = false;
				inCombat = false;
				
				//theCombatBoard.returnUnits();
				
				if(playerTurn == 1)
				{
					playerTurn = 2;
					if(!inCombat)
					{
						StartCoroutine(camLerp(new Vector3(BlueGeneral.transform.position.x, Camera.main.transform.position.y, BlueGeneral.transform.position.z)));
					}
				}
				else 
				{
					playerTurn = 1;
					if(!inCombat)
					{
						StartCoroutine(camLerp(new Vector3(RedGeneral.transform.position.x, Camera.main.transform.position.y, RedGeneral.transform.position.z)));
					}
				}
			
				turnNumber++;
				generalMoved = false; //set general back to being able to move			
			}
		}
			
		
		if(GUI.Button (new Rect(Screen.width-100, Screen.height-50,100,50), "End Turn")){
			displayInvalid = false;
			if(inCombat)
			{
				GameObject.Find ("Combat Cop Out").GetComponent<combatGUI>().resetAllOnTurnEnd();
			}
			idiotStartMessages = false;
			if(playerTurn == 1)
			{
				playerTurn = 2;
				if(!inCombat)
				{
					StartCoroutine(camLerp(new Vector3(BlueGeneral.transform.position.x, Camera.main.transform.position.y, BlueGeneral.transform.position.z)));
				}
			}
			else 
			{
				playerTurn = 1;
				if(!inCombat)
				{
					StartCoroutine(camLerp(new Vector3(RedGeneral.transform.position.x, Camera.main.transform.position.y, RedGeneral.transform.position.z)));
				}
			}
			//theCombatBoard.deselectAll();
			turnNumber++;
			generalMoved = false; //set general back to being able to move
		}
		
	}
}