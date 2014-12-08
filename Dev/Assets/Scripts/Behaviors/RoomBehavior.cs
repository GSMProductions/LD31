using UnityEngine;
using System.Collections;


public class RoomBehavior : MonoBehaviour {

    const string PICTURE_PREFIX = "dsg_";
    const string GAMEOBJECT_SPRITE = "dsg_room";
    const string NAME_PREFIX = "room";
    const int NUMBER_OF_TYPE = 3;

    const float Y_LIMIT = -35.00f;

    public GameObject go_sprite;
    public string code;
    public float dropSpeed = 2f;

    public bool onDrop = false;
    public float yTarget;
    public float dropTolerance = 0.2f;

    public bool heroIsHere = false;
    public bool monsterIsHere = false;

    public HeroBehavior hero = null;
    public MonsterBehavior monster = null;

    public bool [] opening = new bool[4];

    public bool dying = false;
    public float dying_rotation_speed = 25f;
    public float dying_rotation_direction = 1.0f;
    public bool arriving = false;
    public float arriving_speed = 20f;
    public float initial_drop_speed = 2f;

    public int x;
    public int y;

    public ShipBehavior ship;
    public PickupBehavior pickup;

    // Use this for initialization
    public void Start () {
        
        go_sprite = transform.Find(GAMEOBJECT_SPRITE).gameObject;
        int typeNumber = Random.Range(1,NUMBER_OF_TYPE + 1);
        string name = NAME_PREFIX + code + '_' + typeNumber;
        string resourceName = PICTURE_PREFIX + name;

        //load sprite
        Texture2D texture = Resources.Load(resourceName) as Texture2D;
        float width = texture.width;
        float height = texture.height;

        Rect rect = new Rect(0f,0f,width, height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);

        Sprite newSprite = Sprite.Create(texture, rect, pivot,SystemManager.PIXEL_PER_UNIT);
        go_sprite.GetComponent<SpriteRenderer>().sprite = newSprite;

        AddPickup();
    }


    public void RotationEffect(ShipBehavior.Sens sens) {
        bool [] saveOpening = new bool[4];
        saveOpening[0] = opening[0];
        saveOpening[1] = opening[1];
        saveOpening[2] = opening[2];
        saveOpening[3] = opening[3];

        switch(sens) {
            case ShipBehavior.Sens.antiClockwise :
                opening[0] = saveOpening[3];
                opening[1] = saveOpening[0];
                opening[2] = saveOpening[1];
                opening[3] = saveOpening[2];
                break;
            case ShipBehavior.Sens.clockwise :
                opening[0] = saveOpening[1];
                opening[1] = saveOpening[2];
                opening[2] = saveOpening[3];
                opening[3] = saveOpening[0];
                break;
        }

    }
    public void Dropping() {
        if (onDrop) {
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;
            
            Vector3 newPosition = new Vector3(x, y - Time.deltaTime * dropSpeed, z);
            transform.position =  newPosition;
            if (transform.position.y > yTarget - dropTolerance && transform.position.y < yTarget + dropTolerance) {
                newPosition = transform.position;
                newPosition.y = yTarget;
                transform.position =  newPosition;
                dropSpeed = initial_drop_speed;
                onDrop = false;
            }
            if(dying) {
                dropSpeed = arriving_speed;
                transform.Rotate(Vector3.forward * Time.deltaTime * dying_rotation_speed * dying_rotation_direction);
            }
            if(transform.position.y <= Y_LIMIT) {
                Destroy(gameObject);
            }
        }
    }

    public void GiveDropTarget(float yPosition) {
        if (onDrop)
            return;
        float y =  transform.position.y;
        yTarget = y - yPosition;
        onDrop = true;

    }

    public void AddHeroOnRoom(HeroBehavior hero) {
        this.hero = hero;
        this.heroIsHere = true;
        this.hero.transform.parent = transform;
        this.hero.room = this;
    }

    public void AddMonsterOnRoom(MonsterBehavior monster) {
        this.monster = monster;
        this.monsterIsHere = true;
        this.monster.transform.parent = transform;
        this.monster.room = this;
    }


    public void MonsterLeaveRoom() {
        this.monster = null;
        this.monsterIsHere = false;
    }

    public void HeroLeaveRoom() {
        this.hero = null;
        this.heroIsHere = false;
    }

    // Update is called once per frame
    public void Update () {
        Dropping();
    }

    public void OnMouseUp() {
        ship.hero.roomClicked = this;
    }

    public void AddPickup() {
        //add pickup
        GameObject go_pickup =  PickupBehavior.GivePickup();
        pickup = go_pickup.GetComponent<PickupBehavior>();
        pickup.PositionOnRoom(transform.position);
        pickup.room = this;
        pickup.transform.parent = transform;
    }

    public void Pick() {
        if( pickup != null) {

            Destroy(pickup.gameObject);
            pickup = null;
            hero.counter++;
        }
    }

}
