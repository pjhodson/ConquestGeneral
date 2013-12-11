using UnityEngine;
using System.Collections;

public class GeneralEvents : MonoBehaviour {
	
	
	private BoardMaker boardInfo;
	
	public Vector2 myCoords;
	
	// Use this for initialization
	void Start () {
		boardInfo = GameObject.Find ("BoardManager").GetComponent<BoardMaker>();
	}
	
	// Update is called once per frame
	void Update () {
			
	}
	
	void OnCollisionEnter(Collision general)
	{
		//COMBAT! Need a variable in BoardMaker to set combat and determine who started it.
		if(this.gameObject.tag == "red general" && general.gameObject.tag == "blue general")
		{
			this.gameObject.rigidbody.detectCollisions = false;
			this.gameObject.rigidbody.isKinematic = true;
			general.gameObject.rigidbody.detectCollisions = false;
			general.gameObject.rigidbody.isKinematic = true;
			boardInfo.inCombat = true;
		}
		else if (this.gameObject.tag == "blue general" && general.gameObject.tag == "red general")
		{
			this.gameObject.rigidbody.detectCollisions = false;
			this.gameObject.rigidbody.isKinematic = true;
			general.gameObject.rigidbody.detectCollisions = false;
			general.gameObject.rigidbody.isKinematic = true;
			boardInfo.inCombat = true;
		}
		
	
	}
	
	void OnTriggerEnter(Collider collision)
	{
		//Debug.Log ("Trigger Entered");
		if(this.gameObject.tag == "red general" && collision.gameObject.tag == "red base")
		{
			boardInfo.redGeneralHome = true;
		}
		else if(this.gameObject.tag == "red general" && collision.gameObject.tag == "blue base")
		{
			boardInfo.redGeneralBlueBase = true;
		}
		
		if(this.gameObject.tag == "blue general" && collision.gameObject.tag == "blue base")
		{
			boardInfo.blueGeneralHome = true;
		}
		else if(this.gameObject.tag == "blue general" && collision.gameObject.tag == "red base")
		{
			boardInfo.blueGeneralRedBase = true;
		}
		
		if(collision.gameObject.tag == "ADP")
		{
			this.GetComponent<GeneralUnits>().addADToAll();
			Destroy (collision.gameObject);
		}
		if(collision.gameObject.tag == "HPP")
		{
			this.GetComponent<GeneralUnits>().addHPToAll();
			Destroy (collision.gameObject);
		}
		
	}
	
	void OnTriggerExit()
	{
		boardInfo.redGeneralHome = false;
		boardInfo.redGeneralBlueBase = false;
		boardInfo.blueGeneralHome = false;
		boardInfo.blueGeneralRedBase = false;
	}
		
}
