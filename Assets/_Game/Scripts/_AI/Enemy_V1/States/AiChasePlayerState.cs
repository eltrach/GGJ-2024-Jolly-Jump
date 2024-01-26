using VTemplate.Controller;
using UnityEngine;

public class AiChasePlayerState : AiState
{
    //private Transform _target;
    float timer = 0f;

    public AiStateID GetID()
    {
        return AiStateID.ChasePlayer;
    }
    public void Enter(AiAgent agent)
    {

    }
    public void Exit(AiAgent agent)
    {

    }
    public void Update(AiAgent _agent)
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            float sqdistance = (_agent.GetPlayerPos() - _agent._navMeshAgent.destination).sqrMagnitude;
            if (sqdistance > _agent.config.minDistance * _agent.config.minDistance)
            {
                _agent._navMeshAgent.SetDestination(_agent.GetPlayerPos() );
            }
            timer = _agent.config.maxTime;
        }
    }
    
}
