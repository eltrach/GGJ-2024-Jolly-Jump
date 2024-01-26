using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ThirdPersonAiming : MonoBehaviour
{
    private ThirdPersonInputs _input;
    [SerializeField] private TwoBoneIKConstraint twoBoneIKConstraint;

    public int availableShots = 30;
    public Transform idlePositionObj;
    public Transform aimingPositionObj;
    public Transform shotPositionObj;


    public float shotTimeOut;
    public float aimToShotDuration = 0.3f;
    public float lerpSpeed = 5f;

    bool _isAiming;
    bool _canShot;
    bool _isShot;

    private void Start()
    {
        _input = GetComponent<ThirdPersonInputs>();
        twoBoneIKConstraint.data.target.transform.position = idlePositionObj.position;
    }

    private void Update()
    {
        _isAiming = _input.Aim;
        _isShot = _input.Shot;
        if (_isAiming)
        {
            ChangeHandWeight(1);
            print(" --> AIMING");
            _canShot = true;
            if (!_isShot)
                twoBoneIKConstraint.data.target.transform.DOLocalMove(aimingPositionObj.localPosition, aimToShotDuration);

            if (_isShot && availableShots > 0)
            {
                print("---> SHOOT");
                availableShots--;
                //twoBoneIKConstraint.data.S
                twoBoneIKConstraint.data.target.transform.DOLocalMove(shotPositionObj.localPosition, aimToShotDuration);
            }
        }
        else
        {
            ChangeHandWeight(0);
        }
    }
    public void IncreaseShots(int inceaseBy)
    {
        availableShots += inceaseBy;
    }
    public void ChangeHandWeight(float targetWeight)
    {
        twoBoneIKConstraint.weight = Mathf.Lerp(twoBoneIKConstraint.weight, targetWeight, lerpSpeed * Time.deltaTime);
    }
}
