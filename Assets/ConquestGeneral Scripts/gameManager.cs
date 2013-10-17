using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {
	
	private int playerTurn;
	
	public GameObject boardManager;
	
	FX_Map_Manager testing;
		
	// Use this for initialization
	void Start () {
		playerTurn = Random.Range (0,1); //Flip a coin to see who goes first
		
		testing = boardManager.GetComponent<FX_Map_Manager>();
		
		
	
	}
	
	// Update is called once per frame
	void Update () {
		
		Debug.Log (testing.CurrentHex);
	
	}
}
