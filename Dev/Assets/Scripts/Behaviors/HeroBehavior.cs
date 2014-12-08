using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroBehavior : CharacterBehavior {

    // Use this for initialization
    const string PREFAB_HERO_NAME = "hero";

    public RoomBehavior roomClicked = null;

    public int counter = 0;

    void Start () {
    }

    public override void Update () {
        base.Update();
        if(!onRotation) {
            if(roomClicked != null && targetRoom ==  null) {
                int x1 = room.x;
                int y1 = room.y;
                int x2 = roomClicked.x;
                int y2 = roomClicked.y;

                List<RoomBehavior> path = ship.FindPath(x1, y1, x2, y2);
                // Debug.Log(path);
                if (path != null && path.Count > 0) {
                    targetRoom = path[0];
                }
                else {
                    roomClicked = null;
                    //nothing
                    //targetRoom = roomClicked;
                }
            }
        }
    }

    public static GameObject GiveHero() {
        return Instantiate(Resources.Load(PREFAB_HERO_NAME, typeof(GameObject))) as GameObject;
    }

    public override void ChangeRoom() {
        base.ChangeRoom();
        room.HeroLeaveRoom();
        targetRoom.AddHeroOnRoom(this);
        counter ++;
    }

}
