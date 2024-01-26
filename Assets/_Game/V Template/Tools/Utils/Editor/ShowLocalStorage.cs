using System.IO;
using UnityEngine;
using UnityEditor;


namespace MWM.PrototypeTemplate
{
    public class ShowLocalStorage
    {
        [MenuItem("Tools/Show local storage")]
        private static void OpenDirectory ()
        {
            string path = Application.persistentDataPath + "/game_event_iid.dat";
            if (!File.Exists(path))
                path = Application.persistentDataPath;
            EditorUtility.RevealInFinder(path);
        }
    }
}