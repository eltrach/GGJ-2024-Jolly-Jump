using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelManager : Singleton<LevelManager>
{
    [ListDrawerSettings(ShowPaging = true, DraggableItems = true)]
    [InfoBox("List of available levels.")]
    public List<Level> levels;

    [FoldoutGroup("Current Level")]
    [Title("Current Level")]
    [ReadOnly]
    [InfoBox("The currently loaded level.")]
    public Level CurrentLevel;
    public Transform levelParent;


    [FoldoutGroup("Current Level")]
    [Title("Current Level Index")]
    [InfoBox("Getter and setter to handle level saving.")]
    public int CurrentLevelIndex
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentLevel", 0);
        }
        set
        {
            PlayerPrefs.SetInt("CurrentLevel", value);
            PlayerPrefs.Save();
        }
    }

    public void Init(out Level level)
    {
        level = SpawnLevel();
    }
    private Level SpawnLevel()
    {
        UIManager.Instance.UpdateLevelText(CurrentLevelIndex);
        if (CurrentLevelIndex < levels.Count - 1)
        {
            // regular Level 
            CurrentLevel = Instantiate(levels[CurrentLevelIndex], levelParent);
            return CurrentLevel;
        }
        else
        {
            // random Level
            int levelRandomIndex = UnityEngine.Random.Range(0, levels.Count);
            CurrentLevel = Instantiate(levels[levelRandomIndex], levelParent);
            return CurrentLevel;
        }
    }

    [Button(ButtonSizes.Large)]
    [FoldoutGroup("Level Management")]
    [InfoBox("Load the next level or a random level if no next level is available.")]
    public void NextLevel()
    {
        if (CurrentLevelIndex < levels.Count - 1)
        {
            CurrentLevelIndex++;
            Debug.Log("Next Level is : " + CurrentLevelIndex);
            UIManager.Instance.UpdateLevelText(CurrentLevelIndex);
            SaveCurrentLevelIndex();
            LoadCurrentLevel();
        }
        else
        {
            CurrentLevelIndex++;
            UIManager.Instance.UpdateLevelText(CurrentLevelIndex);
            SpawnRandomLevel();
        }
    }
    private void SpawnRandomLevel()
    {
        Debug.Log("Spawn Random Level!!");
        int levelRandomIndex = UnityEngine.Random.Range(0, levels.Count);
        CurrentLevel = Instantiate(levels[levelRandomIndex], levelParent);
        GlobalRoot.GameManager.CurrentLevel = CurrentLevel;
    }

    [Button(ButtonSizes.Large)]
    [FoldoutGroup("Level Management")]
    [InfoBox("Reload the specified level.")]
    public void ReloadLevel()
    {
        Debug.Log("LevelManager.ReloadLevl()");
        if (CurrentLevelIndex < levels.Count - 1)
        {
            SaveCurrentLevelIndex();
            LoadCurrentLevel();
        }
        else SpawnRandomLevel();

    }

    private void SaveCurrentLevelIndex()
    {
        PlayerPrefs.Save();
    }

    private void LoadCurrentLevel()
    {
        CurrentLevel = Instantiate(levels[CurrentLevelIndex], levelParent);
        GlobalRoot.GameManager.CurrentLevel = CurrentLevel;
    }
}
