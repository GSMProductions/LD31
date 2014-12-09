using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TranslucentOverlayBehaviour : MonoBehaviour {

    public float fade_time = 1.0f;

    public Image bg;

    public float target_alpha = 0f;
    public float initial_alpha = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
    public void FadeIn() {
        target_alpha = 1f;
        initial_alpha = bg.color.a;
    }

    public void FadeOut() {
        target_alpha = 0f;
        initial_alpha = bg.color.a;
    }

	// Update is called once per frame
	void Update () {
        if (bg.color.a != target_alpha) {
            bg.color = new Color(1f, 1f, 1f, bg.color.a + (target_alpha-initial_alpha)*Time.deltaTime);
        }
	}
}
