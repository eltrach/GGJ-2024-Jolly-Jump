using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileySpanPoint : MonoBehaviour
{
    [System.NonSerialized] internal GameObject smileyObj;

    public bool IsFree => smileyObj;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
