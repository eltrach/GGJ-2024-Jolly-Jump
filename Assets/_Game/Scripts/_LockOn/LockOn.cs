using DG.Tweening;
using Enemy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOn : LockOnBehavior
{
    [Header("Lock-On Indicator")]
    [SerializeField] private Canvas lockOnCanvas;
    [SerializeField] private Image lockOnIndicator;

    [SerializeField] private float lockOnRange = 10f; // The maximum range to detect enemy targets
    [SerializeField] private LayerMask targetLayer; // The layer(s) that represent enemy targets
    [SerializeField] private float yIndicatorModifier = 2;

    [SerializeField] private float minDistanceToStop = 1f; // The minimum distance to stop moving towards the target
    [SerializeField] private float movementSpeed = 5f; // The speed to move towards the locked-on target
    [SerializeField] private float rotationToTargetSpeed = 30;

    [Header("Debug Gizmos")]
    [SerializeField] private List<Transform> charactersAround = new List<Transform>();
    [SerializeField] private float duration;

    [Header("Detection Gizmos")]
    [SerializeField] private Color detectionGizmoColor = Color.red;
    [SerializeField, Range(0f, 360f)] private float detectionAngle = 45f;

    private Transform currentTarget;
    private bool isMovingToTarget;
    private float detectedAngle;

    // Reference to the CharacterController component
    private ThirdPersonInputs inputs;
    float distanceToTarget;
    private void Awake()
    {
        inputs = GetComponent<ThirdPersonInputs>();
    }
    // Method to activate the lock-on on a specific target
    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
        isMovingToTarget = true;
    }
    // Method to deactivate the lock-on
    public void ClearTarget()
    {
        currentTarget = null;
        isMovingToTarget = false;
    }
    private void Update()
    {
        if (distanceToTarget != 0) LockOnVisuals(distanceToTarget);
        FindNearestEnemy();
    }
    public void Fire()
    {
        if (currentTarget == null) return;
        distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
        //Debug.Log("DISTANCE TO TARGET : " + distanceToTarget);

        if (inputs.Shot)
        {
            MoveTowardsTarget(currentTarget, duration, distanceToTarget);
        }
        if (Input.GetKeyDown(KeyCode.J)) MoveTowardsTarget(currentTarget, duration, distanceToTarget);

        LockOnVisuals(distanceToTarget);
    }
    public void MoveTowardsTarget(Transform target, float duration, float distanceToTarget)
    {
        //Debug.Log("is Close to Target" + (distanceToTarget >= 2 && distanceToTarget > lockOnRange));
        if (distanceToTarget >= 2 && distanceToTarget > lockOnRange) return;


        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        Vector3 horizontalDirectionToTarget = new Vector3(directionToTarget.x, 0f, directionToTarget.z);

        Quaternion targetRotation = Quaternion.LookRotation(horizontalDirectionToTarget);

        transform.DORotateQuaternion(targetRotation, duration);
        transform.DOMove(TargetOffset(target.transform), duration);
    }
    public Vector3 TargetOffset(Transform target)
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        return Vector3.MoveTowards(targetPosition, transform.position, .8f);
    }

    private void LockOnVisuals(float distanceToTarget)
    {
        // Update the position of the lock-on indicator if there is a target and it's within range
        if (currentTarget != null && lockOnCanvas != null && lockOnIndicator != null)
        {
            // Check if the target is within the lock-on range
            if (distanceToTarget <= lockOnRange)
            {
                lockOnIndicator.enabled = true;

                // Position the lock-on indicator above the target's position on the screen
                Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTarget.position);
                screenPos.y += yIndicatorModifier * 100;
                lockOnIndicator.rectTransform.position = screenPos;
            }
            else
            {
                // Target is outside the lock-on range, disable the indicator
                lockOnIndicator.enabled = false;
            }
        }
    }
    #region Utilities

    // Method to detect enemy targets and lock on to the closest one within the player's view
    // Method to find the nearest enemy within the player's view cone
    public void FindNearestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, lockOnRange, targetLayer);
        if (enemies.Length > 0)
        {
            Transform nearestTarget = null;
            float nearestDistanceSqr = Mathf.Infinity;
            Vector3 cameraDirection = Camera.main.transform.forward;

            foreach (Collider enemy in enemies)
            {
                //Debug.Log(enemy);
                // Check if the target has the ThirdPersonEnemy component
                ThirdPersonEnemy enemyComponent = enemy.GetComponent<ThirdPersonEnemy>();
                if (enemyComponent == null)
                    continue; // Skip this target if it doesn't have the ThirdPersonEnemy component

                // Calculate the direction from the player to the enemy
                Vector3 playerToEnemyDirection = enemy.transform.position - transform.position;

                // Calculate the angle between the camera's forward direction and the player-to-enemy direction
                float angleToEnemy = Vector3.Angle(cameraDirection, playerToEnemyDirection);
                detectedAngle = angleToEnemy;
                // If the enemy is within the player's view cone (e.g., within 45 degrees), consider it for locking on
                if (angleToEnemy < 45f)
                {
                    // Calculate the squared distance to the enemy (for efficiency)
                    float distanceSqr = playerToEnemyDirection.sqrMagnitude;
                    // If the enemy is closer than the previous nearest enemy, update the nearest target
                    if (distanceSqr < nearestDistanceSqr)
                    {
                        nearestDistanceSqr = distanceSqr;
                        nearestTarget = enemy.transform;
                    }
                }
            }
            if (nearestTarget != null)
            {
                SetTarget(nearestTarget);
                AddCharacterAround(nearestTarget);
            }
        }
    }
    // Method to add an enemy target to the list
    public void AddCharacterAround(Transform newCharacter)
    {
        if (!charactersAround.Contains(newCharacter))
        {
            charactersAround.Add(newCharacter);
        }
    }
    #endregion

    // Draw gizmos to all the target positions
    private void OnDrawGizmos()
    {
        //Handles.color = new Color(1, 1, 0, 0.2f);
        //Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, detectedAngle * 0.5f, lockOnRange);
        //Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -detectedAngle * 0.5f, lockOnRange);
        Gizmos.color = Color.red;
        if (charactersAround != null)
        {
            foreach (Transform target in charactersAround)
            {
                if (target != null)
                {
                    Gizmos.DrawLine(transform.position, target.position);
                }
            }
        }
    }
    // Alternative way to display the distance text without using canvas
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 20;

        if (charactersAround != null)
        {
            foreach (Transform target in charactersAround)
            {
                if (target != null)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
                    GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 100, 100), distanceToTarget.ToString("F1"), style);
                }
            }
        }
    }
}
