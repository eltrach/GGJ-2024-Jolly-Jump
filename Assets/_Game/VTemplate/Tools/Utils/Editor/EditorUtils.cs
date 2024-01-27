using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MWM.PrototypeTemplate
{
    public static class EditorUtils
    {
        public static void SeparationLine(float top, float bottom)
        {
            GUILayout.Space(top);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(bottom);
        }

        public static void LabelCenter(string text)
        {
            GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            EditorGUILayout.LabelField(text, centeredStyle);
        }
        
        public static bool ButtonImage(Texture2D texture, Vector2 size)
        {
            GUIContent content = new GUIContent(texture);
            return GUILayout.Button(content, GUILayout.Width(size.x), GUILayout.Height(size.y));
        }
        
        // Notifications ------------------------------------------------------------------------
        public static void ShowNotification(string message, double time)
        {
            EditorWindow.focusedWindow.ShowNotification(new GUIContent(message), time);
        }
        public static void ShowNotification(EditorWindow window, string message, double time)
        {
            window.ShowNotification(new GUIContent(message), time);
        }
        public static void ShowNotification<T>(string message, double time) where T : EditorWindow
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(T), false);
            if (window) window.ShowNotification(new GUIContent(message), time);
        }
        public static void ShowNotification(string windowName, string message, double time)
        {
            EditorWindow window = EditorWindow.GetWindow<EditorWindow>(windowName, true);
            if (window) window.ShowNotification(new GUIContent(message), time);
        }

        // Object getter ------------------------------------------------------------------------
        
        public static T FindSceneObject<T>() where T : MonoBehaviour
        {
            GameObject[] objs = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject go in objs) {
                if (go.hideFlags != HideFlags.None)
                    continue;

                if (PrefabUtility.IsPartOfAnyPrefab(go))
                    continue;

                T obj = go.GetComponent<T>();
                if (obj != null) return obj;
            }
            return null;
        }
        
        public static T FindSceneObject<T>(Predicate<T> predicate) where T : MonoBehaviour
        {
            return FindSceneObjects<T>().Find(predicate);
        }
    
        public static List<T> FindSceneObjects<T>() where T : MonoBehaviour
        {
            List<T> objectsInScene = new List<T>();

            GameObject[] objs = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject go in objs) {
                if (go.hideFlags != HideFlags.None)
                    continue;

                if (PrefabUtility.IsPartOfAnyPrefab(go))
                    continue;

                T obj = go.GetComponent<T>();
                if (obj != null) objectsInScene.Add(obj);
            }

            return objectsInScene;
        }
        
        public static List<T> FindSceneObjects<T>(Predicate<T> predicate) where T : MonoBehaviour
        {
            return FindSceneObjects<T>().FindAll(predicate);
        }
    }
}