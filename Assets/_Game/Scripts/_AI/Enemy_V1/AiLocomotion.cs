using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class AiLocomotion : MonoBehaviour
{
    // private / Unserialized
    private NavMeshAgent _navMeshAgent;
    private AiAgent _aiAgent;
    private Animator _animator;
    private Ragdoll _ragdoll;
    private int _animIDLocomotion;
    // animation IDs

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _aiAgent = GetComponent<AiAgent>();
        _animator = GetComponentInChildren<Animator>();
        _ragdoll = GetComponent<Ragdoll>();
        GetAnimationIDs();
    }

    private void GetAnimationIDs()
    {
        _animIDLocomotion = Animator.StringToHash("Speed");
    }

    private void Update()
    {
        // animation~~
        _animator.SetFloat(_animIDLocomotion, _navMeshAgent.velocity.magnitude);
    }

    public void Die()
    {
        AiDeathState deathState = _aiAgent.stateMachine.GetState(AiStateID.Idle) as AiDeathState;
        _aiAgent.stateMachine.ChangeState(AiStateID.Idle);
    }
}
