using Sirenix.OdinInspector;
using UnityEngine;

public enum GameState
{
    Idle,
    Melee,
    InVehicle,
    Shooter,
    Building
}
public class PlayerStateSystem : Singleton<PlayerStateSystem>
{
    public GameState currentState;
    [ShowInInspector]
    public GameState GetCurrentState => currentState;
    private void Start()
    {
        currentState = GameState.Idle;
    }
    public void SwitchState(GameState newState)
    {
        currentState = newState;
        // Additional actions when switching states (if needed)
        Debug.Log($"Switched to {currentState} state".Bold().Color("green"));
    }
    public void ResetGameState()
    {
        currentState = GameState.Idle;
        Debug.Log("Game state reset to idle");
    }
}
