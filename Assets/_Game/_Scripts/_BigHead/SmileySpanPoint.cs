using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileySpanPoint : MonoBehaviour
{
    [System.NonSerialized] internal Smiley smileyObj;

    public bool IsFree => !smileyObj || !smileyObj.canCollect;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
