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

    public int senseLimit = 40;
    private int targetSightedCountdown;
    private bool battleStartUp = false;
    public bool inBattle = false;
    private GameObject thePlayer;

    [SerializeField]
    HealthBar myHealthBar;


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
    bool newChoiceReady;
    bool travelling;
    bool leaping;
    Vector3 startPosition;
    Vector3 destination;
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
		destinationReachedThreshold = (float)0.05f;
        journeyLength = Vector3.Distance(startPosition, destination);
        senseArea = this.GetComponent<BoxCollider>();
        myBody = this.GetComponent<Rigidbody>();
        targetSightedCountdown = senseLimit;
        myAnimator = this.GetComponent<Animator>();
        destination = this.transform.position;
        _battleManager = FindObjectOfType<BattleManager>();
        myHealthBar.setUpBar((int)hp);
        myHealthBar.hideMe();

    }

    private void Update()
    {
       
        
    }
    private void FixedUpdate()
    {

        if (travelling)
        {
            distCovered = (Time.time - journeyStartTime) * moveSpeed;
            fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, destination, fractionOfJourney);

            destinationProximity = Mathf.Abs(transform.position.x - destination.x) + Mathf.Abs(transform.position.y - destination.y) + Mathf.Abs(transform.position.y - destination.y);

            if (destinationProximity < destinationReachedThreshold)
            {
                reachDestination();

            }
        }

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
                    

                    myAnimator.SetTrigger("Detect");
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
        //Debug.Log("Feild entered");
        if (collision.gameObject.tag == "Player")
        {
            //print("Player Hit");
            playerInRange = true;
            thePlayer = GameObject.FindWithTag("Player");

        }
    }
    private void OnTriggerExit(Collider collision)
    {
        //Debug.Log("Feild exit");
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
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }








    public void moveToLocation(Vector3 where, float speedAug = 1){
        //check to look the right way
        alterFacing(where);
        moveSpeed = speedAug * moveSpeed;

        startPosition = transform.position;
        destination = where;
        journeyLength = Vector3.Distance(startPosition, destination);
        // trigger correct animation
        if (leaping)
        {
            Debug.Log("Leaping");
            myAnimator.SetTrigger("Leap");
        }
        else if(inBattle){
            myAnimator.SetTrigger("BattleRun");
            beginMoving();
        }
        else
        {
            myAnimator.SetTrigger("Walk");
            beginMoving();

        }
        // lerp to destination



    }
    public void beginMoving() {
        journeyStartTime = Time.time;
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
            myAnimator.ResetTrigger("Detect");
        }
        else {
            myAnimator.SetTrigger("Rest");
        }
    }

    public void detectAnimation() {
        if (thePlayer == null) { 
            thePlayer = GameObject.FindWithTag("Player");
        }
        alterFacing(thePlayer.transform.position);
        myAnimator.SetTrigger("Detect");
    }
    public void PrepareForBattle(Vector3 battleStartPosition) {
		Debug.Log("Preparing for battle.");
        senseArea.enabled = false;
        leaping = true;
        battleStartUp = true;
        myHealthBar.showMe();
        moveToLocation(battleStartPosition,5f);

    }
    public void startBattleAI() {
        Debug.Log("starting battle AI");
        myAnimator.SetTrigger("BattleIdle");
    }
    public void takeDamage(float rawDamage) {
        float DamgeDealt = rawDamage - (rawDamage * def);
        hp -= DamgeDealt;
        myHealthBar.setHealth((int)hp);
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
