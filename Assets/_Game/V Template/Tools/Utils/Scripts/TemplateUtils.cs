using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace VTemplate
{
    public static class TemplateUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="startRepeatingIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int GetModularIndexOfCollection (int index, int startRepeatingIndex, int length)
        {
            if (index < length)
                return index;
            return startRepeatingIndex + (index - startRepeatingIndex) % (length - startRepeatingIndex);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsPointerOverUI () 
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}