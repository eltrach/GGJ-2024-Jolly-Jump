using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ThirdPersonAiming : MonoBehaviour
{
    private ThirdPersonInputs _input;
    [SerializeField] private TwoBoneIKConstraint twoBoneIKConstraint;

    public int availableShots = 30;
    public Transform aimingPositionObj;
    public Transform shotPositionObj;

    public float aimToShotDuration = 0.3f;

    bool _isAiming;
    bool _canShot;
    bool _isShot;

    private void Start()
    {
        _input = GetComponent<ThirdPersonInputs>();
    }

    private void Update()
    {
        _isAiming = _input.Aim;
        _isShot = _input.Shot;
        if (_isAiming)
        {
            print(" --> AIMING");
            _canShot = true;
            if (!_isShot)
                //twoBoneIKConstraint.data.target.transform.position = aimingPositionObj.position;
                twoBoneIKConstraint.data.target.transform.DOLocalMove(aimingPositionObj.position, aimToShotDuration);

            if (_isShot && availableShots > 0)
            {
                print("---> SHOOT");
                availableShots--;
                twoBoneIKConstraint.data.target.transform.DOLocalMove(shotPositionObj.position, aimToShotDuration);
            }
        }
    }
    public void IncreaseShots(int inceaseBy)
    {
        availableShots += inceaseBy;
    }
}
