using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    public List<GameObject> enemiesInBattle = new List<GameObject>();
    public int enemiesReady = 0;
    private Vector3 neutralCords = new Vector3(-1f, -10f, -1f);
    private Vector3 neutralSize = new Vector3(.1f, .1f, .1f);

    private bool battleCommenced = false;
    private BoxCollider BattleArea;
    public playerController3d _player;
    public GameObject playerBody;
    



    //opt vars
    float optFloat;
    

    // Start is called before the first frame update
    void Start()
    {
        BattleArea = this.GetComponent<BoxCollider>();
        
        BattleArea.size = neutralSize;
        this.transform.position = neutralCords;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesInBattle.Count <=0 && battleCommenced) {
            //EndBattle();
        }
    }



    private void OnTriggerEnter(Collider collision)
    {
        print("colider searching");
        if (collision.gameObject.tag == "Enemy")
        {

            print("enemy found");
            if (!collision.gameObject.GetComponent<EnemyBasic>().inBattle) {
                enemiesInBattle.Add(collision.gameObject);
                collision.gameObject.GetComponent<EnemyBasic>().inBattle = true;
            }

        }
    }

    public void SetUpBattle(GameObject initEnemy) {

        Debug.Log("moving battle area Size: " + BattleArea.size + " | center: " + BattleArea.center);
        BattleArea.size = new Vector3(20f, 2f, 30f);
        this.transform.position = initEnemy.transform.position;
        Debug.Log("battle area fixed Size: " + BattleArea.size + " | center: " + BattleArea.center);

        _player.preapareForBattle();

        StartCoroutine(PositionCombatants());

        

    }

    private IEnumerator PositionCombatants() {

        yield return new WaitForSeconds(.5f);

        Vector3 BattleCenter = this.transform.position;
        float xPosModifer;
        if (BattleCenter.x > playerBody.transform.position.x) { //enemies to left
            xPosModifer = -1f;
        } else {
            xPosModifer = 1f;
        
        }
        Vector3 CombatantPos = new Vector3(BattleCenter.x + (6f * xPosModifer), 0, BattleCenter.z);
        playerBody.transform.position = CombatantPos;
        for (int i=0; i < enemiesInBattle.Count; i++) {
            float xAdjust = 0f;
            float zAdjust = 0f;

            switch (i) {
                case 1:
                    xAdjust = 2f;
                    
                    break;
                case 2:
                    xAdjust = 4f;
                    

                    break;
                case 3:
                    xAdjust = 6f;
                    

                    break;
                case 4:
                    xAdjust = 8f;
                    

                    break;
                case 5:
                    xAdjust = 10f;
                   

                    break;
                case 6:
                    xAdjust = 12f;
                    

                    break;
                case 7:
                    xAdjust = 14f;
                    

                    break;
                case 8:
                    xAdjust = 16f;
                    

                    break;
                case 9:
                    xAdjust = 18f;
                    

                    break;
            }

            CombatantPos = new Vector3(BattleCenter.x + ((3f + xAdjust) * xPosModifer * -1), 1.5f, BattleCenter.z + zAdjust);
            enemiesInBattle[i].GetComponent<EnemyBasic>().PrepareForBattle(CombatantPos);

        }



    }
    public void StartBattle()
    {
        if (enemiesReady >= enemiesInBattle.Count) {
            print("Battle Started!! with " + enemiesInBattle.Count + " enemies");

            //loop through enemies and make them start making fight decisions
            battleCommenced = true;
        }
        

    }

    public void EndBattle() {
        Debug.Log("Leaving battle");
        battleCommenced = false;
        BattleArea.size = neutralSize;
        BattleArea.center = neutralCords;
        _player.leaveBattle();
        



    }
     public void calculateBasicPlayerAttack(float rangeMin, float rangeMax, float damageAug, bool facingLeft)
    {
        print("Battle manager handling attack");
        bool playerFacingLeft = facingLeft;
        print("Battle manager thinks player facing left is " + playerFacingLeft);
        print("Checking " + enemiesInBattle.Count + "enemies for range");
        for (int i = 0; i < enemiesInBattle.Count;i++ )
        {
            if (playerFacingLeft && enemiesInBattle[i].transform.position.x < playerBody.transform.position.x) {// facing left and enemy too the left 
                print("Facing left i found an enemy");
                optFloat = Vector3.Distance(playerBody.transform.position, enemiesInBattle[i].transform.position);

                print("Enemy proximity is " + optFloat);

                print("Attack Xrange is between " + rangeMin + "-" + rangeMax);
                if (optFloat < rangeMax && optFloat > rangeMin)
                {
                    print("the enemy was in range");
                    enemiesInBattle[i].GetComponent<EnemyBasic>().takeDamage(damageAug);
                }

            } else if (!playerFacingLeft && enemiesInBattle[i].transform.position.x > playerBody.transform.position.x)
            {// facing right and enemy too the right 
                print("Facing right i found an enemy");
                optFloat = Vector3.Distance(playerBody.transform.position, enemiesInBattle[i].transform.position);

                print("Enemy proximity is " + optFloat);

                print("Attack Xrange is between " + rangeMin + "-" + rangeMax);
                if (optFloat < rangeMax && optFloat > rangeMin)
                {
                    print("the enemy was in range");
                    enemiesInBattle[i].GetComponent<EnemyBasic>().takeDamage(damageAug);
                }
            }




        }

    }

}
