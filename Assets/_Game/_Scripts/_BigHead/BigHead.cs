using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BigHead : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] float _maxHp;
    [Space]
    [SerializeField] Slider _hpSlider;
    float _hp;

    private void Start()
    {
        if (!_skinnedMeshRenderer)
            _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        _hp = _maxHp;
        _hpSlider.value = 1;
    }

    public void Hit(float damage)
    {
        _hp -= damage;
        float p = (_hp / _maxHp);
        _skinnedMeshRenderer.SetBlendShapeWeight(0, 100 - p * 100);
        _hpSlider.DOValue(p, 0.3f);

        if (_hp <= 0)
        {
            GlobalRoot.LevelManager.LoadNextLevel();
        }
    }
}
