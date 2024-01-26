using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// this class will hold everything about the player UI (HP bars , Stamina , whether to show the mobile inputs or not, many other use cases )
public class ThirdPersonUI : Singleton<ThirdPersonUI>
{
    public bool useMana;
    [Header("HP")]
    [SerializeField] private Slider hpBar;
    [SerializeField] private TextMeshProUGUI hpValueText;
    [Space]
    [Header("MANA"), ShowIf(nameof(useMana))]
    [SerializeField, ShowIf(nameof(useMana))] private Slider ManaBar;
    [SerializeField, ShowIf(nameof(useMana))] private TextMeshProUGUI ManaValueText;

    HealthSystem healthSystem;
    ThirdPersonMage thirdPersonMage;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // getComponents (components exist in the parent gameobject )
        healthSystem = GetComponentInParent<HealthSystem>();
        if (useMana)
        {
            thirdPersonMage = GetComponentInParent<ThirdPersonMage>();
            ManaBar.maxValue = thirdPersonMage.maxMana;
        }
        hpBar.maxValue = healthSystem.maxHealth;
        UpdateHealthBar();
        UpdateSliders();
    }
    public void UpdateSliders()
    {
        UpdateHealthBar();
        UpdateManaBar();
    }
    public void UpdateHealthBar()
    {
        hpBar.value = healthSystem.CurrentHealth;
        if (hpValueText) hpValueText.text = healthSystem.CurrentHealth.ToString() + " %";
    }
    public void UpdateManaBar()
    {
        if (!useMana) return;
        ManaBar.value = thirdPersonMage.CurrentMana;
        ManaValueText.text = thirdPersonMage.CurrentMana.ToString("F0") + " %";
    }
}
