using System;
using UnityEngine;

namespace SilentGallery.TimerSystem
{
    /// <summary>
    /// Counts down from a configurable starting duration and raises events as
    /// time passes and when it expires. Uses <see cref="Time.deltaTime"/>, so it
    /// automatically pauses whenever <see cref="Time.timeScale"/> is 0 (e.g. while
    /// the Pause Menu is open). Attach to a GameObject in the gameplay scene.
    /// </summary>
    public class CountdownTimer : MonoBehaviour
    {
        /// <summary>
        /// PLACEHOLDER: open question, confirm with the team how long the player
        /// has to find the clues before losing. Currently 180 seconds (3 minutes).
        /// </summary>
        [SerializeField]
        private float startingDurationSeconds = 180f;

        /// <summary>If true, the timer starts counting down automatically when the scene loads.</summary>
        [SerializeField]
        private bool autoStartOnSceneLoad = true;

        /// <summary>Raised every frame the timer is running, passing the remaining seconds.</summary>
        public event Action<float> OnTimeUpdated;

        /// <summary>Raised once, the moment the remaining time reaches zero.</summary>
        public event Action OnTimerExpired;

        /// <summary>Seconds remaining on the countdown.</summary>
        public float RemainingSeconds { get; private set; }

        /// <summary>True while the countdown is actively running.</summary>
        public bool IsRunning { get; private set; }

        private void Start()
        {
            if (autoStartOnSceneLoad)
            {
                StartTimer();
            }
        }

        private void Update()
        {
            if (!IsRunning)
            {
                return;
            }

            RemainingSeconds = Mathf.Max(0f, RemainingSeconds - Time.deltaTime);
            OnTimeUpdated?.Invoke(RemainingSeconds);

            if (RemainingSeconds <= 0f)
            {
                IsRunning = false;
                OnTimerExpired?.Invoke();
            }
        }

        /// <summary>
        /// Resets the remaining time to <see cref="startingDurationSeconds"/> and begins counting down.
        /// </summary>
        public void StartTimer()
        {
            RemainingSeconds = startingDurationSeconds;
            IsRunning = true;
            OnTimeUpdated?.Invoke(RemainingSeconds);
        }

        /// <summary>Stops the countdown without resetting the remaining time.</summary>
        public void StopTimer()
        {
            IsRunning = false;
        }
    }
}
