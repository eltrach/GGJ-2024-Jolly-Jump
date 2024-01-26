using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using ThirdPersonController = VTemplate.Controller.ThirdPersonController;

public class ThirdPersonShooter : MonoBehaviour
{
    [TabGroup("Shooting")] public bool canShotWithoutAim = true;
    [Tooltip("if false the player will not sprint while shooting and vice versa")]
    [TabGroup("Shooting")] public bool canShotWhileSprinting = true;
    [TabGroup("Shooting")] public bool onlyWalkWhenAiming = true;

    [TabGroup("Weapon")]
    [Header("Weapon")]
    public Weapon rightArmWeapon;
    [TabGroup("Weapon")] public Weapon leftArmWeapon;
    [TabGroup("Weapon")] public Transform spawnBulletPosition;
    [TabGroup("Weapon")] public Transform debugTransform;

    [TabGroup("Global IK")]
    public Rig rig;

    // right Hand:
    [Header("Right Hand")]
    [TabGroup("Global IK")] public Transform globalRightHandIKTarget;
    [TabGroup("Global IK")] public Transform globalRightHandIKHint;

    // left Hand 
    [Header("Left Hand")]
    [TabGroup("Global IK")] public Transform globalLeftHandIKTarget;
    [TabGroup("Global IK")] public Transform globalLeftHandIKHint;


    [TabGroup("Scope"), Tooltip("The time before closing the scope : ")] public bool enableHipfire;
    [TabGroup("Scope")] public float hipfireDuration = 0.2f;
    [TabGroup("Scope"), ReadOnly, ShowInInspector] float _initialFieldOfView;
    [TabGroup("Scope")] public float targetFieldOfView = 50f;

    [TabGroup("Camera"), Tooltip("zoom duration in seconds")]
    public float zoomDuration = 0.5f;
    [TabGroup("Camera"), Header("Sensitivity")] public float normalSensitivity = 3;
    [TabGroup("Camera")] public float aimSensitivity = 2;


    [TabGroup("Components")] public CinemachineVirtualCamera _tppCamera;
    [TabGroup("Components")] public CinemachineVirtualCamera _tppAimCamera;
    [TabGroup("Components")] public Animator _animator;
    [TabGroup("Components")] public ThirdPersonController _player;
    [TabGroup("Components")] public ThirdPersonInputs _input;

    [HideInInspector] public bool _autoShot = true;
    [HideInInspector] public LayerMask aimColliderLayerMask = new();
    [HideInInspector] public float autoAimDistance = 10f;
    [HideInInspector] public LayerMask targetLayer = new();
    float aimRigWeight;
    bool isAiming;
    bool _canShot;
    FireableTarget fireableTarget;

    int aimLayer;

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        _initialFieldOfView = _tppCamera.m_Lens.FieldOfView;
        _player = GetComponent<ThirdPersonController>();
        _input = GetComponent<ThirdPersonInputs>();
        _animator = GetComponent<Animator>();

