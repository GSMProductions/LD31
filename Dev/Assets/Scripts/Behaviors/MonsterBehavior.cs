using UnityEngine;
using System.Collections;

public class MonsterBehavior : CharacterBehavior {
    const string PREFAB_MONSTER_NAME = "monster";

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    public override void Update () {
    
    }

    public static GameObject GiveMonster() {
        return Instantiate(Resources.Load(PREFAB_MONSTER_NAME, typeof(GameObject))) as GameObject;
    }
}
