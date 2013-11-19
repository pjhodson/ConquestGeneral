using UnityEngine;
using System.Collections;

public class musicHandler : MonoBehaviour {
	
	public AudioClip[] songs;
	
	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		DontDestroyOnLoad(this.gameObject);
		audio.clip = songs[0];
		audio.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!audio.isPlaying)
		{
			audio.clip = songs[Random.Range (0,songs.Length - 1)];
			audio.Play();
		}
	
	}
	
	void OnLevelWasLoaded(int level)
	{
		this.transform.parent = Camera.main.transform;
		this.transform.localPosition = new Vector3(0,0,0);
	}
}
