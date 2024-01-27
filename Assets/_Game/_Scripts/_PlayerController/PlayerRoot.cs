using Sirenix.OdinInspector;
using UnityEngine;
using VTemplate.Controller;

public class PlayerRoot : MonoBehaviour
{
    public static PlayerRoot Instance;

    [SerializeField] private ThirdPersonInputs _input;
    [SerializeField] private ThirdPersonVFX _thirdPersonVFX;
    [SerializeField] private ThirdPersonController _thirdPersonController;
    [SerializeField] private ThirdPersonAiming _thirdPersonAiming;

    // ui
    [SerializeField] private ThirdPersonUI _thirdPersonUI;

    public static ThirdPersonInputs Input { get => Instance._input; set => Instance._input = value; }
    public static ThirdPersonAiming ThirdPersonAiming { get => Instance._thirdPersonAiming; set => Instance._thirdPersonAiming = value; }
    public static ThirdPersonVFX ThirdPersonVFX { get => Instance._thirdPersonVFX; set => Instance._thirdPersonVFX = value; }
    public static ThirdPersonController ThirdPersonController { get => Instance._thirdPersonController; set => Instance._thirdPersonController = value; }
    public static ThirdPersonUI ThirdPersonUI { get => Instance._thirdPersonUI; set => Instance._thirdPersonUI = value; }

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
        _thirdPersonUI = GetComponentInChildren<ThirdPersonUI>();
        _thirdPersonVFX = GetComponentInChildren<ThirdPersonVFX>();
    }
}
