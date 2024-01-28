using DG.Tweening;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private Renderer[] meshRenderer;
    [SerializeField] private float fadeDuration = 5;
    [SerializeField] private float scaleDuration = 5;


    private void Start()
    {
        meshRenderer = GetComponentsInChildren<Renderer>();
    }
    public void FadeOutStart()
    {
        foreach (Renderer renderer in meshRenderer)
        {
            renderer.material.DOColor(Color.clear, fadeDuration);
        }
        gameObject.transform.DOScale(0, scaleDuration).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

}
