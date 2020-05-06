using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]


public class playerController : MonoBehaviour {

public float speed = 10;
public float  jumpVelocity = 10;
public bool canMoveInAir = true;
public LayerMask playerMask;
public bool topdown = false;
public bool _facingLeft = true;
private Animator playerAnimator;

Transform myTrans;
Transform tagGround;
Rigidbody2D myBody;
bool isGrounded = false;
Vector3 theScale;
private bool lockedOn = false;
    //public VillagerChar targetVillager;



// Use this for initialization
void Start () {
myBody = this.GetComponent<Rigidbody2D>();	
myTrans = this.transform;
tagGround = GameObject.Find (this.name + "/Tag_Ground").transform;
playerAnimator = GetComponent<Animator>();
theScale = myTrans.localScale;
} 

// Update is called once per frame
void FixedUpdate () {
//moving
if (!topdown) {
	isGrounded = Physics2D.Linecast (myTrans.position, tagGround.position, playerMask); 
	MovePlayerSide (Input.GetAxisRaw ("Horizontal"));
	if (Input.GetButtonDown ("Jump")) {
		Jump ();
	}
} else {
	MovePlayerTop (Input.GetAxisRaw ("Horizontal"),Input.GetAxisRaw ("Vertical"));

    if(Mathf.Abs(myBody.velocity.x) > 0 || Mathf.Abs(myBody.velocity.y) > 0){
        
            if(myBody.velocity.x < 0){
                _facingLeft = true;
                    //theScale.x = 1;
                    //transform.localScale = theScale;
                    GetComponent<SpriteRenderer>().flipX = false;
            } else if (myBody.velocity.x > 0 || !_facingLeft)
            {
                _facingLeft = false;
                    //theScale.x = -1;
                    //transform.localScale = theScale;
                    GetComponent<SpriteRenderer>().flipX = true;
                }
            playerAnimator.SetBool("FacingLeft", _facingLeft);
    } else{
        
        //theScale.x = 1;
        //transform.localScale = theScale;
    }
             

}
    playerAnimator.SetFloat("Speed", Mathf.Abs(myBody.velocity.y) + Mathf.Abs(myBody.velocity.x));

    //attacking
    if (Input.GetKeyUp("left shift")) {
        lockedOn = !lockedOn;
       
    }

    playerAnimator.SetBool("LockedOn", lockedOn);

        //Interaction
        if (Input.GetButtonUp("Fire1")) {
            print("hit A");
            //if(targetVillager != null) {
            //    targetVillager.InteractWithVillager();
            //}
        }

    }
public void MovePlayerTop(float horizontalInput,float verticalInput){

Vector2 moveVel = myBody.velocity;
moveVel.x = horizontalInput * speed;
    if (!lockedOn) {
        moveVel.y = verticalInput * speed;
    }
    else {
        moveVel.y = speed * 0;
    }

    if (moveVel.x > 0)
{
    if (_facingLeft)
    {
        _facingLeft = false;
    }
}
else if (moveVel.x < 0)
{
    if(!_facingLeft){
        
        _facingLeft = true;
    }
   
}
playerAnimator.SetBool("FacingLeft", _facingLeft);
myBody.velocity = moveVel;
}

public void MovePlayerSide(float horizontalInput){
if(!canMoveInAir && !isGrounded){
	return;
}
Vector2 moveVel = myBody.velocity;
moveVel.x = horizontalInput * speed;
print(moveVel.x);

myBody.velocity = moveVel;


}

public void Jump(){
if(isGrounded){
	myBody.velocity += jumpVelocity * Vector2.up;

}

}
}
