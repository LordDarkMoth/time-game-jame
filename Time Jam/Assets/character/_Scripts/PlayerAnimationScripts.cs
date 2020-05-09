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
    
    public void resetPostDash()
    {
        playerAnimator.SetBool("ScootNext", false);
        playerScript.moveable = true;
    }

    public void triggerBasicAttack(int attackType)
    {
        playerScript.ApplyDamage(attackType);

    }


    public void triggerFinisher(){
        //battleManager.calculateFinisherAttack();

    }

    public void SwitchToBack() {
        
        playerAnimator.SetBool("CombatFrontFacing", false);
    }
    public void SwitchToFront()
    {

        playerAnimator.SetBool("CombatFrontFacing", true);
    }
    public void StaminaReset() {
        playerScript.playerStamina = playerScript.playerMaxStamina;
        //comboUI.fillStaminaToMax();
        //comboUI.ClearCurrentMedallion();
    }
}
