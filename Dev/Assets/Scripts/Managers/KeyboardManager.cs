using UnityEngine;
using System.Collections;

public class KeyboardManager : MonoBehaviour {

    const float LEFTANGLE = -90f;
    const float RIGHTANGLE = 90f;

    const string SHIP =  "Rooms";

    public ShipBehavior shipBehavior;

    // Use this for initialization
    void Start () {
        shipBehavior = GameObject.Find(SHIP).GetComponent<ShipBehavior>();
    }
    
    // Update is called once per frame
    void Update () {
    if (!shipBehavior.OnDropping()) {
        if(Input.GetKeyDown(KeyCode.RightArrow))
            shipBehavior.GiveRotation(RIGHTANGLE);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            shipBehavior.GiveRotation(LEFTANGLE);
        } else if (Input.GetKeyUp(KeyCode.Escape)) {
            Application.LoadLevel (Application.loadedLevelName);
        }
    }


}
