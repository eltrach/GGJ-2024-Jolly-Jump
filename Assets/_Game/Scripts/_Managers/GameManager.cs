using UnityEngine;
public class GameManager : Singleton<GameManager>
{

    #region variables
    public GameObject spawnedPlayer;

    [Header("Gameplay Elements")]
    public bool spawnPlayerOnStart;

    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    private bool isPaused;



    public bool CanPlay { get; internal set; }
    public Level CurrentLevel { get; set; }
    #endregion

    public void Init(Level generatedLevel = null)
    {
        CurrentLevel = generatedLevel;
        if(spawnPlayerOnStart) spawnedPlayer = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation, playerSpawnPoint);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Pause();
        }
    }
    public void NextLevel() => ResetLevel();
    public void ReloadLevel() => ResetLevel();

    private void ResetLevel()
    {

    }

    public void FindPlayer() => spawnedPlayer = GameObject.FindGameObjectWithTag("Player");

    public GameObject GetPlayer()
    {
        if (spawnedPlayer == null) FindPlayer();
        return spawnedPlayer;
    }
    public void Pause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void Reset()
    {
        if (spawnedPlayer != null) Destroy(spawnedPlayer);
    }
}
