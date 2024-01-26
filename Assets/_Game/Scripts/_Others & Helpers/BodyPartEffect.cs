using DG.Tweening;
using UnityEngine;

public class BodyPartEffect : MonoBehaviour
{
    [SerializeField] private float _transitionDuration;
    [SerializeField] private float _jumpPower = 3.1f;

    [SerializeField] private float _destroyOnWinDelay = 0.2f; // set it to 0 to destory immediate

    public float TransitionDuration { get => _transitionDuration; set => _transitionDuration = value; }
    private void OnEnable()
    {
        //EventsManager.OnGameWin += DestroyOnWin;
    }
    private void OnDisable()
    {
        //EventsManager.OnGameWin -= DestroyOnWin;
    }
    public void Init(GameObject _target)
    {
        transform.DOJump(_target.transform.position, _jumpPower, 1, TransitionDuration).SetEase(Ease.OutQuad);
    }
    void DestroyOnWin()
    {
        Destroy(gameObject, _destroyOnWinDelay);
    }
}
