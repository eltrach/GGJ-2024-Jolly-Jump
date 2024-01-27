using System;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelManager : Singleton<LevelManager>
{
    int currentLevel;
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(currentLevel + 1);
    }
}
