using UnityEngine;
using System.Collections;

public class attributes : MonoBehaviour {
	
	public int hp;
	public int dmgDie;
	
	public string unitName;
	
	public Texture unitImage;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public int rollDamage()
	{
		return Random.Range (1, dmgDie);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
