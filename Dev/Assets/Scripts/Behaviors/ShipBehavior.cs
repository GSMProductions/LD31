using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipBehavior : MonoBehaviour {

    public float rotationSpeed = 5.0f;
    public bool onRotation = false;
    public float targetRotation;
    public float rotationSens = 1.0f;
    public float rotationTolerance = 2f;
    public float rotation_sinus_amplitude = 1f;

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
        float rotation_speed_amplitude;
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
                rotation_speed_amplitude = Mathf.Max(0.1f,Mathf.Sin(Mathf.PI / 180f * rotation_sinus_amplitude *Mathf.Abs(targetRotation-transform.rotation.eulerAngles.z)));
                transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed * rotationSens * rotation_speed_amplitude);
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

            ship[x,0].dying = true;
            ship[x,0].dying_rotation_direction = Random.Range(-1, 2);
            while(ship[x,0].dying_rotation_direction == 0f) {
                ship[x,0].dying_rotation_direction = Random.Range(-1, 2);
            }
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
            ship[x,2].arriving = true;
            ship[x,2].dropSpeed = 20f;
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

    public List<Vector2> FindPath(int x1, int y1, int x2, int y2) {
        int [,] map = new int[3,3];
        for (int mx = 0; mx < 3 ; mx++) {
            for (int my = 0; my < 3 ; my++) {
                map [mx,my] = -1;
            }
        }

        List<Vector2> roomList = new List<Vector2>();
        List<Vector2> roomStore = new List<Vector2>();

        roomList.Add(new Vector2(x1,y1));
        bool found = false;
        while(!found && roomList.Count > 0){
            foreach(Vector2 room in roomList) {
                for (int sens = 0; sens < 4 && !found; sens++) {
                    if (ship[(int)room.x,(int)room.y].opening[sens] && map[(int)room.x,(int)room.y] == -1) {
                        int dx = 0;
                        int dy = 0;
                        switch(sens) {
                            case 0 :
                                dy = 1;
                                break;
                            case 1 :
                                dx = 1;
                                break;
                            case 2 :
                                dy = -1;
                                break;
                            case 3 :
                                dx = -1;
                                break;
                        }
                        int dMan = System.Math.Abs(((int)room.x + dx) - x2) + System.Math.Abs(((int)room.y + dy) - y2);
                        if(ship[(int)room.x,(int)room.y].monsterIsHere) {
                            dMan += 10;
                        }
                        roomStore.Add(new Vector2(room.x + dx , room.y + dy));
                        if((int)room.x + dx == x2 && (int)room.y + dy == y2) {
                            found = true;
                        }
                    }
                }
                roomList = roomStore;
                roomStore =  new List<Vector2>();
            }
        }

        List<Vector2> path = new List<Vector2>();

        if (!found) {
            return null;
        }

        int x = x1;
        int y = y1;

        while(x != x2 || y != y2) {
            int px = -1, py = -1, val = 10;
            for (int sens = 0; sens < 4 && !found; sens++) {
                int dx = 0;
                int dy = 0;
                switch(sens) {
                    case 0 :
                        dy = 1;
                        break;
                    case 1 :
                        dx = 1;
                        break;
                    case 2 :
                        dy = -1;
                        break;
                    case 3 :
                        dx = -1;
                        break;
                }
                if(map [x+dx, y+dy] != -1) {
                    if (val > map[x+dx, y+dy]) {
                        val = map[x+dx, y+dy];
                        px = x+dx;
                        py = y+dy;
                    }
                }
            }
            path.Add(new Vector2((int)px,(int)py));
            x = px;
            y = py;
        }

    return path;
    }
}
