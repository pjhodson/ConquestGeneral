using UnityEngine;
using System.Collections;

public class musicHandler : MonoBehaviour {
	
	public AudioClip[] songs;
	
	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		DontDestroyOnLoad(transform.root.gameObject);
		audio.clip = songs[0];
		audio.Play ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!audio.isPlaying)
		{
			audio.clip = songs[Random.Range (0,songs.Length - 1)];
			audio.Play();
		}
		
		this.transform.position = Camera.main.transform.position;
		
	
	}
}
