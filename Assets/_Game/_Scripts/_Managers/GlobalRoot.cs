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

    static bool win = false;
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
        if (win) return;
        Debug.Log("<color=cyan><b> GAME WIN </b></color>");
        StartCoroutine(Win());
    }
    IEnumerator Win()
    {
        win = true;
        UIManager.LoadWinScreen();

        yield return new WaitForSeconds(5);
        LevelManager.LoadNextLevel();

    }
    public void ReloadLevel()
    {
        if (!win)
        {
            Debug.Log("<color=cyan><b> RELOAD LEVEL </b></color>");
            StartCoroutine(Reload());
        }
    }
    IEnumerator Reload()
    {
        UIManager.LoadLoseScreen();
        yield return new WaitForSeconds(5);
        LevelManager.Reload();
        win = false;
    }
}
