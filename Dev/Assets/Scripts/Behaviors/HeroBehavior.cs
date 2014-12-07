using UnityEngine;
using System.Collections;

public class HeroBehavior : CharacterBehavior {

    // Use this for initialization
    const string PREFAB_HERO_NAME = "hero";

    void Start () {
    
    }

    public static GameObject GiveHero() {
        return Instantiate(Resources.Load(PREFAB_HERO_NAME, typeof(GameObject))) as GameObject;
    }

}
