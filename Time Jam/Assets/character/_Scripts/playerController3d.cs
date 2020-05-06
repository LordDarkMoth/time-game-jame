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

    //private Vector3 cameraRestPosition;

    private bool inMenu = false;


    //combat Tracking
    public int comboPoints = 0;
    public Text comboBox;
    public bool atkInputRecieved = false;
    private int[] superMoveValues = new int[4];
    private int currentSuperSlot = 0;
    //public VillagerChar targetVillager;

    public int speed;
    public float leapDistance;
    public float LeapAniTime;
    public int playerMaxStamina = 4;
    public int playerStamina;

    private float t;
    Vector3 startPosition;
    Vector3 target;
    float incrementMoveTime;
    
    public bool moveable = true;
    private Vector3 endpoint;


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

        //cameraRestPosition = playerCamera.transform.position;


    }
   
    // Update is called once per frame
    void FixedUpdate()
    {
        if (inCombat)
        {
            //t += Time.deltaTime / incrementMoveTime;
            //transform.position = Vector3.Lerp(startPosition, target, t);


        }
        if (moveable) {
            calcualteFacing();
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");

            smoothMove();

            //if (!inCombat)
            //{
            //    smoothMove();
            //}
            //else
            //{
            //    incrementalMove();

            //}


            //if (Input.GetKeyUp("space")) {
            //inCombat = !inCombat;
            //playerAnimator.SetBool("InCombat", inCombat);

            //}
            //Buttons
            if (Input.GetButtonUp("A"))
            {
                print("hit A, combat: " + inCombat);

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
                print("hit B");

                if (!inCombat)
                {
                    //passive 
                    //cancel
                }
                else
                {
                    //combat
                    //Jump
                }
            }
            if (Input.GetButtonUp("X"))
            {
                print("hit X");

                if (!inCombat)
                {
                    //passive
                    // Stats
                    if (inMenu)
                    {
                        showPlayer();
                        inMenu = false;
                    }
                    else
                    {
                        showMap();
                        inMenu = true;
                    }

                }
                else
                {
                    //combat
                    //Heavy Atk
                    if (!atkInputRecieved)
                    {
                        //atkInputRecieved = true;
                        //if (playerStamina <=1) {
                        //    playerAnimator.SetBool("OutOfStamNext", true);
                        //    atkInputRecieved = false;
                        //} else if (playerStamina > 1)
                        //{
                            playerAnimator.SetBool("CmbtHeavyNext", true);

                            calculateheaveyHit();
                        //    playerStamina -= 2;
                        //}
                        //else
                        //{

                        //}
                    }

                }
            }
            if (Input.GetButtonUp("Y"))
            {
                print("hit Y");

                if (!inCombat)
                {
                    //passive 
                    //ForceWalk
                }
                else
                {
                    //combat
                    //LightAtk
                    if (!atkInputRecieved)
                    {
                        atkInputRecieved = true;
                        //if (playerStamina <= 0) {
                        //    playerAnimator.SetBool("OutOfStamNext", true);
                        //    atkInputRecieved = false;
                        //} else if (playerStamina > 0) {
                            playerAnimator.SetBool("CmbtLightNext", true);

                            calculateLightHit();
                            //playerStamina -= 1;
                        //}


                    }


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
        startPosition = target = transform.position;
        inCombat = true;
        playerAnimator.SetBool("InCombat", inCombat);
        showBattleCamera();
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
    void calculateLightHit() {

        comboPoints = Mathf.Clamp(comboPoints + 1, 0, 8);
        //comboBox.text = comboPoints.ToString();
        playerAnimator.SetInteger("ComboPoints", comboPoints);

    }
    void calculateheaveyHit()
    {
        comboPoints = Mathf.Clamp(comboPoints + 2,0,8);
        //comboBox.text = comboPoints.ToString();
        playerAnimator.SetInteger("ComboPoints", comboPoints);
    }

    void calculateFinisherDamage() {

    }


    public void AddComboToSuper() {
        if (currentSuperSlot < 4) {
            superMoveValues[currentSuperSlot] = comboPoints;
            //populate correct trackerBox
            currentSuperSlot += 1;
        }


    }

    public void useSuperMove() {
        //transverse numbers- 1:MeleeStill, 2:Throw, 3:MeleeDash, 4: RangeRapid, 5:MeleeSplash, 6:RangedBurst, 7:Melee Launch, 8:Summon 

        playerAnimator.SetInteger("SuperAnimationTransverse", comboPoints);
        playerAnimator.SetInteger("SuperAnimationRelease", comboPoints);
        playerAnimator.SetBool("CombatSuperNext", true);

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
}
