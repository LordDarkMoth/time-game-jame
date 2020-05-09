using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{

    private BoxCollider senseArea;
    private Rigidbody myBody;
    private bool playerInRange = false;
    private int myFacing = 1; // 0: East, 1: West;
    //public BattleManager _battleManager;

    public int senseLimit = 120;
    private int targetSightedCountdown;
    public bool inBattle = false;
    private GameObject thePlayer;



    //Enemy STATS
    [SerializeField]
    float hp = 40f;
    [SerializeField]
    float def = .2f;
    [SerializeField]
    float attack;



    // Start is called before the first frame update
    void Start()
    {
        senseArea = this.GetComponent<BoxCollider>();
        myBody = this.GetComponent<Rigidbody>();

        targetSightedCountdown = senseLimit;
    }

    private void FixedUpdate()
    {
        if (playerInRange) {
            if (targetSightedCountdown > 0) {
                targetSightedCountdown--;
                if (myFacing == 0 && thePlayer.transform.position.x > this.transform.position.x || myFacing == 1 && thePlayer.transform.position.x < this.transform.position.x) {
                    targetSightedCountdown--;
                }
            } else if(!inBattle)
            {
                //_battleManager.SetUpBattle(this.gameObject);
            }


        } else if (targetSightedCountdown < senseLimit) {
            targetSightedCountdown++;
        }


    }


    // Update is called once per frame
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //print("Player Hit");
            playerInRange = true;
            thePlayer = GameObject.FindWithTag("Player");

        }
    }
    private void OnTriggerExit(Collider collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            //print("Player Left");
            playerInRange = false;
            thePlayer = null;
        }
    }

    public void takeDamage(float rawDamage) {
        float DamgeDealt = rawDamage - (rawDamage * def);
        hp -= DamgeDealt;
        print("took " + DamgeDealt + " damage, now hp at " + hp);
        if (hp <=0) {
            getKilled();
        }
    }

    private void getKilled() {
        print("I died!");
        //_battleManager.enemiesInBattle.Remove(gameObject);
        Destroy(gameObject);
    }
}
