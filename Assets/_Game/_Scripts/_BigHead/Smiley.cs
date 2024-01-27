using DG.Tweening;
using UnityEngine;

public class Smiley : MonoBehaviour
{
    [SerializeField] GameObject[] _emojisType;
    [SerializeField] int value = 1;
    [Space]
    [SerializeField] GameObject _rootObj;
    [SerializeField] GameObject _fxHideObj;
    [SerializeField] GameObject _fxCollectObj;

    bool _canCollect = true;

    public bool canCollect => _canCollect;

    int _indexImoji;

    private void Start()
    {
        foreach (GameObject emoji in _emojisType)
            emoji.SetActive(false);

        _indexImoji = Random.Range(0, _emojisType.Length);
        _emojisType[_indexImoji].SetActive(true);

        _canCollect = true;

        _rootObj.SetActive(true);
        _fxHideObj.SetActive(false);
        _fxCollectObj.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    void Collect()
    {
        if (canCollect)
        {
            PlayerRoot.ThirdPersonAiming.IncreaseShots(value);

            _rootObj.SetActive(false);
            _fxCollectObj.SetActive(true);

            Destroy(gameObject, 1.22f);

            _canCollect = false;
        }
    }

    public void Hide()
    {
        if (canCollect)
        {
            _canCollect = false;

            _rootObj.transform.DOScale(0, 0.32f).OnComplete(() =>
            {
                _rootObj.SetActive(false);
                _fxHideObj.SetActive(true);

                Destroy(gameObject, 1.22f);
            });
        }
    }
}
