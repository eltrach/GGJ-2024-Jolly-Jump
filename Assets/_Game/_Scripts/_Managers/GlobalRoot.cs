using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

// main class singleton that responsible for all the managers (AKA MASTER)

public class GlobalRoot : MonoBehaviour
{
    public static GlobalRoot Instance;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private ThirdPersonUI playerUI;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private vDataManager dataManager;

    public static GameManager GameManager { get => Instance.gameManager; }
    public static UIManager UIManager { get => Instance.uIManager; }
    public static ThirdPersonUI PlayerUI => Instance.playerUI;
    public static LevelManager LevelManager { get => Instance.levelManager; }
    public static ShopManager ShopManager { get => Instance.shopManager; }
    public static vDataManager DataManager { get => Instance.dataManager; }
    public static bool IsLost { get => isLost; set => isLost = value; }
    public static bool IsWon { get => isWon; set => isWon = value; }

    static bool isLost = false;
    static bool isWon = false;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
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

    private void GameStart()
    {

    }
    [Button]
    public void GameWin()
    {
        if (IsWon || IsLost) return;
        Debug.Log("<color=cyan><b> GAME WIN </b></color>");
        IsWon = true;

        StartCoroutine(Win());
    }
    IEnumerator Win()
    {
        UIManager.LoadWinScreen();

        yield return new WaitForSeconds(5);
        LevelManager.LoadNextLevel();

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
