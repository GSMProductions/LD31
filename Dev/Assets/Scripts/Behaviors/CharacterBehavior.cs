using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {
    
    public RoomBehavior room;
    public bool onRotation = false;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
        if (onRotation) {
            PositionOnRoom(room.transform.position);
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
}
