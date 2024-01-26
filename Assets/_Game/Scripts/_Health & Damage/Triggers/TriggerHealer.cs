using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TriggerHealer : Healer
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out HealthSystem health))
        {
            Heal(health);
            gameObject.SetActive(false);
        }
    }
}
