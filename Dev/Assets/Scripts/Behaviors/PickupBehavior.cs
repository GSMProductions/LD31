using UnityEngine;
using System.Collections;

public class PickupBehavior : MonoBehaviour {

    const string PREFAB_PICKUP_NAME = "pickup";

    public bool onRotation =  false;

    public RoomBehavior room;
    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
        if(onRotation) {
            PositionOnRoom(room.transform.position);
        }
    
    }

    public static GameObject GivePickup() {
        return Instantiate(Resources.Load(PREFAB_PICKUP_NAME, typeof(GameObject))) as GameObject;
    }

    public void PositionOnRoom(Vector3 newPosition) {
        newPosition.z =  transform.position.z;
        transform.position = newPosition;
    }

    public void RotationActivate() {
        onRotation = true;
        transform.parent = null;
    }

    public void RotationDesactivate() {
        onRotation = false;
        transform.parent = room.transform;
    }

}
