using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial interface IHealthSystem
{
    bool IsAlive();
    void SetHealth(int health);
    int GetHealth();
    int GetMaxHealth();
    void TakeDamage(int amount, int damageMultiplier = 1, bool activeRagdoll = false);
}
