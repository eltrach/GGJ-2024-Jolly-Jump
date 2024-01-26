using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private int _damage;
    public int pdamage { get => _damage; set => _damage = value; }
    public void Damage(HealthSystem health)
    {
        if (pdamage > 0) health.TakeDamage(pdamage);
    }
}
