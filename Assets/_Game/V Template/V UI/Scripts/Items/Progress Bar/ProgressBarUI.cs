using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VTemplate.UI
{
    public class ProgressBarUI : MonoBehaviour
    {
        [Title("Progress Bar Settings")]
        [SerializeField] private bool getSliderOnStart = true;

        [FoldoutGroup("References")]
        [Tooltip("The slider component representing the progress bar.")]
        public Slider progressBar;

        [FoldoutGroup("References")]
        [Tooltip("The TextMeshPro component to display the current slider value.")]
        public TextMeshProUGUI progressText;

        [FoldoutGroup("Animation")]
        [Tooltip("The duration of the progress fill animation.")]
        public float fillDuration = 1.0f;

        [FoldoutGroup("Animation")]
        [Tooltip("Ease type for the fill animation.")]
        public Ease fillEaseType = Ease.Linear;

        [FoldoutGroup("Animation")]
        [Tooltip("Value representing the minimum progress.")]
        [Range(0, 1)]
        public float minProgress = 0;

        [FoldoutGroup("Animation")]
        [Tooltip("Value representing the maximum progress.")]
        [Range(0, 1)]
        public float maxProgress = 1;

        [FoldoutGroup("Events")]
        [Tooltip("Event triggered when the progress bar reaches 100% completion.")]
        public UnityEngine.Events.UnityEvent onCompletion;

        [FoldoutGroup("Events")]
        [Tooltip("Event triggered when the progress bar reaches a specific milestone.")]
        public UnityEngine.Events.UnityEvent onMilestoneReached;

        public Action OnCompletionAction;
        public Action OnMilestoneReachedAction;

        [FoldoutGroup("Milestones")]
        [Tooltip("A list of milestones as percentages (0-100) to check for when updating the progress.")]
        [Range(0, 100)]
        public List<float> milestones = new List<float>();

        [Button("Fill Progress")]
        [FoldoutGroup("Debug")]
        [Tooltip("Manually fill the progress bar to a specified value.")]
        public void FillProgress(float targetProgress)
        {
            targetProgress = Mathf.Clamp(targetProgress, minProgress, maxProgress);
            progressBar.DOValue(targetProgress, fillDuration).SetEase(fillEaseType);
        }

        public float CurrentProgress { get; private set; }
        private float previousProgress = 0;
        private Tweener progressBarFillTweener;

        public void UpdateProgress(float newProgress)
        {
            float clampedProgress = Mathf.Clamp(newProgress, minProgress, maxProgress);

            if (clampedProgress < CurrentProgress)
            {
                // If the new progress is smaller than the current progress,
                // reverse the animation to decrease the value.
                progressBarFillTweener?.Kill(); // Stop the previous animation.
                progressBarFillTweener = progressBar.DOValue(clampedProgress, fillDuration).SetEase(fillEaseType);

                // Update the CurrentProgress to the new value.
                CurrentProgress = clampedProgress;
            }
            else if (clampedProgress > CurrentProgress)
            {
                // If the new progress is larger than the current progress,
                // play a new animation to increase the value.
                progressBarFillTweener = progressBar.DOValue(clampedProgress, fillDuration).SetEase(fillEaseType);

                // Update the CurrentProgress to the new value.
                CurrentProgress = clampedProgress;
            }

            // Check for milestone reached
            foreach (float milestone in milestones)
            {
                if (CurrentProgress >= milestone && previousProgress < milestone)
                {
                    OnMilestoneReachedAction?.Invoke();
                    onMilestoneReached.Invoke();
                }
            }

            // Check for completion
            if (CurrentProgress >= maxProgress)
            {
                OnCompletionAction?.Invoke();
                onCompletion.Invoke();
            }

            previousProgress = CurrentProgress;
        }
        private void UpdateProgressText(float value)
        {
            // Update the TextMeshPro text with the current slider value.
            progressText.text = $"{Mathf.RoundToInt(value * 100)}%";
        }

        public void PauseProgress()
        {
            progressBarFillTweener?.Pause();
        }

        public void ResumeProgress()
        {
            progressBarFillTweener?.Play();
        }

        private void Start()
        {
            if (getSliderOnStart)
                progressBar = GetComponent<Slider>();
            if (progressText) progressBar?.onValueChanged?.AddListener(UpdateProgressText);


        }
    }
}
