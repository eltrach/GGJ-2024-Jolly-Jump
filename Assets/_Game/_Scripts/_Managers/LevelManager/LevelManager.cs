using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelManager : Singleton<LevelManager>
{
    int currentLevel = 0;

    [SerializeField]
    public int CurrentLevel
    {
        get
        {
            int value = PlayerPrefs.GetInt("currentLevel", 0);
            return value;
        }
        set
        {
            PlayerPrefs.GetInt("currentLevel", value);
            PlayerPrefs.Save();
            currentLevel = value;
        }
    }

    public void LoadNextLevel()
    {
        CurrentLevel++;
        SceneManager.LoadScene(CurrentLevel);
    }
}
