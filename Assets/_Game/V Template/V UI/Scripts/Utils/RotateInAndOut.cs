using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInAndOut : MonoBehaviour
{
    public float duration;
    public Vector3 startRotation;
    public Vector3 endRotation;

    private void Start()
    {
        // Set the initial rotation
        transform.localRotation = Quaternion.Euler(startRotation);

        // Rotate from startRotation to endRotation and back in a loop
        Sequence rotationSequence = DOTween.Sequence();
        rotationSequence.Append(transform.DOLocalRotate(endRotation, duration));
        rotationSequence.Append(transform.DOLocalRotate(startRotation, duration));
        rotationSequence.SetLoops(-1);
    }
}
