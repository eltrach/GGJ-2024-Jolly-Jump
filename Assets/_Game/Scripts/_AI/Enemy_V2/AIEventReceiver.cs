using Sirenix.OdinInspector;
using UnityEngine;

public class AIEventReceiver : MonoBehaviour
{
    [SerializeField] private CapsuleCollider lHandDamage;
    [SerializeField] private CapsuleCollider rHandDamage;

    private void Start()
    {
        lHandDamage.enabled = false;
        rHandDamage.enabled = false;
    }
    public void ActivateDamage()
    {
        lHandDamage.enabled = true;
    }
    public void DeactivateDamage()
    {
        lHandDamage.enabled = false;
    }
}
