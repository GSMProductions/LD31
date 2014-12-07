using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipBehavior : MonoBehaviour {

    public float rotationSpeed = 5.0f;
    public bool onRotation = false;
    public float targetRotation;
    public float rotationSens = 1.0f;
    public float rotationTolerance = 2f;

    public RoomBehavior[,] ship = new RoomBehavior[3,3];

    public HeroBehavior hero;

    public enum Sens {
        clockwise,
        antiClockwise
    }

    // Use this for initialization
    void Start () {
    }
    
    // Update is called once per frame
    void Update () {
        Rotate();
    }

    private void Rotate() {
        if (onRotation && !OnDropping()) {
            if (transform.rotation.eulerAngles.z < targetRotation + rotationTolerance && transform.rotation.eulerAngles.z > targetRotation - rotationTolerance) {
                transform.eulerAngles = new Vector3(0f, 0f, targetRotation);
                onRotation = false;

                Sens sens = Sens.clockwise;
                if (rotationSens < 0) {
                    sens =  Sens.antiClockwise;
                }
                for(int x = 0; x < 3; x++) {
                    for(int y = 0; y < 3; y++) {
                        ship[x,y].RotationEffect(sens);
                    }
                }
                hero.RotationDesactivate();
                UpdateShip();
                Drop();
            }
            else {

                transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed * rotationSens);
            }
        }
    }

    public void UpdateShip() {
            List<RoomBehavior> collect = new List<RoomBehavior>();
            bool placed = false;
            for (int x = 0; x < 3; x++) {
                for (int y = 0; y < 3; y++) {
                    float poidShip = ship[x,y].gameObject.transform.position.x * SystemManager.ROOM_PIXEL_SIZE + ship[x,y].gameObject.transform.position.y;
                    for (int index = 0; index < collect.Count && !placed; index++) {
                        float poidCollect =  collect[index].gameObject.transform.position.x * SystemManager.ROOM_PIXEL_SIZE + collect[index].gameObject.transform.position.y;
                        if( poidShip < poidCollect) {
                            collect.Insert(index,ship[x,y]);
                            placed = true;
                        }
                    }
                    if (!placed) {
                        collect.Add(ship[x,y]);
                    }
                    placed = false;
                }
            }

            for (int index = 0; index < collect.Count && !placed; index++) {
                ship[index/3,index%3] = collect[index];
            }
        }

    public void GiveRotation(float rotation) {
        if(onRotation) {
            return;
        }
        hero.RotationActivate();
        targetRotation = transform.rotation.eulerAngles.z + rotation;
        onRotation =  true;
        if (rotation < 0f) {
            rotationSens = -1.0f;
        }
        else {
            rotationSens = 1.0f;
        }

        if (targetRotation < 0f) {
            targetRotation += 360f;
            
        }
    }

    public void Drop() {
        if (!OnDropping()){

            int x = Random.Range(0,3);

            while(ship[x,0].heroIsHere) {
                x = Random.Range(0,3);
            }

            float roomUnitySize = (float)SystemManager.ROOM_PIXEL_SIZE/(float)SystemManager.PIXEL_PER_UNIT;

            ship[x,0].GiveDropTarget(roomUnitySize*2);
            ship[x,0] = ship [x,1];
            ship[x,0].GiveDropTarget(roomUnitySize);
            
            Vector3 newPosition = ship[x,2].transform.position;

            ship[x,1] = ship [x,2];
            ship[x,1].GiveDropTarget(roomUnitySize);

            GameObject newRoom = GenerateRoom();
            newRoom.transform.parent = transform;

            newPosition.y += roomUnitySize*2;
            newRoom.transform.position = newPosition;
            ship[x,2] =  newRoom.GetComponent<RoomBehavior>();
            ship[x,2].GiveDropTarget(roomUnitySize*2);
        }
    }

    public static GameObject GenerateRoom() {
        string name = "room4";
        return Instantiate(Resources.Load(name, typeof(GameObject))) as GameObject;
    }

    public bool OnDropping()
        {
            bool onDrop = false;
            for (int x = 0; x < 3 ; x++) {
                for (int y = 0; y < 3 ; y++) {
                    if (ship[x,y].onDrop) {
                        onDrop = true;
                    }
                }
            }
            return onDrop;
        }

    public void FindPath(int x1, int y1, int x2, int y2) {
        RoomBehavior [,] map = new RoomBehavior[3,3];
        for (int x = 0; x < 3 ; x++) {
            for (int y = 0; y < 3 ; y++) {
                int val = System.Math.Abs(x-x1) + System.Math.Abs(y-y1);
                if (ship[x,y].monsterIsHere) {
                    val += 10;
                }
                map = val;
            }
         }
    }

}
