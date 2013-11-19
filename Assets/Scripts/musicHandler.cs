using UnityEngine;
using System.Collections;

public class musicHandler : MonoBehaviour {
	
	public AudioClip[] songs;
	
	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!audio.isPlaying)
		{
			audio.clip = songs[Random.Range (0,songs.Length - 1)];
			audio.Play();
		}
	
	}
}
