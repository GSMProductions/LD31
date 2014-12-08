using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {
    
    public RoomBehavior room;
    public RoomBehavior targetRoom;
    public bool onRotation = false;
    public bool onDrop = false;
    public float speed = 10f;
    public ShipBehavior ship;

    public float moveTolerance = 0.1f;

    private Animator animator;

    // Use this for initialization
    void Awake () {
        animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    public virtual void Update () {
        if (onRotation) {
            PositionOnRoom(room.transform.position);
        }
        else if (targetRoom != null && !onDrop) {
            MoveToRoom();
        }

        if(targetRoom != null) {
            if(Vector3.Distance(transform.position, room.transform.position) > Vector3.Distance(transform.position, targetRoom.transform.position)) {
                ChangeRoom();
            }
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

    public void DropActivate() {
        onDrop = true;
        if (room != targetRoom) {
            targetRoom = null;
        }
    }

    public void DropDesactivate() {
        onDrop = false;
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

        if(dx == 0f && dy == 0f) {
            targetRoom = null;
        }
        else {
            Vector3 newPosition = new Vector3(transform.position.x + dx * Time.deltaTime * speed, transform.position.y + dy * Time.deltaTime * speed, 0f);
            animator.SetFloat("dx", dx);
            animator.SetFloat("dy", dy);
            PositionOnRoom(newPosition);
        }
    }

    public virtual void ChangeRoom() {
        Debug.Log("Change Room!" + targetRoom.x +  " " + targetRoom.y);
    }

}
