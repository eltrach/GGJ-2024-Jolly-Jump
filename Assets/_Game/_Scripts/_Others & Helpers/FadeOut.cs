using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private Renderer[] meshRenderer;
    //[SerializeField] private SkinnedMeshRenderer[] meshRenderer;
    [SerializeField] private float fadeDuration = 5;

    public void FadeOutStart()
    {
        foreach (var renderer in meshRenderer)
        {
            renderer.material.DOColor(Color.clear, fadeDuration);
        }
        gameObject.transform.DOScale(0, fadeDuration / 2);
    }

}
