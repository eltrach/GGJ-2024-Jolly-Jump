using UnityEngine.AI;

public class AiDeathState : AiState
{
    public AiStateID GetID()
    {
        return AiStateID.Idle;
    }
    public void Enter(AiAgent agent)
    {

    }

    public void Exit(AiAgent agent)
    {
        agent._ragdoll.ActivateRagdoll();
        agent._navMeshAgent.isStopped = true;
    }
    public void Update(AiAgent agent)
    {

    }
}
