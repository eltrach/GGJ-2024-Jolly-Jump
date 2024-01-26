#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ClearLocalStorage
{
    /// <summary>
    /// This method clears all known local storage elements
    /// - PlayerPrefs
    /// - Persistent Data path
    /// - Cache data path
    /// </summary>
    [MenuItem("V Template/Tools/Clear local storage")]
    private static void ClearStorage()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Directory.Delete(Application.persistentDataPath, true);
        Directory.Delete(Application.temporaryCachePath, true);
    }
    [MenuItem("V Template/Tools/Open Save Path")]
    private static void ShowSavePath()
    {
        Application.OpenURL(Application.persistentDataPath);
        //Application.OpenURL(Application.temporaryCachePath);
    }
}
#endif