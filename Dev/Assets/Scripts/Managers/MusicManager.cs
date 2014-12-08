using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioSource[] bgm_tracks;
    private int currentMusic = 0;

    public AudioSource[] sfx;

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

    public void PlaySound(int index) {
        sfx[index].Play();
    }

    public void StopSound(int index) {
        sfx[index].Stop();
    }
}
