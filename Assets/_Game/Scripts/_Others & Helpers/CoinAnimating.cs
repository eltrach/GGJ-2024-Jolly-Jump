using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinAnimating : MonoBehaviour
{
    [SerializeField] private RectTransform _targetPos;
    [SerializeField] private RectTransform _coin;
    [SerializeField] private float _transitionDuration;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private List<RectTransform> spawnedCoins = new List<RectTransform>();
    private bool _canDestory;

    [Button("Init Test : ")]
    public void Init(int toAdd)
    {
        var delay = 0f;
        for (int i = 0; i < toAdd; i++)
        {
            RectTransform coin = Instantiate(_coinPrefab , gameObject.transform).GetComponent<RectTransform>();
            coin.anchoredPosition = new Vector2(Random.Range(-250, 250), Random.Range(-250, 250));
            coin.DOAnchorPos(new Vector2(491.8556f, 782.1998f), _transitionDuration).SetDelay(delay).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                Destroy(coin.gameObject);
            });
            delay += 0.1f;
            // if(i == toAdd) _canDestory = true;
        }
        //Destroy(gameObject);
    }
}
