using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {
    
    public RoomBehavior room;
    public RoomBehavior targetRoom;
    public bool onRotation = false;
    public bool onDrop = false;
    public float speed = 10f;
    public ShipBehavior ship;

    public Vector3 deltaRotation = Vector3.zero;
    public float angleInitial = 0f;

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
        deltaRotation.x = transform.position.x - room.transform.position.x;
        deltaRotation.y = transform.position.y - room.transform.position.y;
        angleInitial = ship.transform.rotation.eulerAngles.z;
    }

    public void RotationDesactivate() {
        Vector3 newPosition = room.transform.position;
        newPosition.z =  transform.position.z;
        float angle = -Mathf.PI /180f * (ship.transform.rotation.eulerAngles.z - angleInitial);

        float dx = Mathf.Cos(angle) * deltaRotation.x + Mathf.Sin(angle) * deltaRotation.y;
        float dy = -1 * Mathf.Sin(angle) * deltaRotation.x + Mathf.Cos(angle) * deltaRotation.y;

        newPosition.x += dx;
        newPosition.y += dy;

        onRotation = false;
        transform.parent = room.transform;
        deltaRotation = Vector3.zero;
        angleInitial = 0f;
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
        float angle = -Mathf.PI /180f * (ship.transform.rotation.eulerAngles.z - angleInitial);

        float dx = Mathf.Cos(angle) * deltaRotation.x + Mathf.Sin(angle) * deltaRotation.y;
        float dy = -1 * Mathf.Sin(angle) * deltaRotation.x + Mathf.Cos(angle) * deltaRotation.y;

        newPosition.x += dx;
        newPosition.y += dy;

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
            InRoom();
        }
        else {
            Vector3 newPosition = new Vector3(transform.position.x + dx * Time.deltaTime * speed, transform.position.y + dy * Time.deltaTime * speed, 0f);
            animator.SetFloat("dx", dx);
            animator.SetFloat("dy", dy);
            PositionOnRoom(newPosition);
        }
    }

    public virtual void ChangeRoom() {
        //Debug.Log("Change Room!" + targetRoom.x +  " " + targetRoom.y);
    }

    public virtual void InRoom() {
    }
}
