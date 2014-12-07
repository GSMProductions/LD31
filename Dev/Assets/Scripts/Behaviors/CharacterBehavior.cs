using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {
    
    public RoomBehavior room;
    public RoomBehavior targetRoom;
    public bool onRotation = false;
    public float speed = 10f;
    public ShipBehavior ship;

    public float moveTolerance = 0.1f;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    public virtual void Update () {
        if (onRotation) {
            PositionOnRoom(room.transform.position);
        }
        else if (targetRoom != null) {
            MoveToRoom();
        }
    
    }

    public void RotationActivate() {
        onRotation = true;
        transform.parent = null;
    }

    public void RotationDesactivate() {
        onRotation = false;
        transform.parent = room.transform;
    }

    public void PositionOnRoom(Vector3 newPosition) {
        newPosition.z =  transform.position.z;
        transform.position = newPosition;
    }

    public void MoveToRoom() {
        float dx = 0f, dy = 0f;
        if(System.Math.Abs(transform.position.x - targetRoom.transform.position.x) >=  moveTolerance) {
            if (transform.position.x > targetRoom.transform.position.x) {
                dx = -1f;
            }
            else {
                dx = 1f;
            }
        }

        if(System.Math.Abs(transform.position.y - targetRoom.transform.position.y) >=  moveTolerance) {
            if (transform.position.y > targetRoom.transform.position.y) {
                dy = -1f;
            }
            else {
                dy = 1f;
            }
        }

        Vector3 newPosition = new Vector3(transform.position.x + dx * Time.deltaTime * speed, transform.position.y + dy * Time.deltaTime * speed, 0f);
        PositionOnRoom(newPosition);
    }

}
