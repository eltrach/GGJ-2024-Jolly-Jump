using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigHead : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] float _maxHp;

    float _hp;

    private void Start()
    {
        if(!_skinnedMeshRenderer)
            _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        _hp = _maxHp;
    }

    public void Hit(float damage)
    {
        _hp -= damage;
        _skinnedMeshRenderer.SetBlendShapeWeight(0, 100 - (_hp / _maxHp) * 100);
    }
}
