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
    [Space]
    [SerializeField] Transform _head;
    [SerializeField] Transform _eyes1, _eyes2;

    [SerializeField] Transform _target;

    private void Start()
    {
        if (!_skinnedMeshRenderer)
            _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        _hp = _maxHp;
        GetComponentInChildren<Animation>().Play();
        _hpSlider.value = 1;
    }

    private void Update()
    {
        if (_target)
        {
            FlowTarget(_head, 0.6f);
            FlowTarget(_eyes1, 50);
            FlowTarget(_eyes2, 50);
        }
        else
        {
            _target = PlayerRoot.ThirdPersonController.transform;
        }
    }

    private void FlowTarget(Transform folower, float speed)
    {
        Vector3 deff = _target.position - folower.position;
        Vector3 normal = deff.normalized;

        folower.forward = Vector3.RotateTowards(folower.forward, normal, speed * Time.deltaTime, 1);
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
