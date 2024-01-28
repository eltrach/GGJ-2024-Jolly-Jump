using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class FadeOutText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float fadeDuration = 2f;
    public float moveDuration = 1.2f;
    public float moveDistance = -1500f;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(5f);
        textMeshPro.DOFade(0f, fadeDuration);
        MoveText();

    }

    private void MoveText()
    {
        textMeshPro.rectTransform.DOAnchorPosX(moveDistance, moveDuration)
            .OnComplete(() => Destroy(gameObject));
    }
}
