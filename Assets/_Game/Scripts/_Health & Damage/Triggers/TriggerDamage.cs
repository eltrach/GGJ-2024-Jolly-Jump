using UnityEngine;

public class TriggerDamage : Damager
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out HealthSystem health))
        {
            if (health.IsAI) return;
            Damage(health);
        }
    }

}