        // get layer
        aimLayer = _animator.GetLayerIndex("AimingLayer");
        if (rightArmWeapon) GetWeaponIK();
    }

    private void Update()
    {
        ShootingLogic();
        if (_autoShot) AutoShot();
        if (rig) rig.weight = Mathf.Lerp(rig.weight, aimRigWeight, Time.deltaTime * 20f);
        else Debug.LogWarning("Note : ---> Please Setup a Rig");
    }

    #endregion

    #region Shooting Logic
    private void ShootingLogic()
    {
        //Debug.Log("HandleAim();");
        if (PlayerRoot.HealthSystem.IsDead) return;
        Vector3 mouseWorldPosition = Vector3.zero;
        Transform hitTransform = null;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f); // screen centre point
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastInfo, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastInfo.point;
            mouseWorldPosition = raycastInfo.point;
            hitTransform = raycastInfo.transform;
        }
        if (_input.HoldToAim)
        {
            if (_input.Aim) isAiming = _input.Aim;
            else isAiming = false;
        }
        else
        {
            if (_input.Aim) isAiming = !isAiming;
        }

        if (isAiming) AimingON(mouseWorldPosition);
        else AimingOFF();

        if (_input.Shot)
        {
            Debug.Log("Shoot");
            OpenFire(mouseWorldPosition, raycastInfo);
        }
        else if (_canShot && _autoShot && fireableTarget != null)
        {
            if (!fireableTarget) return;
            Debug.Log("-> Auto Shooting!");
            OpenFire(mouseWorldPosition, raycastInfo);
        }
    }
    private void OpenFire(Vector3 mouseWorldPosition, RaycastHit raycastInfo)
    {
        _player.SetRotateOnMove(false);
        AdjustRotation(mouseWorldPosition);
        _animator.SetLayerWeight(aimLayer, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, Time.deltaTime * 13f));
        aimRigWeight = 1;

        StopCoroutine(Shoot(mouseWorldPosition, raycastInfo));
        StartCoroutine(Shoot(mouseWorldPosition, raycastInfo));
    }
    IEnumerator Shoot(Vector3 mouseWorldPosition, RaycastHit raycastHit)
    {
        Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        //Instantiate(weapon.projectile, weapon.muzzle.position, Quaternion.LookRotation(aimDir, Vector3.up));
        yield return new WaitForSeconds(0.1f);
        rightArmWeapon.Shoot(mouseWorldPosition, null, null);
        if (leftArmWeapon) leftArmWeapon.Shoot(aimDir, null, null);
        SpawnDecals(raycastHit);
    }
    #endregion

    #region Aiming

    private void AimingON(Vector3 mouseWorldPosition)
    {
        // fov 
        DOTween.To(() => _tppCamera.m_Lens.FieldOfView, x => _tppCamera.m_Lens.FieldOfView = x, targetFieldOfView, zoomDuration);
        _player.SetSensitivity(aimSensitivity);
        _tppAimCamera.gameObject.SetActive(true);
        _player.SetRotateOnMove(false);

        DOTween.To(() => _animator.GetLayerWeight(aimLayer), x => _animator.SetLayerWeight(aimLayer, x), 1f, zoomDuration);
        aimRigWeight = 1;

        AdjustRotation(mouseWorldPosition);
    }

    private void AimingOFF()
    {
        DOTween.To(() => _tppCamera.m_Lens.FieldOfView, x => _tppCamera.m_Lens.FieldOfView = x, _initialFieldOfView, zoomDuration);
        _player.SetSensitivity(normalSensitivity);
        _tppAimCamera.gameObject.SetActive(false);

        DOTween.To(() => _animator.GetLayerWeight(aimLayer), x => _animator.SetLayerWeight(aimLayer, x), 0f, zoomDuration);
        aimRigWeight = 0;
        isAiming = false;
        _player.SetRotateOnMove(true);
    }
    #endregion

    #region Utils
    private void AdjustRotation(Vector3 mouseWorldPosition)
    {
        // adjust the player Rotation
        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
    }
    public Weapon GetEquippedWeapon()
    {
        if (rightArmWeapon != null)
        {
            return rightArmWeapon;
        }
        else return null;
    }
    private void AutoShot()
    {
        // check if there is a target in sight
        Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f); // screen centre point
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        _canShot = Physics.Raycast(ray, out RaycastHit hitInfo, 999f, targetLayer);
        if (_canShot)
        {
            if (hitInfo.collider.TryGetComponent(out fireableTarget))
            {
                Debug.Log("<color=red> Target Founded </color>" + fireableTarget.name);
                ShootingLogic();
            }
        }
    }
    [Button]
    private void GetWeaponIK()
    {
        Weapon weapon = GetEquippedWeapon();
        if (weapon != null)
        {
            // Target 
            // Right Hand
            Debug.Log("weapon value" + weapon.rightHandIKTarget.localPosition);

            globalRightHandIKTarget.localPosition = weapon.rightHandIKTarget.localPosition;
            globalRightHandIKTarget.localEulerAngles = weapon.rightHandIKTarget.localEulerAngles;

            //globalRightHandIKTarget.localPosition = weapon.rightHandIKTargetPosition;
            //globalRightHandIKTarget.localEulerAngles = weapon.rightHandIKTargetEulerAngles;

            Debug.Log("Setted Values" + globalRightHandIKTarget.eulerAngles);
            // Left Hand
            //globalLeftHandIKTarget.localPosition = weapon.leftHandIKTargetPosition; 
            //globalLeftHandIKTarget.rotation = Quaternion.LookRotation(weapon.leftHandIKTargetRotation);

            // Hint
            // Right Hand
            //globalRightHandIKHint.localPosition = weapon.rightHandIKHintPosition;
            //globalRightHandIKHint.rotation = Quaternion.LookRotation(weapon.rightHandIKHintRotation);

            // Left Hand
            //globalLeftHandIKHint.localPosition = weapon.leftHandIKHintPosition;
            //globalLeftHandIKHint.rotation = Quaternion.LookRotation(weapon.leftHandIKHintRotation);
        }
    }

    #endregion

    #region Decals
    void SpawnDecals(RaycastHit raycastInfo)
    {
        if (rightArmWeapon.projectile.impactDecal != null)
        {
            GameObject decalObj = Instantiate(rightArmWeapon.projectile.impactDecal.gameObject);
            decalObj.transform.position = raycastInfo.point;
            decalObj.transform.forward = raycastInfo.normal * -1f; // we added the (* -1f) to prevent the Zfighting~
        }
    }
    #endregion
}
