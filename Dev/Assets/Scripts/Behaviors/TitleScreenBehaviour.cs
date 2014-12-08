using UnityEngine;
using System.Collections;

public class TitleScreenBehaviour : MonoBehaviour {

    public bool sliding = false;

    public float target_y = 0f;
    public float initial_y = 0f;
    private float animation_timer = 0f;
    private float direction = 1.0f;

    private KeyboardManager system;
    private MouseManager system_mouse;
    public bool disable_input_on_start = false;
	// Use this for initialization
	void Start () {
	    system = GameObject.Find("System").GetComponent<KeyboardManager>();
        system_mouse = GameObject.Find("System").GetComponent<MouseManager>();

        if (disable_input_on_start) {
            system.enabled = false;
            system_mouse.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
        if (transform.position.y != target_y) {
            if (target_y > 10) {
                transform.position += new Vector3(0f, (target_y-initial_y)*Time.deltaTime/2f*animation_timer*animation_timer, 0f);
            } else {
                transform.position += new Vector3(0f, (target_y-transform.position.y)*Time.deltaTime/2f*animation_timer*animation_timer, 0f);
            }
            animation_timer += direction*Time.deltaTime;
        }
        if (Mathf.Abs(transform.position.y - target_y) < 1 || Mathf.Abs(transform.position.y) > 2000) {
            transform.position = new Vector3(transform.position.x, target_y, transform.position.z);
            sliding = false;
        }
	}

    public void SlideOut() {
        if (sliding) {
            return;
        }

        initial_y = 0f;
        target_y = 2000f;
        animation_timer = 0f;
        direction = 1f;
        sliding = true;
        system.enabled = true;
        system_mouse.enabled = true;
    }

    public void SlideIn() {
        if (sliding) {
            return;
        }

        initial_y = -2000f;
        target_y = 0f;
        animation_timer = 2f;
        direction = 1f;
        sliding = true;
        transform.position = new Vector3(transform.position.x, initial_y, transform.position.z);
        system.enabled = false;
        system_mouse.enabled = false;
    }
}
