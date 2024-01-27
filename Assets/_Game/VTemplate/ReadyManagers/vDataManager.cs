using Sirenix.OdinInspector;
using UnityEngine;
using VTemplate;

#pragma warning disable IDE1006
public class vDataManager : MonoBehaviour
{
    private IDataManager _globalDataManager;

    [ShowInInspector]
    public int RandomSelectedShoesIndex
    {
        get => GetIntData(GameKeys.RandomSelectedShoesIndex);
        set => SetIntData(GameKeys.RandomSelectedShoesIndex, value);
    }

    [ShowInInspector]
    public int ShoesIndex
    {
        get => GetIntData(GameKeys.ShoesIndexKey);
        set => SetIntData(GameKeys.ShoesIndexKey, value);
    }

    [ShowInInspector]
    public float SubRewardProgression
    {
        get => GetFloatData(GameKeys.SubRewardProgression);
        set => SetFloatData(GameKeys.SubRewardProgression, value);
    }

    [ShowInInspector]
    public bool SubRewardEnded
    {
        get => GetBoolData(GameKeys.SubRewardEnded);
        set => SetBoolData(GameKeys.SubRewardEnded, value);
    }

    private void Start() => Init();

    private void Init()
    {
        _globalDataManager = new DataManagerImpl(GameKeys.GlobalDataManagerKey);
    }

    private int GetIntData(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    private void SetIntData(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    private float GetFloatData(string key, float defaultValue = 0f)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    private void SetFloatData(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    private bool GetBoolData(string key, bool defaultValue = false)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) != 0;
    }

    private void SetBoolData(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();
    }



    // removed temporary
#if false
    [Button]
    public void LoadData()
    {
        // Shoes Data
        LoadIntData(GameKeys.ShoesIndexKey, ref _shoesIndex);

        // Progression
        LoadIntData(GameKeys.SubRewardProgression, ref _subRewardProgression);
    }

    private void LoadIntData(string key, ref int targetVariable)
    {
        if (HasData(key))
        {
            object data = GetData(key);

            if (data is long v)
            {
                targetVariable = Convert.ToInt32(v);
            }
            else
            {
                Debug.LogError($"Invalid data type for {key}");
            }
        }
        else
        {
            Debug.Log($"{key} not found, setting the default value: ");
            SetData(key, 0);
        }
    }

    // it will return NULL if the data isn't found!!!!
    public object GetData(string key)
    {
        return _globalDataManager.GetData(key);
    }

    public bool HasData(string key)
    {
        return _globalDataManager.HasData(key);
    }

    [Button]
    public void SetData(string key, object newValue)
    {
        _globalDataManager.SetData(key, newValue);
        _globalDataManager.SaveData();
    }
#endif
}
