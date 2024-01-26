using UnityEngine;

public static class vUtility
{
    public static Color GetHealthBarColor(float value)
    {
        return UnityEngine.Color.Lerp(UnityEngine.Color.red, UnityEngine.Color.green, Mathf.Pow(value / 100f, 2));
    }

    public static string Bold(this string str) => "<b>" + str + "</b>";
    public static string Color(this string str, string clr) => string.Format("<color={0}>{1}</color>", clr, str);
    public static string Italic(this string str) => "<i>" + str + "</i>";
    public static string Size(this string str, int size) => string.Format("<size={0}>{1}</size>", size, str);

}
