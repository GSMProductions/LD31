using UnityEngine;
using System.Collections;

public class SystemManager : MonoBehaviour {

    public const int PIXEL_PER_UNIT = 20;
    public const int ROOM_PIXEL_SIZE = 256;

    const string GAMEOBJECT_ROOMS = "/Rooms";

    // Use this for initialization
    public GameObject go_rooms;
    public const int NumberOfRoomsBySide = 3;

    void Start () {
    
        go_rooms = GameObject.Find(GAMEOBJECT_ROOMS);
        LoadRooms();
    }
    

    public void LoadRooms() {
        float roomUnitySize = (float)ROOM_PIXEL_SIZE/(float)PIXEL_PER_UNIT;

        for(float index_x = 0; index_x < NumberOfRoomsBySide; index_x++) {
            for(float index_y = 0; index_y < NumberOfRoomsBySide; index_y++) {
                GameObject newRoom = ShipBehavior.GenerateRoom();
                newRoom.transform.parent = go_rooms.transform;
                newRoom.transform.position = new Vector3((index_x -1) * roomUnitySize, (index_y - 1) * roomUnitySize, 0f);
                go_rooms.GetComponent<ShipBehavior>().ship[(int)index_x,(int)index_y] = newRoom.GetComponent<RoomBehavior>();
            }
        }
        GameObject hero = HeroBehavior.GiveHero();
        go_rooms.GetComponent<ShipBehavior>().ship[0,0].AddHeroOnRoom(hero.GetComponent<HeroBehavior>());
        go_rooms.GetComponent<ShipBehavior>().hero = hero.GetComponent<HeroBehavior>();

        hero.GetComponent<HeroBehavior>().PositionOnRoom(go_rooms.GetComponent<ShipBehavior>().ship[0,0].transform.position);
    }

    
    // Update is called once per frame
    void Update () {
    
    }
}
