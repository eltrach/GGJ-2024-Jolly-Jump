using UnityEngine;
using VTemplate.Controller;

// this should be a global class for combat so we could use it for AIs and also the Player
public class ThirdPersonMelee : MonoBehaviour
{
    public int attackDamage = 20;
    public float attackRange = 2f;
    public float attackCooldown = 0.2f;

    public LayerMask enemyLayer;

    private Animator animator;
    private ThirdPersonController cc;
    private ThirdPersonInputs inputs;
    private LockOn LockOn;

    private bool IsPlayer => cc;
    private bool isAttacking = false;

    [SerializeField] private AttackReceiver leftHandHitBox;
    [SerializeField] private AttackReceiver rightHandHitBox;

    private int damageMultiplier;
    private int currentRecoilID;
    private int currentReactionID;
    private bool ignoreDefense;
    private bool activeRagdoll;
    private float senselessTime;
    private bool inRecoil;
    private string attackName;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //Load Components 
        
        animator = GetComponent<Animator>();
        inputs = GetComponent<ThirdPersonInputs>();
        cc = GetComponent<ThirdPersonController>();
        LockOn = GetComponent<LockOn>();
        
        // logic

        //hands
        SetActiveAttack(Bodypart.leftHand, false);
        SetActiveAttack(Bodypart.rightHand, false);
        // legs
        SetActiveAttack(Bodypart.leftLeg, false);
        SetActiveAttack(Bodypart.rightLeg, false);
    }
    private void Update()
    {
        if (IsPlayer)
        {
            if (inputs.Shot) WeakAttacks(); // shot / Mouse-left Button = Weak attacks
            else if (inputs.Aim) StrongAttacks();
        }
    }

    public void WeakAttacks()
    {
        if (IsPlayer) LockOn.Fire();
        // Play/Trigger attack animation
        animator.SetInteger("AttackID", 0); // the AttackID mean the Attack type ( unarmed - sword type animations - axe type... )
        animator.SetTrigger("WeakAttacks");

        isAttacking = true;
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    public void StrongAttacks()
    {
        if (IsPlayer) LockOn.Fire();
        // Play attack animation
        animator.SetInteger("AttackID", 0);
        animator.SetTrigger("StrongAttacks");

        isAttacking = true;
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    public void SetActiveAttack(Bodypart bodyPart, bool value, int damageMultiplier = 1, int recoilID = 1, int reactionID = 1, bool ignoreDefense = false, bool activeRagdoll = false)
    {
        this.damageMultiplier = damageMultiplier;
        currentRecoilID = recoilID;
        currentReactionID = reactionID;
        this.ignoreDefense = ignoreDefense;
        this.activeRagdoll = activeRagdoll;

        if (bodyPart == Bodypart.leftHand)
        {
            leftHandHitBox.SetActiveDamage(value, damageMultiplier, recoilID, ignoreDefense, activeRagdoll);
        }
        else if (bodyPart == Bodypart.rightHand)
        {
            rightHandHitBox.SetActiveDamage(value, damageMultiplier, recoilID, ignoreDefense, activeRagdoll);
        }
    }
    private void ResetAttack()
    {
        isAttacking = false;
    }
}
