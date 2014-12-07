using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreenBehaviour : MonoBehaviour {

    public float timer = 2.0f;
    public float fadeout_time = 1.0f;

    public GameObject start_button;

    public float current_timer = 0f;

	// Use this for initialization
	void Start () {
	    start_button.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	    current_timer += Time.deltaTime;
        if (current_timer > timer && current_timer < timer+fadeout_time) {
            GetComponent<Image>().color = new Color(1f,1f,1f, 1f-(current_timer-timer)/(fadeout_time));
        } else if (current_timer > timer+fadeout_time) {
            start_button.SetActive(true);
            gameObject.SetActive(false);
        }

	}

    public void SkipSplash()
    {
        if (current_timer < timer)
            current_timer = timer;
    }
}
