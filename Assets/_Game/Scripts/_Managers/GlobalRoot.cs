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
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private vDataManager dataManager;


    // gets
    public static GameManager GameManager { get => _instance.gameManager; }
    public static UIManager UIManager { get => _instance.uIManager; }
    public static ThirdPersonUI PlayerUI => _instance.playerUI;
    public static LevelManager LevelManager { get => _instance.levelManager; }
    public static CurrencyManager CurrencyManager { get => _instance.currencyManager; }
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
        //DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        Debug.Log("<color=cyan><b> GAME START </b></color>");
        //
        IsWon = false;
        IsLost = false;
        //
        if (isLevelingGame)
        {
            levelManager.Init(out Level GeneratedLevel);
            gameManager.Init(GeneratedLevel);
        }
        else
        {
            gameManager.Init();
        }
        ShopManager.Init();
        uIManager.Init();

        //UpdateSubReward();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameStarted)
        {
            GameStarted = true;
            GameStart();
        }
    }

    private void GameStart()
    {
        uIManager.GameStart();
    }
    [Button]
    public static void GameWin()
    {
        if (IsWon || IsLost) return;
        Debug.Log("<color=cyan><b> GAME WIN </b></color>");
        IsWon = true;
        UIManager.GameWin();
        //UpdateSubReward();
    }

    public static void GameLose()
    {
        if (IsLost) return;
        Debug.Log("<color=cyan><b> GAME LOSE </b></color>");
        IsLost = true;
        UIManager.GameLose();
    }
    // Level Manager
    public static void NextLevel()
    {
        Debug.Log("<color=cyan><b> NEXT LEVEL </b></color>");
        Reset();
        LevelManager.Instance.NextLevel();
        GameManager.NextLevel();
        AddWinReward();
        UIManager.UpdateCoinsUI();
    }
    public static void ReloadLevel()
    {
        Debug.Log("<color=cyan><b> RELOAD LEVEL </b></color>");
        Reset();
        LevelManager.ReloadLevel();
        GameManager.ReloadLevel();
    }
    // currency manager manipilation 
    public static void AddWinReward()
    {
        CurrencyManager.AddCoins(100);
    }
    private static void Reset()
    {
        IsWon = false;
        IsLost = false;
    }
}
