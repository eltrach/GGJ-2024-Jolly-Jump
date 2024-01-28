using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelManager : Singleton<LevelManager>
{
    int currentLevel = 0;


    [ShowInInspector]
    public int CurrentLevel
    {
        get
        {
            return PlayerPrefs.GetInt("currentLevel", 0);
        }
        set
        {
            PlayerPrefs.SetInt("currentLevel", value);
            PlayerPrefs.Save();
            currentLevel = value;
        }
    }

    public void LoadNextLevel()
    {
        CurrentLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
