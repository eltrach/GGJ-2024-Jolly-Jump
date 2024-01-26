using Enemy;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using VTemplate.Controller;

public class HealthSystem : MonoBehaviour, IHealthSystem
{
    public int maxHealth = 100;
    [SerializeField, ProgressBar(0, "maxHealth", ColorGetter = "GetHealthBarColor"), ReadOnly] private int currentHealth = 100;
    [SerializeField] private bool isAI = true;
    [SerializeField] private bool isDead;

    public UnityEvent OnDeath;

    // getters and setters
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public bool IsAI { get => isAI; set => isAI = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    ThirdPersonUI ui;
    ThirdPersonVFX thirdPersonVFX;
    Ragdoll ragdoll;

    private void Start() => Init();
    private void Init()
    {
        thirdPersonVFX = GetComponent<ThirdPersonVFX>();
        ui = GetComponentInChildren<ThirdPersonUI>();
        ragdoll = GetComponent<Ragdoll>();

        currentHealth = maxHealth;
    }
    private void Update()
    {
#if UNITY_EDITOR 
        if (Input.GetKeyDown(KeyCode.I))
        {
            TakeDamage(50);
        }
#endif
    }
    public void TakeDamage(int amount, int damageMultiplier = 1, bool activeRagdoll = false)
    {
        if (currentHealth > 0) currentHealth -= amount;

        if (TryGetComponent(out ThirdPersonEnemy enemyAI)) enemyAI.HitByThePlayer();
        if (activeRagdoll && ragdoll) ragdoll.ActivateRagdoll();

        // if the health reaches 0 kill the Object (Player/Enemy/Obj)
        if (currentHealth <= 0)
        {
            Kill();
            OnDeath?.Invoke();
        }
        ui?.UpdateHealthBar();
        if (thirdPersonVFX) thirdPersonVFX.PlayDamageFX(); else { Debug.Log("PlayerVFX Var Is NULL"); }
    }

    private void Kill()
    {
        currentHealth = 0;

        // Enemy Or Player
        if (TryGetComponent(out ThirdPersonController player)) player.Die();
        else if (TryGetComponent(out ThirdPersonEnemy enemyAI)) enemyAI.Die();

        // Disable/Remove Components
        if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;
        ui?.UpdateHealthBar();
    }

    public void TakeHeal(int amount)
    {
        currentHealth += amount;
        thirdPersonVFX.PlayHealFX();
        if (currentHealth > maxHealth)
        {
            // Heal is Full
            currentHealth = maxHealth;
        }
        ui.UpdateHealthBar();
    }
    public void SetHealth(int health)
    {
        currentHealth = health;
    }
    public int GetHealth()
    {
        return currentHealth;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    private Color GetHealthBarColor(float value)
    {
        return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
    }
    public bool IsAlive()
    {
        return CurrentHealth < 0;
    }
}

