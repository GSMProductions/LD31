using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroBehavior : CharacterBehavior {

    // Use this for initialization
    const string PREFAB_HERO_NAME = "hero";

    public RoomBehavior roomClicked = null;

    void Start () {
    }

    public override void Update () {
        base.Update();
        if(roomClicked != null) {
            List<RoomBehavior> path = new List<RoomBehavior>();
            if (path.Count > 0 ) {
                targetRoom = path[0];
            }
            else {
                targetRoom = roomClicked;
            }
        }
    }

    public static GameObject GiveHero() {
        return Instantiate(Resources.Load(PREFAB_HERO_NAME, typeof(GameObject))) as GameObject;
    }

}
