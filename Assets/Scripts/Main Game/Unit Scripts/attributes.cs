using UnityEngine;
using System.Collections;

public class attributes : MonoBehaviour {
	
	public int hp;
	public int dmgDie;
	
	public int dmgMod;
	
	public string unitName;
	
	public Texture unitImage;
	
	// Use this for initialization
	void Start () {
		dmgMod = 0;
	}
	
	public int rollDamage()
	{
		return Random.Range (1, dmgDie) + dmgMod;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
