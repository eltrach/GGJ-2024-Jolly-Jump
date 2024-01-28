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
    [Space]
    [SerializeField] Renderer _flagRendeer;
    [SerializeField] Texture[] _flagTextures;
    private void Start()
    {
        if (!_skinnedMeshRenderer)
            _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        _hp = _maxHp;
        GetComponentInChildren<Animation>().Play();
        _hpSlider.value = 0;

        _flagRendeer.material.mainTexture = _flagTextures[Random.Range(0, _flagTextures.Length)];
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
        float p = (_maxHp != 0) ? (_hp / _maxHp) : 0;
        float blendShapeValue = Mathf.Clamp((_hp / _maxHp) * 100, 0, 100);

        DOVirtual.Float(_skinnedMeshRenderer.GetBlendShapeWeight(0), 100f, 0.3f, value =>
        {
            _skinnedMeshRenderer.SetBlendShapeWeight(0, value);
        });

        _hpSlider.DOValue(1 - p, 0.3f);

        if (_hp <= 0)
        {
            GlobalRoot.Instance.GameWin();
        }
    }

}
