using System;
using SilentGallery.ClueSystem;
using SilentGallery.TimerSystem;
using UnityEngine;

namespace SilentGallery.WinLoseSystem
{
    /// <summary>
    /// Decides whether the player has won or lost by listening to the Clue Reveal
    /// System and Countdown Timer, and raises a single outcome event for the End
    /// Screen UI to react to. Place exactly one instance of this in the gameplay scene.
    /// </summary>
    public class GameOutcomeManager : MonoBehaviour
    {
        /// <summary>The scene's ClueManager. Assign in the Inspector.</summary>
        [SerializeField]
        private ClueManager clueManager;

        /// <summary>The scene's CountdownTimer. Assign in the Inspector.</summary>
        [SerializeField]
        private CountdownTimer countdownTimer;

        /// <summary>Raised once, when the player reveals enough clues to win before time runs out.</summary>
        public event Action OnGameWon;

        /// <summary>Raised once, when the countdown timer expires before the player has won.</summary>
        public event Action OnGameLost;

        /// <summary>True once a win or loss has been decided, preventing either event from firing twice.</summary>
        public bool GameHasEnded { get; private set; }

        private void Start()
        {
            if (clueManager != null)
            {
                clueManager.OnAllRequiredCluesRevealed += HandleAllCluesRevealed;
            }
            if (countdownTimer != null)
            {
                countdownTimer.OnTimerExpired += HandleTimerExpired;
            }
        }

        private void OnDestroy()
        {
            if (clueManager != null)
            {
                clueManager.OnAllRequiredCluesRevealed -= HandleAllCluesRevealed;
            }
            if (countdownTimer != null)
            {
                countdownTimer.OnTimerExpired -= HandleTimerExpired;
            }
        }

        private void HandleAllCluesRevealed()
        {
            if (GameHasEnded)
            {
                return;
            }
            GameHasEnded = true;
            countdownTimer?.StopTimer();
            OnGameWon?.Invoke();
        }

        private void HandleTimerExpired()
        {
            if (GameHasEnded)
            {
                return;
            }
            GameHasEnded = true;
            OnGameLost?.Invoke();
        }
    }
}
