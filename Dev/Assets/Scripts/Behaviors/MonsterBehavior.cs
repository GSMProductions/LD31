using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MonsterBehavior : CharacterBehavior {
    const string PREFAB_MONSTER_NAME = "monster";

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    public override void Update () {
        base.Update();
        if(!onRotation) {
            if(targetRoom ==  null) {
                int x1 = room.x;
                int y1 = room.y;
                int x2 = ship.hero.room.x;
                int y2 = ship.hero.room.y;

                List<RoomBehavior> path = ship.FindPath(x1, y1, x2, y2);
                // Debug.Log(path);
                if (path != null && path.Count > 0) {
                    targetRoom = path[0];
                }
                else {
                    //nothing
                    //targetRoom = roomClicked;
                }
            }
        }
    
    }

    public static GameObject GiveMonster() {
        return Instantiate(Resources.Load(PREFAB_MONSTER_NAME, typeof(GameObject))) as GameObject;
    }

    public override void ChangeRoom() {
        base.ChangeRoom();
        room.MonsterLeaveRoom();
        targetRoom.AddMonsterOnRoom(this);
    }
}
