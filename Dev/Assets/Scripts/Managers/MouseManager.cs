using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

    private bool is_dragging = false;
    public float drag_length = 20f;
    public ShipBehavior ship;
    private Vector3 mousePosition_initial;

    const float LEFTANGLE = -90f;
    const float RIGHTANGLE = 90f;

    const string SHIP =  "Rooms";

	// Use this for initialization
	void Start () {
	    ship = GameObject.Find(SHIP).GetComponent<ShipBehavior>();
	}
	
    public void RoomClicked(RoomBehavior room) {
        if (enabled)
            ship.hero.roomClicked = room;
    }

	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0) && !is_dragging) {
            is_dragging = true;
            mousePosition_initial = Input.mousePosition;
        } else if (!Input.GetMouseButtonUp(0) && is_dragging) {
            if (ship.OnDropping()) return;
            Vector3 mouse_movement = Input.mousePosition - mousePosition_initial;
            if (Mathf.Abs(mouse_movement.x) > Mathf.Abs(mouse_movement.y)) {
                if (mouse_movement.x < -drag_length) {
                    if (Input.mousePosition.y > Screen.height/2f) {
                        ship.GiveRotation(RIGHTANGLE);
                    } else {
                        ship.GiveRotation(LEFTANGLE);
                    }
                    is_dragging = false;
                } else if (mouse_movement.x > drag_length) {
                    if (Input.mousePosition.y > Screen.height/2f) {
                        ship.GiveRotation(LEFTANGLE);
                    } else {
                        ship.GiveRotation(RIGHTANGLE);
                    }
                    is_dragging = false;
                }
            } else {
                if (mouse_movement.y < -drag_length) {
                    if (Input.mousePosition.x > Screen.width/2f) {
                        ship.GiveRotation(LEFTANGLE);
                    } else {
                        ship.GiveRotation(RIGHTANGLE);
                    }
                    is_dragging = false;
                } else if (mouse_movement.y > drag_length) {
                    if (Input.mousePosition.x > Screen.width/2f) {
                        ship.GiveRotation(RIGHTANGLE);
                    } else {
                        ship.GiveRotation(LEFTANGLE);
                    }
                    is_dragging = false;
                }                
            }
        } else if (Input.GetMouseButtonUp(0) && is_dragging) {
            is_dragging = false;
        }
	}
}
