using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    [SerializeField] public float maxTime = 1f;
    [SerializeField] public float minDistance = 1f;
    [SerializeField] public float maxSightDistance = 10;

}
