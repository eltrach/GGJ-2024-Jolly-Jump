using System;
using UnityEngine;

public enum Bodypart
{
    leftHand = 0,
    rightHand = 1,
    leftLeg = 2,
    rightLeg = 3,
}

public class MeleeAttackControl : StateMachineBehaviour
{
    [Tooltip("Normalized time of Active Damage")]
    public float startDamage = 0.05f;
    [Tooltip("Normalized time of Disable Damage")]
    public float endDamage = 0.9f;
    public int damageMultiplier;
    public int recoilID;
    public int reactionID;

    [Tooltip("You can use a name as a reference to trigger a custom HitDamageParticle")]
    public string damageType;

    public Bodypart bodyPart;

    public bool ignoreDefense;
    public bool activeRagdoll;

    [Tooltip("Check true in the last attack of your combo to reset the triggers")]
    public bool resetAttackTrigger;
    public bool debug;

    private bool isActive;
    private ThirdPersonMelee meleeController;
    private bool isAttacking;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        meleeController = animator.GetComponent<ThirdPersonMelee>();
        isAttacking = true;

        if (debug)
            Debug.Log("Enter " + damageType);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime % 1 >= startDamage && stateInfo.normalizedTime % 1 <= endDamage && !isActive)
        {
            if (debug) Debug.Log(animator.name + " attack " + damageType + " enable damage in " + System.Math.Round(stateInfo.normalizedTime % 1, 2));
            isActive = true;
            ActiveDamage(animator, true);
        }
        else if (stateInfo.normalizedTime % 1 > endDamage && isActive)
        {
            if (debug) Debug.Log(animator.name + " attack " + damageType + " disable damage in " + System.Math.Round(stateInfo.normalizedTime % 1, 2));
            isActive = false;
            ActiveDamage(animator, false);
        }

        if (stateInfo.normalizedTime % 1 > endDamage && isAttacking)
        {
            isAttacking = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (debug)
            Debug.Log("Exit " + damageType);

        if (isActive)
        {
            isActive = false;
            ActiveDamage(animator, false);
        }
        if (debug) Debug.Log(animator.name + " attack " + damageType + " stateExit");
    }

    void ActiveDamage(Animator animator, bool value)
    {
        var meleeManager = animator.GetComponent<ThirdPersonMelee>();
        if (meleeManager)
            meleeManager.SetActiveAttack(bodyPart, value, damageMultiplier, recoilID, reactionID, ignoreDefense, activeRagdoll);
    }
}