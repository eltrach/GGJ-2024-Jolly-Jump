using Sirenix.OdinInspector;
using UnityEngine;
using VTemplate.Controller;

public class PlayerRoot : MonoBehaviour
{
    public static PlayerRoot Instance;

    [SerializeField] private ThirdPersonInputs _input;
    [SerializeField] private ThirdPersonMage _thirdPersonMage;
    [SerializeField] private ThirdPersonShooter _thirdPersonShooter;
    [SerializeField] private ThirdPersonMelee _thirdPersonMelee;
    [SerializeField] private ThirdPersonVFX _thirdPersonVFX;
    [SerializeField] private ThirdPersonController _thirdPersonController;
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private PlayerStateSystem _playerStateSystem;

    // ui
    [SerializeField] private ThirdPersonUI _thirdPersonUI;

    public static ThirdPersonInputs Input { get => Instance._input; set => Instance._input = value; }
    public static ThirdPersonMage ThirdPersonMage { get => Instance._thirdPersonMage; set => Instance._thirdPersonMage = value; }
    public static ThirdPersonShooter ThirdPersonShooter { get => Instance._thirdPersonShooter; set => Instance._thirdPersonShooter = value; }
    public static ThirdPersonMelee ThirdPersonMelee { get => Instance._thirdPersonMelee; set => Instance._thirdPersonMelee = value; }

    public static ThirdPersonVFX ThirdPersonVFX { get => Instance._thirdPersonVFX; set => Instance._thirdPersonVFX = value; }
    public static ThirdPersonController ThirdPersonController { get => Instance._thirdPersonController; set => Instance._thirdPersonController = value; }
    public static HealthSystem HealthSystem { get => Instance._healthSystem; set => Instance._healthSystem = value; }
    public static ThirdPersonUI ThirdPersonUI { get => Instance._thirdPersonUI; set => Instance._thirdPersonUI = value; }
    public static PlayerStateSystem StateSystem { get => Instance._playerStateSystem; set => Instance._playerStateSystem = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        LoadComponents();
    }
    [Button]
    private void LoadComponents()
    {
        _input = GetComponentInChildren<ThirdPersonInputs>();
        _thirdPersonController = GetComponentInChildren<ThirdPersonController>();
        _thirdPersonMage = GetComponentInChildren<ThirdPersonMage>();
        _healthSystem = GetComponentInChildren<HealthSystem>();

        _thirdPersonMelee = GetComponentInChildren<ThirdPersonMelee>();
        _thirdPersonShooter = GetComponentInChildren<ThirdPersonShooter>();
        _thirdPersonUI = GetComponentInChildren<ThirdPersonUI>();
        _thirdPersonVFX = GetComponentInChildren<ThirdPersonVFX>();
    }
}
