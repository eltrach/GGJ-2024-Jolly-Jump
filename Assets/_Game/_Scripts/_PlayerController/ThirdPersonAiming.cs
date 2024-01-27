using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ThirdPersonAiming : MonoBehaviour
{
    [SerializeField] private Rig aimLayer;
    [SerializeField] private Rig shotLayer;



    [Title("throwable")]
    public Rigidbody throwable;
    public Transform releasePosition;
    //public Transform releasePosition;

    public int availableShots = 30;
    public float throwStrenght = 10f;

    // visuals / line renderer
    [Title("Visuals ")]
    [SerializeField] LineRenderer lineRenderer;

    public int linePoints = 50;
    public float timeBetweenPoints = 0.1f;
    public float velocityOffset;
    public LayerMask throwableCollisionMask;


    public float aimToShotDuration = 0.3f;
    public float lerpSpeed = 5f;

    bool _isAiming;
    bool _canShot;
    bool _isShot;

    private ThirdPersonInputs _input;
    private Animator _animator;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject normalCamera;
    [SerializeField] private GameObject aimCamera;


    int aimLayerAnimator;
    int shotLayerAnimator;

    private void Start()
    {
        Init();

    }
    private void Init()
    {
        _input = GetComponent<ThirdPersonInputs>();
        _animator = GetComponent<Animator>();
        aimLayerAnimator = _animator.GetLayerIndex("AimingLayer");
        shotLayerAnimator = Animator.StringToHash("Attack");
        mainCamera = Camera.main;

        int throwableLayer = throwable.gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(throwableLayer, i))
            {
                throwableCollisionMask |= 1 << i; // magic
            }
        }
        GlobalRoot.UIManager.UpdateLaughterEmoji(availableShots);

    }

    private void SwitchCamera()
    {
        if (_isAiming)
        {
            normalCamera.SetActive(false);
            aimCamera.SetActive(true);
        }
        else
        {
            normalCamera.SetActive(true);
            aimCamera.SetActive(false);
        }
    }
    private void Update()
    {
        _isAiming = _input.Aim;
        _isShot = _input.Shot;

        if (_isAiming)
        {
            //print(" --> AIMING");
            RotateThePlayerToCameraRot();
            SwitchCamera();
            ChangeAimWeight(1);
            DrawProjectionLine();
            if (_isShot)
            {
                if (availableShots > 0)
                {
                    StartCoroutine(Shot());
                }
                else
                {
                    // non enough bullets
                    GlobalRoot.UIManager.EnableCollectMoreEmojis();
                }
            }

        }
        else
        {
            ChangeAimWeight(0);
            SwitchCamera();
            lineRenderer.enabled = false;
            _canShot = false;
        }
    }

    private void DrawProjectionLine()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        Vector3 startPosition = releasePosition.position;
        Vector3 startVelocity = throwStrenght * mainCamera.transform.forward / throwable.mass;
        lineRenderer.SetPosition(0, startPosition);
        int i = 0;
        for (float time = 0; time < linePoints - 1; time += timeBetweenPoints)
        {
            Vector3 point = startPosition + time * startVelocity;
            if (i != 0)
                point.y = startPosition.y + startVelocity.y * velocityOffset * time + (Physics.gravity.y / 2f * time * time);
            lineRenderer.SetPosition(i, point);
            i++;

            Vector3 lastPosition = lineRenderer.GetPosition(i - 1);
            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit,
                 (point - lastPosition).magnitude,
                 throwableCollisionMask))
            {
                lineRenderer.SetPosition(i, hit.point);
                lineRenderer.positionCount = i + 1;
                return;
            }
        }
    }
    private void ReleaseThrowable()
    {
        Rigidbody gernade = Instantiate(throwable.gameObject, releasePosition.position, Quaternion.identity).GetComponent<Rigidbody>();
        gernade.velocity = Vector3.zero;
        gernade.angularVelocity = Vector3.zero;
        gernade.isKinematic = false;
        gernade.freezeRotation = false;
        gernade.transform.SetParent(null, true);
        gernade.AddForce((throwStrenght * mainCamera.transform.forward), ForceMode.Impulse);
    }

    IEnumerator Shot()
    {
        print("---> SHOOT");

        _animator.SetTrigger(shotLayerAnimator);
        ReleaseThrowable();
        availableShots--;
        GlobalRoot.UIManager.UpdateLaughterEmoji(availableShots);
        yield return new WaitForSeconds(aimToShotDuration);
        _animator.ResetTrigger(shotLayerAnimator);
        ChangeAimWeight(0f);
        lineRenderer.enabled = false;
    }
    private void RotateThePlayerToCameraRot()
    {
        // Set the target rotation to match the camera's rotation, while preserving the y-rotation of the player
        Quaternion targetRotation = Camera.main.transform.rotation;
        targetRotation.eulerAngles = new(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        gameObject.transform.DORotate(targetRotation.eulerAngles, 0.3f);
    }

    public void IncreaseShots(int increaseBy)
    {
        availableShots += increaseBy;
        GlobalRoot.UIManager.UpdateLaughterEmoji(availableShots);
    }

    public void ChangeAimWeight(float targetWeight)
    {
        _animator.SetLayerWeight(aimLayerAnimator, targetWeight);
        aimLayer.weight = Mathf.Lerp(aimLayer.weight, targetWeight, lerpSpeed * Time.deltaTime);
    }
}
