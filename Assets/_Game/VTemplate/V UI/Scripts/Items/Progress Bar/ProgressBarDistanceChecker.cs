using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VTemplate.UI;

public class ProgressBarDistanceChecker : MonoBehaviour
{
    [Tooltip("The target to keep track of for progress calculation.")]
    public Transform TargetToKeepTrackOn;

    [Tooltip("The starting position.")]
    public Transform startPosition;

    [Tooltip("The target position.")]
    public Transform targetPosition;

    [Tooltip("The UI progress bar to update.")]
    public ProgressBarUI progressBar;

    [Tooltip("The axis to calculate the distance on.")]
    public DistanceAxis distanceAxis = DistanceAxis.Y;

    [ShowInInspector]
    [ReadOnly]
    [Tooltip("The current progress based on the relative position of the target.")]
    private float _settedValue;

    public enum DistanceAxis
    {
        X,
        Y,
        Z
    }

    private void Update()
    {
        Updater();
    }

    private void Updater()
    {
        if (progressBar == null || startPosition == null || targetPosition == null || TargetToKeepTrackOn == null)
        {
            return;
        }

        float progress = CalculateProgress();

        // Update the progress bar value based on the calculated progress.
        progressBar.UpdateProgress(progress);
        // Update the internal _settedValue for reference.
        _settedValue = progress;
    }

    private float CalculateProgress()
    {
        float targetPos = GetPosition(TargetToKeepTrackOn.position);
        float startPos = GetPosition(startPosition.position);
        float finishPos = GetPosition(targetPosition.position);

        return (targetPos - startPos) / (finishPos - startPos);
    }
    private float GetPosition(Vector3 position)
    {
        switch (distanceAxis)
        {
            case DistanceAxis.X:
                return position.x;
            case DistanceAxis.Y:
                return position.y;
            case DistanceAxis.Z:
                return position.z;
            default:
                return 0f;
        }
    }

    // Helpers
    public void UpdateProgressBar(Transform startPosition, Transform targetPosition, Transform targetToTrack)
    {
        progressBar.UpdateProgress(0);
        TargetToKeepTrackOn = targetToTrack;
        this.targetPosition = targetPosition;
        this.startPosition = startPosition;
    }

    [Tooltip("Get the current target position.")]
    public Transform GetTargetPosition() => targetPosition;
}
