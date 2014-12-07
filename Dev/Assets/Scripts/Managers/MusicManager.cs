using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioSource[] bgm_tracks;
    private int currentMusic = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetMusic(int index) {
        bgm_tracks[currentMusic].Stop();
        bgm_tracks[index].Play();
        currentMusic = index;
    }
}
