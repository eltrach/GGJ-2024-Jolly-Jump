using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public bool showHiddenVars = false;
    [SerializeField, ShowIf("showHiddenVars")] Rigidbody[] _rigidBodies;
    [SerializeField, ShowIf("showHiddenVars")] Collider[] _colliders;
    [SerializeField, ShowIf("showHiddenVars")] Animator _animator;

    void Start()
    {
        Init();
    }
    private void Init()
    {
        _rigidBodies = GetComponentsInChildren<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        DeactivateRagdoll();
    }
    public void DeactivateRagdoll()
    {
        foreach (Rigidbody rigidBody in _rigidBodies)
        {
            rigidBody.isKinematic = true;
        }
        _animator.enabled = true;
    }
    [Button("Activate RAGDOLL")]
    public void ActivateRagdoll()
    {
        StartCoroutine(ActivateRag());
    }
    IEnumerator ActivateRag()
    {
        foreach (Rigidbody rigidBody in _rigidBodies)
        {
            rigidBody.isKinematic = false;
        }
        _animator.enabled = false;
        yield return new WaitForSeconds(0.5f);
        foreach (Collider collider in _colliders)
        {
            collider.isTrigger = true;
            Destroy(collider);
        }
        foreach (Rigidbody rigidBody in _rigidBodies)
        {
            rigidBody.isKinematic = true;
        }
    }
}
