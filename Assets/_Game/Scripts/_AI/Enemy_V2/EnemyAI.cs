using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using VTemplate.Controller;
using Random = UnityEngine.Random;

namespace EnemyV2
{
    public class EnemyAI : MonoBehaviour
    {
        //Locomotion
        [SerializeField] private float _walkSpeed = 3f;
        [SerializeField] private float _runSpeed = 7f;
        [SerializeField] private LayerMask _whatIsGround, _whatIsPlayer;

        //Patroling
        [SerializeField] private Vector3 _walkPosition;
        [SerializeField] private bool _walkPointSet;
        [SerializeField] private float _walkPointRange;

        //Attacking
        [SerializeField] private float _timeBetweenAttacks;
        [SerializeField] private bool _alreadyAttacked;


        [SerializeField] private bool showDetectionVars;
        [Header("Detection :")]
        [SerializeField, ShowIf("showDetectionVars")] private float _sightRange;
        [SerializeField, ShowIf("showDetectionVars")] private float _attackRange;
        [SerializeField, ShowIf("showDetectionVars")] private bool _playerInSightRange, _playerInAttackRange;

        [SerializeField] private bool showComponents;
        [Header("Components :")]
        [SerializeField, ShowIf("showComponents")] private Animator _animator;
        [SerializeField, ShowIf("showComponents")] private NavMeshAgent _navMeshAgent;
        [SerializeField, ShowIf("showComponents")] private Transform _player;
        [SerializeField, ShowIf("showComponents")] private Ragdoll ragdoll;
        [SerializeField, ShowIf("showComponents")] private FadeOut fadeOut;

        [SerializeField, ShowIf("showComponents")] private bool _hited = false;
        private bool _isDead;

        [Header("Wandering:")]
        [SerializeField] private bool _wanderingEnabled = true;
        [SerializeField, ShowIf("_wanderingEnabled")] private float _wanderRadius = 5f;
        [SerializeField, ShowIf("_wanderingEnabled")] private float _wanderTimer = 5f;
        private bool _isWandering = false;
        private float _currentWanderTimer = 0f;
        private Vector3 _wanderPoint;


        [SerializeField] private bool destroyBodyAfterDead = true;
        [SerializeField] private int secondsToWaitBeforeDestroy = 5; // Seconds
        public int SecondsToWaitBeforeDestroy { get => secondsToWaitBeforeDestroy; set => secondsToWaitBeforeDestroy = value; }

        private void Awake()
        {
            LoadComponents();
        }
        private void LoadComponents()
        {
            _player = FindObjectOfType<ThirdPersonController>().transform;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            ragdoll = GetComponent<Ragdoll>();
            fadeOut = GetComponent<FadeOut>();
        }
        private void Start()
        {
            SetAgentSpeed(_walkSpeed);
        }
        private void Update()
        {
            if (_isDead) return;
            //Check for sight and attack range
            _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
            _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);
            //_animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
            if (_navMeshAgent.velocity.magnitude > 0.1f) _animator.SetBool("Run", true);
            else _animator.SetBool("Run", false);

            if (_hited) HitBythePlayer();

            if (!_playerInSightRange && !_playerInAttackRange) Wander(); // if we the player isn't in the attack range do patrol
            if (_playerInSightRange && !_playerInAttackRange) ChasePlayer(); // if the player is in sight range chase him
            if (_playerInAttackRange && _playerInSightRange) AttackPlayer(); // if in sight and in attack range then DO attack;
        }
        private void Wander()
        {
            if (!_isWandering)
            {
                _currentWanderTimer += Time.deltaTime;
                if (_currentWanderTimer >= _wanderTimer)
                {
                    _wanderPoint = GetRandomPointInRadius(transform.position, _wanderRadius);
                    _isWandering = true;
                    _currentWanderTimer = 0f;
                }
            }
            else
            {
                SetAgentSpeed(_walkSpeed);
                _navMeshAgent.SetDestination(_wanderPoint);

                Vector3 distanceToWanderPoint = transform.position - _wanderPoint;

                if (distanceToWanderPoint.magnitude < 1f)
                {
                    _isWandering = false;
                    SetAgentSpeed(0f);
                }
            }
        }
        private void ChasePlayer()
        {
            if (!_player) return;
            if (_hited) return;
            SetAgentSpeed(_runSpeed);
            _navMeshAgent.SetDestination(_player.position);
        }
        private void AttackPlayer()
        {
            //Make sure enemy doesn't move
            _navMeshAgent.SetDestination(transform.position);

            transform.LookAt(_player);

            if (!_alreadyAttacked)
            {
                ///Attack code here
                MeleeAttack();
                ///End of attack code

                _alreadyAttacked = true;
                Invoke(nameof(ResetAttack), _timeBetweenAttacks);
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
            //if (GameManager.Instance.GetPlayer().GetComponent<HealthSystem>().IsAlive()) return;
            _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, Time.deltaTime * 13f));
            _animator.SetTrigger("Attack");
            Debug.Log(" ---> MeleeAttack() ");

            if (_playerInAttackRange)
            {
                // damage the player
                Debug.Log("PLAYER DAMAGED");
                GameManager.Instance.GetPlayer().GetComponent<IHealthSystem>().TakeDamage(5);
            }
        }
        private void ResetAttack()
        {
            _alreadyAttacked = false;
        }
        // if the player shoot at the AI the Ai will go to the player position and chase him running to kill him
        public void HitBythePlayer()
        {
            _hited = true;
            //Debug.Log(" EnemyAI.HitBythePlayer()");
            SetAgentSpeed(_runSpeed);
            if (_player) _navMeshAgent.SetDestination(_player.position);
            else Debug.LogWarning("<color=red> Player Didnt Found <color/>");
        }


        public void Die()
        {
            if (_isDead) return;
            if (WaveSystem.Instance) WaveSystem.Instance.OnEnemyDeath();

            _animator.SetTrigger("Die");
            _navMeshAgent.SetDestination(transform.position);
            _navMeshAgent.isStopped = true;

            // Fade Out Material Color ... 
            if (fadeOut) fadeOut.FadeOutStart();

            if (ragdoll) ragdoll.ActivateRagdoll();
            Destroy(GetComponent<FireableTarget>());

            //Root.UIManager.IncreaseKillsCounter();
            if (destroyBodyAfterDead) DestoryAfterXSec(SecondsToWaitBeforeDestroy);

        }
        private void DestoryAfterXSec(int toWait)
        {
            Destroy(gameObject, toWait);
            WaveSystem.Instance.RefreshSpawnedList();
        }
        private void SetAgentSpeed(float speed) { _navMeshAgent.speed = speed; }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _sightRange);

            // Draw gizmo for walk position
            if (_walkPointSet)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, _walkPosition);
                Gizmos.DrawWireSphere(_walkPosition, 0.5f);
            }
            // Draw gizmo for wander state
            if (_wanderingEnabled && _isWandering)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(_wanderPoint, 0.5f);
            }
            // Draw gizmo for chase position
            if (_playerInSightRange)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, _player.position);
                Gizmos.DrawWireSphere(_player.position, 0.5f);
            }
        }
    }
}