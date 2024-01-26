using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationMarkTween : MonoBehaviour
{
    // The initial scale of the text
    public Vector3 initialScale;

    // The final scale of the text
    public Vector3 finalScale;

    // The duration of one rotation tween
    public float rotationDuration = 1f;

    // The duration of one scale tween
    public float scaleDuration = 0.5f;

    // The number of loops for the rotation tween
    public int rotationLoops = -1; // -1 means infinite loops

    // The number of loops for the scale tween
    public int scaleLoops = -1; // -1 means infinite loops

    // The ease type for the rotation tween
    public Ease rotationEase = Ease.Linear;

    // The ease type for the scale tween
    public Ease scaleEase = Ease.InOutSine;

    void Start()
    {
        Transform obj = gameObject.transform;

        // Set the initial and final scale values
        initialScale = obj?.transform.localScale ?? Vector3.one;
        //finalScale = initialScale * 2f;

        //// Create a rotation tween that rotates the text 360 degrees around the Z axis
        //Tween rotationTween = obj!.transform.DOLocalRotate(new(0f, 0f, 23f), rotationDuration)
        //    .SetEase(rotationEase)
        //    .SetLoops(rotationLoops, LoopType.Yoyo);

        // Create a scale tween that scales the text up and down between the initial and final scale values
        Tween scaleTween = obj!.transform.DOScale(finalScale, scaleDuration)
            .SetEase(scaleEase)
            .SetLoops(scaleLoops, LoopType.Yoyo);

        // Create a sequence that plays both tweens simultaneously
        Sequence sequence = DOTween.Sequence()
            //.Join(rotationTween)
            .Join(scaleTween);

        // Play the sequence
        sequence.Play();
    }
}
