using Sirenix.OdinInspector;
using UnityEngine;

// main class singleton that responsible for all the managers (AKA MASTER)

public class GlobalRoot : MonoBehaviour
{
    private static GlobalRoot _instance;
    public bool isLevelingGame;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private ThirdPersonUI playerUI;
    [SerializeField, ShowIf(nameof(isLevelingGame))] private LevelManager levelManager;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private vDataManager dataManager;

    public static GameManager GameManager { get => _instance.gameManager; }
    public static UIManager UIManager { get => _instance.uIManager; }
    public static ThirdPersonUI PlayerUI => _instance.playerUI;
    public static LevelManager LevelManager { get => _instance.levelManager; }
    public static ShopManager ShopManager { get => _instance.shopManager; }
    public static vDataManager DataManager { get => _instance.dataManager; }
    public static bool IsLost { get => isLost; set => isLost = value; }
    public static bool IsWon { get => isWon; set => isWon = value; }

    static bool isLost = false;
    static bool isWon = false;
    bool GameStarted = false;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        Debug.Log("<color=cyan><b> GAME START </b></color>");
    }
    private void Update()
    {

    }

    private void GameStart()
    {

    }
    [Button]
    public static void GameWin()
    {
        if (IsWon || IsLost) return;
        Debug.Log("<color=cyan><b> GAME WIN </b></color>");
        IsWon = true;


    }

    public static void GameLose()
    {
        if (IsLost) return;
        Debug.Log("<color=cyan><b> GAME LOSE </b></color>");
        IsLost = true;
    }
    // Level Manager
    public static void NextLevel()
    {
        Debug.Log("<color=cyan><b> NEXT LEVEL </b></color>");
        Reset();

    }
    public static void ReloadLevel()
    {
        Debug.Log("<color=cyan><b> RELOAD LEVEL </b></color>");
        Reset();

    }
    private static void Reset()
    {
        IsWon = false;
        IsLost = false;
    }
}
