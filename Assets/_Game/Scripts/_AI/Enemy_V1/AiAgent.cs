using UnityEngine;
using UnityEngine.AI;
using VTemplate.Controller;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateID initialState;
    public NavMeshAgent _navMeshAgent;
    public AiAgentConfig config;
    public Ragdoll _ragdoll;
    public Animator _animator;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _ragdoll = GetComponent<Ragdoll>();
        _animator = GetComponent<Animator>();
        stateMachine = new AiStateMachine(this);

        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.ChangeState(initialState);
    }

    void Update()
    {
        stateMachine.Update();
        
    }
    public Vector3 GetPlayerPos()
    {
        return FindObjectOfType<ThirdPersonController>().transform.position;
    }
}
