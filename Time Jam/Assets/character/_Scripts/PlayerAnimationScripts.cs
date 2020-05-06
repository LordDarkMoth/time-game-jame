using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScripts : MonoBehaviour
{
    private Animator playerAnimator;
    public playerController3d playerScript;
    //public BattleManager battleManager;
    //public ComboTrackerUI comboUI;

    public int repeatCount = 0;

    // use these are set by the players stats and then fed into the battlemanager to do the damage.
    public float[] basicAtkRangeMin = new float[4]; // 0: F-light 1: B-light 2: F-heavy 3:B-heavy
    public float[] basicAtkRangeMax = new float[4];

    public float[] basicDamageAug = new float[4];


    public float[] finishersXRangeMin = new float[8];
    public float[] finishersXRangeMax = new float[8];
    public float[] finishersZRangeMin = new float[8];
    public float[] finishersZRangeMax = new float[8];
    public float[] finishersDamageAug = new float[8];



    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        //comboUI.setUpComboTracker(playerScript.playerMaxStamina);
        StaminaReset();

    }

    // Update is called once per frame
    void Update()
    {

    }
    // combat animations
    public void resetLightAtk() {
        playerAnimator.SetBool("CmbtLightNext", false);
        playerScript.atkInputRecieved = false;
    }
    public void resetHeavyAtk()
    {
        playerAnimator.SetBool("CmbtHeavyNext", false);
        playerScript.atkInputRecieved = false;

    }
    public void resetFinisher()
    {
        playerAnimator.SetBool("CmbtFinisherNext", false);
        playerAnimator.SetInteger("ComboPoints", 0);
        playerScript.atkInputRecieved = false;
        playerScript.AddComboToSuper();
        playerScript.comboPoints = 0;

    }
    public void resetPostDash()
    {
        playerAnimator.SetBool("ScootNext", false);
        playerScript.moveable = true;
    }

    public void triggerBasicAttack(int attackType)
    {

        //print("triggered basic attack type: " + attackType + "facing left is: " + playerScript._facingLeft);
        //battleManager.calculateBasicPlayerAttack(basicAtkRangeMin[attackType], basicAtkRangeMax[attackType], basicDamageAug[attackType], playerScript._facingLeft);
        if (attackType < 2) {
            //comboUI.uiBasicAttackUpdate(1);
        } else if (attackType >= 2)
        {
            //comboUI.uiBasicAttackUpdate(2);
        }
    }


    public void triggerFinisher(){
        //battleManager.calculateFinisherAttack();

    }

    public void outOfStamina() {
        //comboUI.uiBasicAttackUpdate(-1);
        playerAnimator.SetBool("OutOfStamNext", false);


    }

    public void StaminaReset() {
        playerScript.playerStamina = playerScript.playerMaxStamina;
        //comboUI.fillStaminaToMax();
        //comboUI.ClearCurrentMedallion();
    }
}
