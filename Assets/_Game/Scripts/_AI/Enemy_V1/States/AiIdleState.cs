using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AiState
{
    public AiStateID GetID()
    {
        return AiStateID.Idle;
    }
    public void Enter(AiAgent agent)
    {

    }
    public void Update(AiAgent agent)
    {
        Vector3 playerDirection = agent.GetPlayerPos() - agent.transform.position;
        if (playerDirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }
        
        Vector3 agentDirection = agent.transform.forward;
        playerDirection.Normalize();

        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        if(dotProduct > 0f)
        {
            agent.stateMachine.ChangeState(AiStateID.Idle);
        }
    }
    public void Exit(AiAgent agent)
    {

    }

}
