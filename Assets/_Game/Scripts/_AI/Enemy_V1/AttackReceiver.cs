using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackReceiver : MonoBehaviour
{
    public List<HitBox> hitBoxes;

    [SerializeField] HealthSystem _health;
    public ThirdPersonMelee thirdPersonMelee;
    public bool canApplyDamage;


    public UnityEvent onEnableDamage;
    public UnityEvent onDisableDamage;



    void Start()
    {
        Init();
    }
    private void Init()
    {
        _health = GetComponentInParent<HealthSystem>();
        thirdPersonMelee = GetComponentInParent<ThirdPersonMelee>();

        if (hitBoxes.Count > 0)
        {
            // initialize hitBox properties
            foreach (HitBox hitBox in hitBoxes)
            {
                hitBox.attackObject = this;
            }
        }
        else
        {
            this.enabled = false;
        }

    }
    public void OnRaycastHit(BulletProjectile bullet)
    {
        _health.TakeDamage(bullet.damage);
        Debug.Log("Current Health :" + _health.CurrentHealth);
    }

    public void SetActiveDamage(bool active, int damageMultiplier, int recoilID, bool ignoreDefense, bool activeRagdoll)
    {
        if (thirdPersonMelee == null) thirdPersonMelee = GetComponentInParent<ThirdPersonMelee>();
       
        canApplyDamage = active;
        for (int i = 0; i < hitBoxes.Count; i++)
        {
            var hitCollider = hitBoxes[i];
            hitCollider.trigger.enabled = active;            
        }

        if (active) onEnableDamage.Invoke();
        else onDisableDamage.Invoke();
        }

    public void OnHit(HitBox hitBox, Collider other)
    {
        Debug.Log("HIT : " + other.gameObject);
        // here will deal the damage:
        HealthSystem health = other.GetComponent<HealthSystem>();
        health.TakeDamage(thirdPersonMelee.attackDamage);
    }
}
