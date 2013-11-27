using UnityEngine;
using System.Collections;

public class baseClickHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0))
		{
			RaycastHit vHit = new RaycastHit();
			Ray vRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(vRay, out vHit, 1000)) 
			{
				if(vHit.collider.tag == "red base")
				{
					GameObject.Find ("RBaseHandler").GetComponent<RedBase>().baseClickedOn = true;
				}
				else if (vHit.collider.tag == "blue base")
				{
					Debug.Log ("BLUE BASE.");
				}
			
			}
		}
	}
}
