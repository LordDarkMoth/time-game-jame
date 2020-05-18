using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController3d : MonoBehaviour
{

    private Rigidbody body;
    float moveHorizontal;
    float moveVertical;
    public Animator playerAnimator;
    public bool _facingLeft = true;
    public bool inCombat = false;
    public GameObject visuals;
    private SpriteRenderer charRenderer;
    public Camera playerCamera;
    //public Camera MapCamera;
    public Camera BattleCamera;
    public Transform attackPoint;
    public float attackRange = .5f;
    public LayerMask enemyLayer;

    //private Vector3 cameraRestPosition;

    private bool inMenu = false;


    //combat Tracking
    [SerializeField]
    private int[] basicAttacksDmg;
    [SerializeField]
    private int[][] basicAttacksOrgin;
    [SerializeField]
    private int[] basicAttacksRange;

    private int[] superMoveValues = new int[4];
    private int currentSuperSlot = 0;

    [SerializeField]
    HealthBar playerHealthBar;


    //public VillagerChar targetVillager;

    public int speed;
    public float leapDistance;
    public float LeapAniTime;
    public int playerMaxStamina = 6;
    public int playerStamina;
    [SerializeField]
    int maxHealth = 100;
    int currentHealth = 100;

    private float t;
    Vector3 startPosition;
    Vector3 target;
    float incrementMoveTime;

    public bool moveable = true;
    private Vector3 endpoint;

    EnemyBasic curEnemy;


    private void Awake()
    {
        playerStamina = playerMaxStamina;

    }
    // Start is called before the first frame update
    void Start()
    {
        body = this.GetComponent<Rigidbody>();
        playerAnimator = visuals.GetComponent<Animator>();
        charRenderer = visuals.GetComponent<SpriteRenderer>();
        startPosition = target = transform.position;
        showPlayer();

        playerHealthBar.setUpBar(maxHealth);
       

        //cameraRestPosition = playerCamera.transform.position;


    }

    private void Update()
    {
        if (moveable)
        {
            
            //Buttons
            if (Input.GetButtonUp("A"))
            {
                //print("hit A, combat: " + inCombat);

                if (!inCombat)
                {
                    //passive
                    //interact
                    //if (targetVillager != null)
                    //{
                    //    targetVillager.InteractWithVillager();
                    //}
                }
                else
                {
                    //combat
                    //dogde/dash



                }
            }
            if (Input.GetButtonUp("B"))
            {
                

                if (!inCombat)
                {
                    //print("hit B, combat: " + inCombat);
                    //passive 
                    //cancel
                }
               
            }
            if (Input.GetButtonUp("X"))
            {
                //print("hit X, combat: " + inCombat);

                if (!inCombat)
                {
                    //passive
                    // Stats
                    //if (inMenu)
                    //{
                    //showPlayer();
                    //inMenu = false;
                    //}
                    //else
                    //{
                    //showMap();
                    //inMenu = true;
                    //}

                }
                else if (playerStamina > 1)
                {
                    //combat
                    //Heavy Atk
                    moveable = false;
                        playerAnimator.SetTrigger("heavyAttack");
                        playerAnimator.ResetTrigger("lightAttack");
                    playerStamina -= 2;



                }else {
                    moveable = false;
                    playerAnimator.SetTrigger("OOS");
                }
            }
            if (Input.GetButtonUp("Y"))
            {
                //print("hit Y, combat: " + inCombat);

                if (!inCombat)
                {
                    //passive 
                    //ForceWalk
                    preapareForBattle();
                }
                else if (playerStamina > 0)
                {
                    //combat
                    //LightAtk
                    moveable = false;
                    playerAnimator.SetTrigger("lightAttack");
                        playerAnimator.ResetTrigger("heavyAttack");
                    playerStamina--;

                } else {
                    moveable = false;
                    playerAnimator.SetTrigger("OOS");
                }
            }

        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        if (moveable) {
            calcualteFacing();
            if (inCombat)
            {
                //t += Time.deltaTime / incrementMoveTime;
                //transform.position = Vector3.Lerp(startPosition, target, t);
                moveHorizontal = Input.GetAxis("Horizontal");
                moveVertical = 0;

            }
            else {
                moveHorizontal = Input.GetAxis("Horizontal");
                moveVertical = Input.GetAxis("Vertical");
            }
            

            smoothMove();

           
            if (Input.GetButtonUp("B"))
            {
                

                if (inCombat)
                {
                    //jump 
                    //print("hit B, combat: " + inCombat);
                }
               
            }
            

        }
       

    }
    void calcualteFacing() {

        if (Mathf.Abs(body.velocity.x) > 0 && Mathf.Abs(body.velocity.x) > Mathf.Abs(body.velocity.z))
        {
            playerAnimator.SetInteger("Facing", 3);
            if (body.velocity.x < 0)
            {
                _facingLeft = true;
                //theScale.x = 1;
                //transform.localScale = theScale;
                charRenderer.flipX = true;
            }
            else if (body.velocity.x > 0 || !_facingLeft)
            {
                _facingLeft = false;
                //theScale.x = -1;
                //transform.localScale = theScale;
                charRenderer.flipX = false;
            }
            //playerAnimator.SetBool("FacingLeft", _facingLeft);
        }
        else if (Mathf.Abs(body.velocity.z) > 0 && Mathf.Abs(body.velocity.z) > Mathf.Abs(body.velocity.x + 1))
        {
            if (!inCombat)
            {
                charRenderer.flipX = false;
            }
            if (body.velocity.z > 0)
            {
                playerAnimator.SetInteger("Facing", 1);
            }
            else if (body.velocity.z <= 0)
            {
                playerAnimator.SetInteger("Facing", 2);
            }
        }
    }
   

    void incrementalMove() {
        float xMove = this.transform.position.x;
        float zMove = this.transform.position.z;
        
        if (moveHorizontal > 0.05f)
        {
            //move right
            xMove += leapDistance;
            //cameraOffsetX = -1 * leapDistance;
            _facingLeft = false;
            //theScale.x = -1;
            //transform.localScale = theScale;
            charRenderer.flipX = false;
        }
        else if (moveHorizontal < -0.05f)
        {
            //move left
            xMove -= leapDistance;
            _facingLeft = true;
            //theScale.x = 1;
            //transform.localScale = theScale;
            charRenderer.flipX = true;
            //cameraOffsetX = leapDistance;
        }
        if (moveVertical > 0.05f)
        {
            //move up
            zMove += leapDistance;
            //cameraOffsetZ = -1 * leapDistance;
        }
        else if (moveVertical < -0.05f)
        {
            //move down
            zMove -= leapDistance;
            //cameraOffsetZ = leapDistance;
        }

        if (xMove != this.transform.position.x || zMove != this.transform.position.z)
        {
            print("StartX: " + this.transform.position.x);
            print("X Move: " + xMove);
            endpoint = new Vector3(xMove, 0,zMove);
            print(endpoint);
            //playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, new Vector3(playerCamera.transform.position.x + cameraOffsetX, 0, playerCamera.transform.position.z + cameraOffsetZ), Time.time);
            
            SetIncrementDestination(endpoint,LeapAniTime);
            playerAnimator.SetBool("ScootNext", true);
            moveable = false;
        }
        else {
            playerAnimator.SetBool("ScootNext", false);
        }
    
    }
    public void preapareForBattle() {
        //startPosition = target = transform.position;
        inCombat = true;
        playerAnimator.SetBool("InCombat", inCombat);
        playerAnimator.SetTrigger("enterCombat");
        moveable = false;
        //showBattleCamera();
        //unhookCamera();
    }
    public void leaveBattle() {
        
        inCombat = false;
        playerAnimator.SetBool("InCombat", inCombat);
        hookUpCamera();

    }
    public void SetIncrementDestination(Vector3 destination, float time)
    {
        t = 0;
        startPosition = transform.position;
        incrementMoveTime = time;
        target = destination;
    }

    void smoothMove() {
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        

        body.AddForce(movement * speed);
        playerAnimator.SetFloat("Speed", Mathf.Abs(body.velocity.z) + Mathf.Abs(body.velocity.x));

    }
    public void unhookCamera() {
        playerCamera.transform.parent = null;
    }
    public void hookUpCamera() {
        playerCamera.transform.parent = this.transform;

    }
    public void resetCameraToExplore() {
        //playerCamera.transform.position = cameraRestPosition;
    }

   

    public void ApplyDamage(int atkType) {
        //apply damage to enemies in the list
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position,attackRange);

        foreach (Collider enemy in hitEnemies) {
            curEnemy = enemy.GetComponent<EnemyBasic>();

            if(curEnemy != null)
            curEnemy.takeDamage(basicAttacksDmg[atkType-1]);

            curEnemy = null;

        }
    }

    public void resetStamina() {
        playerStamina = playerMaxStamina;
    
    }







    public void showPlayer()
    {
        playerCamera.enabled = true;
        //MapCamera.enabled = false;
        BattleCamera.enabled = false;

    }

    public void showMap()
    {
        playerCamera.enabled = false;
        //MapCamera.enabled = true;
        BattleCamera.enabled = false;
    }

    public void showBattleCamera() {
        playerCamera.enabled = false;
        //MapCamera.enabled = false;
        BattleCamera.enabled = true;
    }

    void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
