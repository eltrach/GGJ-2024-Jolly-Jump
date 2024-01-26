using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] private int _healToAdd;

    public int HealToAdd { get => _healToAdd; set => _healToAdd = value; }

    public void Heal(HealthSystem health)
    {
        health.TakeHeal(_healToAdd);
    }

}
