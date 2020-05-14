using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{

    private BoxCollider senseArea;
    private Rigidbody myBody;
    private bool playerInRange = false;
    private int myFacing = 1; // 0: East, 1: West;
    private Animator myAnimator;
    public BattleManager _battleManager;

    public int senseLimit = 120;
    private int targetSightedCountdown;
    private bool battleStartUp = false;
    public bool inBattle = false;
    private GameObject thePlayer;



    //Enemy STATS
    [SerializeField]
    float hp = 40f;
    [SerializeField]
    float def = .2f;
    [SerializeField]
    float attack;
    [SerializeField]
    float speed = 1f;


    //Enemy AI
    bool travelling;
    bool leaping;
    Vector3 startPosition;
    Vector3 destination;
    [SerializeField]
    float destinationReachedThreshold;
    float destinationProximity;
    private float journeyStartTime;
    private float journeyLength;
    float distCovered;
    float fractionOfJourney;
    float moveSpeed;




    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = speed;
        journeyLength = Vector3.Distance(startPosition, destination);
        senseArea = this.GetComponent<BoxCollider>();
        myBody = this.GetComponent<Rigidbody>();
        targetSightedCountdown = senseLimit;
        myAnimator = this.GetComponent<Animator>();
        destination = this.transform.position;
    }

    private void Update()
    {
        if (travelling) {
            distCovered = (Time.time - journeyStartTime) * moveSpeed;
            fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, destination, fractionOfJourney);

            destinationProximity = Mathf.Abs(transform.position.x - destination.x) + Mathf.Abs(transform.position.y - destination.y) + Mathf.Abs(transform.position.y - destination.y);

            if (destinationProximity < destinationReachedThreshold)
            {
                reachDestination();

            }
        }
        
    }
    private void FixedUpdate()
    {
        


        if (!inBattle) {
            if (playerInRange)
            {
                if (targetSightedCountdown > 0)
                {
                    targetSightedCountdown--;
                    if (myFacing == 0 && thePlayer.transform.position.x > this.transform.position.x || myFacing == 1 && thePlayer.transform.position.x < this.transform.position.x)
                    {
                        targetSightedCountdown--;
                    }
                }
                else
                {
                    
                    inBattle = true;
                    _battleManager.SetUpBattle(this.gameObject);
                }


            }
            else if (targetSightedCountdown < senseLimit)
            {
                targetSightedCountdown++;
            }
        }



    }


    // Update is called once per frame
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Feild entered");
        if (collision.gameObject.tag == "Player")
        {
            //print("Player Hit");
            playerInRange = true;
            thePlayer = GameObject.FindWithTag("Player");

        }
    }
    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("Feild exit");
        if (collision.gameObject.tag == "Player")
        {
            //print("Player Left");
            playerInRange = false;
            thePlayer = null;
        }
    }

    public void alterFacing(Vector3 LookToPoint) {
        if (this.transform.position.x > LookToPoint.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void moveToLocation(Vector3 where, float speedAug = 1){
        //check to look the right way
        alterFacing(where);
        // trigger correct animation
        if (leaping)
        {
            myAnimator.SetTrigger("Leap");
        }
        else if(inBattle){
            myAnimator.SetTrigger("BattleRun");
        }
        else
        {
            myAnimator.SetTrigger("Walk");
        }
        // lerp to destination
        journeyStartTime = Time.time;
        startPosition = transform.position;
        destination = where;
        moveSpeed = speedAug * moveSpeed;
        travelling = true;

    }

    public void reachDestination() {
        moveSpeed = speed;
        travelling = false;
        if (leaping) {
            myAnimator.SetTrigger("Land");
            leaping = false;
            if (battleStartUp) {
                _battleManager.enemiesReady++;
                _battleManager.StartBattle();
                battleStartUp = false;
            }
                
        }
        else if (inBattle)
        {
            myAnimator.SetTrigger("BattleIdle");
        }
        else {
            myAnimator.SetTrigger("Rest");
        }
    }
    public void PrepareForBattle(Vector3 battleStartPosition) {
        senseArea.enabled = false;
        leaping = true;
        moveToLocation(battleStartPosition,3);

    }
    public void takeDamage(float rawDamage) {
        float DamgeDealt = rawDamage - (rawDamage * def);
        hp -= DamgeDealt;
        myAnimator.SetTrigger("TakeDamage");

        if (hp <=0) {
            getKilled();
        }
    }

    private void getKilled() {
        print("I died!");
        myAnimator.SetTrigger("Die");
        
    }
    public void RemoveMe() {
        if(_battleManager != null)
            _battleManager.enemiesInBattle.Remove(gameObject);

        Destroy(gameObject);
    }
}
