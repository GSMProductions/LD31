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
                collect[index].x = index/3;
                collect[index].y = index%3;
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
            newRoom.GetComponent<RoomBehavior>().ship = this;
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
        string[] name = new string[12] {"roomS", 
                                       "roomES",
                                       "roomES",
                                       "roomEW",
                                       "roomEW",
                                       "roomESW",
                                       "roomESW",
                                       "roomESW",
                                       "roomNESW",
                                       "roomNESW",
                                       "roomNESW",
                                       "roomNESW",
                                   };
        int rotations = Random.Range(0,4);
        GameObject room = Instantiate(Resources.Load(name[Random.Range(0,name.Length)], typeof(GameObject))) as GameObject;
        for (int r = 0; r < rotations; r++) {
            room.GetComponent<RoomBehavior>().RotationEffect(Sens.clockwise);
        }
        room.transform.eulerAngles = new Vector3(0f, 0f, -90*rotations);
        return room;
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

    public List<RoomBehavior> FindPath(int x1, int y1, int x2, int y2) {
        
        List<RoomBehavior> path =  new List<RoomBehavior>();

        int [,] map = new int[3,3];
        for (int mx = 0; mx < 3 ; mx++) {
            for (int my = 0; my < 3 ; my++) {
                map [mx,my] = -1;
            }
        }

        List<int[]> roomList = new List<int []>();
        List<int[]> roomStore = new List<int []>();
        bool found = false;
        int [] first = new int[2];

        first[0] = x1;
        first[1] = y1; 

        roomStore.Add(first);
            int cpt = 0;
        while(!found && roomStore.Count > 0) {
            roomList = roomStore;
            roomStore = new List<int []>();
            cpt++;

            foreach(int [] current in roomList) {
                int cx = current[0];
                int cy = current[1];
                for(int sens=0; sens < 4 && !found; sens++) {
                    if(ship[cx,cy].opening[sens]) {
                        int opposite = (sens + 2) % 4;
                        int dx = 0 ,dy = 0;
                        switch(sens) {
                            case 0 : dx = 1; break;
                            case 1 : dy = 1; break;
                            case 2 : dx = -1; break;
                            case 3 : dy = -1; break;
                        }
                        int [] other = new int[2];
                        int ox = cx + dx;
                        int oy = cy + dy;
                        other[0] = ox;
                        other[1] = oy;
                        if(ox >= 0 && oy >= 0 && ox <= 2 && oy <= 2) {
                            if(map[ox,oy] == -1) {
                                if(ship[ox,oy].opening[opposite]) {
                                    int dMan;
                                    if(ship[ox,oy].monsterIsHere) {
                                        dMan = 10;
                                    }
                                    else {
                                        dMan = System.Math.Abs(ox-x2) + System.Math.Abs(oy-y2);
                                    }
                                    map[ox,oy] = dMan;
                                    if(ox == x2 && oy == y2) {
                                        found =  true;
                                    }
                                    roomStore.Add(other);
                                }
                            }
                        }
                    }
                }
            }
        }

        return path;
    }
}
