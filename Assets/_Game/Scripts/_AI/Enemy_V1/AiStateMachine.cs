using UnityEngine.UI;

public class AiStateMachine
{
    public AiState[] states;
    public AiAgent _agent;
    public AiStateID currentState;

    public AiStateMachine(AiAgent aiAgent)
    {
        _agent = aiAgent;
        int numStates = System.Enum.GetNames(typeof(AiStateID)).Length;
        states = new AiState[numStates];
    }
    public void RegisterState(AiState state)
    {
        int index = (int)state.GetID();
        states[index] = state;
    }
    public AiState GetState(AiStateID stateID)
    {
        int index = (int)stateID;
        return states[index];
    }
    public void Update()
    {
        GetState(currentState)?.Update(_agent);
    }
    public void ChangeState(AiStateID newState)
    {
        GetState(currentState)?.Exit(_agent);
        currentState = newState;
        GetState(currentState)?.Enter(_agent);
    }
}
