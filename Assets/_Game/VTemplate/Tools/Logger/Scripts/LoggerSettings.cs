using UnityEngine;


namespace VTemplate
{
    public class LoggerSettings
    {
        public readonly string Tag;
        public readonly Color Color;



        public LoggerSettings (string tag)
        {
            Precondition.CheckNotNull(tag);
            Tag = tag;
            Color = Color.white;
        }
        
        public LoggerSettings (string tag, Color color)
        {
            Precondition.CheckNotNull(tag);
            Precondition.CheckNotNull(color);
            Tag = tag;
            Color = color;
        }
    }
}