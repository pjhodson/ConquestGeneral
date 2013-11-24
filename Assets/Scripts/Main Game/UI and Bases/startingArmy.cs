using UnityEngine;
using System.Collections;

public class startingArmy : MonoBehaviour {
	
	public bool gameStart;
	
	public GameObject redGrunt, blueGrunt;
	public GameObject redTank, blueTank;
	public GameObject redLaBomba, blueLaBomba;
	
	public int gruntCost;
	public int tankCost;
	public int laBombaCost;
	
	private int redHP;
	private int blueHP;
	
	private bool redPlane;
	private bool bluePlane;
	
	private int numRGrunt, numRTank, numRPlane;
	private int numBGrunt, numBTank, numBPlane;
	
	public Texture blackPixel;
	
	private GeneralUnits redGeneralUnits;
	private GeneralUnits blueGeneralUnits;
	
	// Use this for initialization
	void Awake () {
		gameStart = true;
		redHP = blueHP = 30;
		
		numRGrunt = numRTank = numRPlane = 0;
		numBGrunt = numBTank = numBPlane = 0;
		
		
		
	}
	
	void Start() {
		redGeneralUnits = GameObject.Find ("Red General").GetComponent<GeneralUnits>();
		blueGeneralUnits = GameObject.Find ("Blue General").GetComponent<GeneralUnits>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		
		if(gameStart)
		{
			GUI.BeginGroup(new Rect(0,0,Screen.width,Screen.height));
				GUI.DrawTexture (new Rect(0,0,Screen.width,Screen.height),blackPixel);
				GUI.BeginGroup (new Rect(Screen.width/4,Screen.height/2 - Screen.height/4,Screen.width/2,280));
				
				GUI.Box (new Rect(0,0,Screen.width/4,250), "Red Starting Army");
					GUI.Label (new Rect(0,20,Screen.width/4,30),redHP.ToString());
			
					if(redHP < gruntCost){
						GUI.enabled = false;
					}
					if(GUI.Button (new Rect(0,50,Screen.width/4,50), "Add Grunt (-" + gruntCost.ToString() + ")") && redHP >= gruntCost)
					{
						redGeneralUnits.addUnit(redGrunt);
						redHP -= gruntCost;
						numRGrunt++;
					}
					GUI.enabled = true;
			
					if(redHP < tankCost){
						GUI.enabled = false;
					}
					if(GUI.Button (new Rect(0,105,Screen.width/4,50), "Add Tank (-" + tankCost.ToString() + ")") && redHP >= tankCost)
					{
						redGeneralUnits.addUnit(redTank);
						redHP -= tankCost;
						numRTank++;
					}
					GUI.enabled = true;
			
					if(redPlane || redHP < laBombaCost){
						GUI.enabled = false;
					}
					if(GUI.Button (new Rect(0,160,Screen.width/4,50), "Add Plane (-" + laBombaCost.ToString() + ")") && redHP >= laBombaCost && !redPlane)
					{
						redGeneralUnits.addUnit(redLaBomba);
						redHP -= laBombaCost;
						redPlane = true;
						numRPlane++;
					}
					GUI.enabled = true;
					GUI.Label (new Rect(0,215,Screen.width/4,30), "G: " + numRGrunt.ToString() + " T: " + numRTank.ToString() + " P: " + numRPlane.ToString());
			
			
				GUI.Box (new Rect(Screen.width/4,0,Screen.width/4,250), "Blue Starting Army");
					GUI.Label (new Rect(Screen.width/4,20,Screen.width/4,30),blueHP.ToString());
			
					if(blueHP < gruntCost){
						GUI.enabled = false;
					}
					if(GUI.Button (new Rect(Screen.width/4,50,Screen.width/4,50), "Add Grunt (-" + gruntCost.ToString() + ")") && blueHP >= gruntCost)
					{
						blueGeneralUnits.addUnit(blueGrunt);
						blueHP -= gruntCost;
						numBGrunt++;
					}
					GUI.enabled = true;
			
					if(blueHP < tankCost){
						GUI.enabled = false;
					}
					if(GUI.Button (new Rect(Screen.width/4,105,Screen.width/4,50), "Add Tank (-" + tankCost.ToString() + ")") && blueHP >= tankCost)
					{
						blueGeneralUnits.addUnit(blueTank);
						blueHP -= tankCost;
						numBTank++;
					}
					GUI.enabled = true;
			
					if(bluePlane || blueHP < laBombaCost){
						GUI.enabled = false;
					}
					if(GUI.Button (new Rect(Screen.width/4,160,Screen.width/4,50), "Add Plane (-" + laBombaCost.ToString() + ")") && blueHP >= laBombaCost && !bluePlane)
					{
						blueGeneralUnits.addUnit(blueLaBomba);
						blueHP -= laBombaCost;
						bluePlane = true;
						numBPlane++;
					}
					GUI.enabled = true;
					GUI.Label (new Rect(Screen.width/4,215,Screen.width/4,30), "G: " + numBGrunt.ToString() + " T: " + numBTank.ToString() + " P: " + numBPlane.ToString());
					
					if(redHP < gruntCost && blueHP < gruntCost)
					{
						if(GUI.Button (new Rect(0,250,Screen.width/2,30), "FIGHT"))
						{
							gameStart = false;
						}
					}
			GUI.EndGroup();
			GUI.EndGroup();
			
			
			
		}
	}
}
