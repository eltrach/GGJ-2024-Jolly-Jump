using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using VTemplate.Controller;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class ThirdPersonEnemy : MonoBehaviour
    {
        [Title("Locomotion")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 7f;
        [SerializeField] private LayerMask whatIsPlayer;

        [Title("Attacking")]
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private bool alreadyAttacked;
        [SerializeField] private bool attackingEnabled = true;
        [SerializeField] private bool chasingEnabled = true;

        [Title("Detection")]
        [SerializeField, ShowInInspector] private bool showDetectionVars;
        [SerializeField, ShowIf("showDetectionVars")] private float sightRange;
        [SerializeField, ShowIf("showDetectionVars")] private float attackRange;

        [Title("Components")]
        [SerializeField, ShowInInspector] private bool showComponents;
        [SerializeField, ShowIf("showComponents")] private Animator animator;
        [SerializeField, ShowIf("showComponents")] private NavMeshAgent navMeshAgent;
        [SerializeField, ShowIf("showComponents")] private Transform player;
        [SerializeField, ShowIf("showComponents")] private Ragdoll ragdoll;
        [SerializeField, ShowIf("showComponents")] private FadeOut fadeOut;
        [SerializeField, ShowIf("showComponents")] private ThirdPersonMelee thirdPersonMelee;


        [Title("Wandering")]
        [SerializeField] private bool wanderingEnabled = true;
        [SerializeField, ShowIf("wanderingEnabled")] private float wanderRadius = 5f;
        [SerializeField, ShowIf("wanderingEnabled")] private float wanderTimer = 5f;
        private bool isWandering = false;
        private float currentWanderTimer = 0f;
        private Vector3 wanderPoint;

        [Title("Death Settings")]
        [SerializeField] private bool destroyBodyAfterDead = true;
        [SerializeField, ShowIf("destroyBodyAfterDead")] private int secondsToWaitBeforeDestroy = 5;

        private bool isDead;
        private bool hited;

        private void Awake()
        {
            LoadComponents();
        }

        private void LoadComponents()
        {
            player = FindObjectOfType<ThirdPersonController>().transform;
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            ragdoll = GetComponent<Ragdoll>();
            fadeOut = GetComponent<FadeOut>();
            thirdPersonMelee = GetComponent<ThirdPersonMelee>();
        }

        private void Start()
        {
            InitializeAnimator();
            SetAgentSpeed(walkSpeed);
        }
        private void InitializeAnimator() => animator.SetFloat("Speed", 0f);
        private void Update()
        {
            if (isDead) return;
            
            //Check for sight and attack range
            bool playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


            float speed = navMeshAgent.velocity.magnitude;
            animator.SetFloat("Speed", speed);

            if (hited) HitByThePlayer();

            if (!playerInSightRange && !playerInAttackRange && wanderingEnabled)
                Wander(); // if the player isn't in the attack range do patrol
            if (playerInSightRange && !playerInAttackRange && chasingEnabled)
                ChasePlayer(); // if the player is in sight range chase him
            if (playerInAttackRange && playerInSightRange && attackingEnabled)
                AttackPlayer(); // if in sight and in attack range then DO attack;
        }

        private void Wander()
        {
            if (!isWandering)
            {
                currentWanderTimer += Time.deltaTime;
                if (currentWanderTimer >= wanderTimer)
                {
                    wanderPoint = GetRandomPointInRadius(transform.position, wanderRadius);
                    isWandering = true;
                    currentWanderTimer = 0f;
                }
            }
            else
            {
                SetAgentSpeed(walkSpeed);
                navMeshAgent.SetDestination(wanderPoint);

                Vector3 distanceToWanderPoint = transform.position - wanderPoint;

                if (distanceToWanderPoint.magnitude < 1f)
                {
                    isWandering = false;
                    SetAgentSpeed(0f);
                }
            }
        }

        private void ChasePlayer()
        {
            if (!player) return;
            if (hited) return;
            SetAgentSpeed(runSpeed);
            navMeshAgent.SetDestination(player.position);
        }

        private void AttackPlayer()
        {
            // Make sure enemy doesn't move
            navMeshAgent.SetDestination(transform.position);

            // Look at the player only on the XZ (horizontal) plane
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0f;
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
            //transform.LookAt(player);

            if (!alreadyAttacked)
            {
                //Debug.Log("attack played");
                //MeleeAttack();
                thirdPersonMelee.WeakAttacks();
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }

        private Vector3 GetRandomPointInRadius(Vector3 center, float radius)
        {
            Vector3 randomPoint = center + (Random.insideUnitSphere * radius);
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas);
            return hit.position;
        }

        private void MeleeAttack()
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 13f));
            animator.SetTrigger("Attack");

            if (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer))
            {
                // damage the player
                GameManager.Instance.GetPlayer().GetComponent<IHealthSystem>().TakeDamage(5);
            }
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        public void HitByThePlayer()
        {
            hited = true;
            SetAgentSpeed(runSpeed);
            if (player) navMeshAgent.SetDestination(player.position);
        }

        public void Die()
        {
            if (isDead) return;
            if (WaveSystem.Instance) WaveSystem.Instance.OnEnemyDeath();

            animator.SetTrigger("Die");
            navMeshAgent.SetDestination(transform.position);
            navMeshAgent.isStopped = true;

            if (fadeOut) fadeOut.FadeOutStart();

            if (ragdoll) ragdoll.ActivateRagdoll();
            Destroy(GetComponent<FireableTarget>());

            //Root.UIManager.IncreaseKillsCounter();
            if (destroyBodyAfterDead) DestroyAfterXSec(secondsToWaitBeforeDestroy);
        }

        private void DestroyAfterXSec(int toWait)
        {
            Destroy(gameObject, toWait);
            WaveSystem.Instance.RefreshSpawnedList();
        }

        private void SetAgentSpeed(float speed) => navMeshAgent.speed = speed;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);

            // Draw gizmo for wander state
            if (wanderingEnabled && isWandering)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(wanderPoint, 0.5f);
            }
            // Draw gizmo for chase position
            if (Physics.CheckSphere(transform.position, sightRange, whatIsPlayer))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, player.position);
                Gizmos.DrawWireSphere(player.position, 0.5f);
            }
        }
    }
}
