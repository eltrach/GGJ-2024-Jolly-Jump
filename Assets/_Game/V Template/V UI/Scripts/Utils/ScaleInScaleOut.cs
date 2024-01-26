using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


public class ScaleInScaleOut : MonoBehaviour
{
    [Title("Important")]
    public bool isStartBtn = true;
    
    Button button;
    public float scaleDuration = 1.0f;
    public float scaleAmount = 1.2f;
    public Ease startEase = Ease.Linear;
    public Ease endEase = Ease.Linear;

    Vector3 initScale;

    private void Start()
    {
        initScale = transform.localScale;
        button = GetComponent<Button>();
        // Start the scaling animation
        ScaleRoutine();
        if (button && isStartBtn) button.onClick.AddListener(ButtonClicked);
    }
    private void ButtonClicked()
    {
        gameObject.SetActive(false);
    }
    private void ScaleRoutine()
    {
        // Scale in
        transform.DOScale(Vector3.one * scaleAmount, scaleDuration).SetEase(startEase).SetLoops(-1, LoopType.Yoyo);
    }
}
