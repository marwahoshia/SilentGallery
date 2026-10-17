using System;
using SilentGallery.ClueSystem;
using SilentGallery.TimerSystem;
using UnityEngine;

namespace SilentGallery.WinLoseSystem
{
    /// <summary>
    /// Decides whether the player has won or lost. Finding all required clues no
    /// longer wins outright - it hands off to ConclusionManager's final question,
    /// which calls back into <see cref="TriggerWin"/>/<see cref="TriggerLose"/>.
    /// The countdown timer can still end the game directly via
    /// <see cref="HandleTimerExpired"/>, including while that question is up.
    /// Raises a single outcome event for the End Screen UI to react to. Place
    /// exactly one instance of this in the gameplay scene.
    /// </summary>
    public class GameOutcomeManager : MonoBehaviour
    {
        /// <summary>The scene's ClueManager. Assign in the Inspector.</summary>
        [SerializeField]
        private ClueManager clueManager;

        /// <summary>The scene's CountdownTimer. Assign in the Inspector.</summary>
        [SerializeField]
        private CountdownTimer countdownTimer;

        /// <summary>The final conclusion quiz, shown once all clues are found. Assign in the Inspector.</summary>
        [SerializeField]
        private ConclusionManager conclusionManager;

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
            if (GameHasEnded) return;
            conclusionManager?.ShowConclusion();
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

        /// <summary>
        /// Wins the game immediately. Called by ConclusionManager once the
        /// player answers the final question correctly.
        /// </summary>
        public void TriggerWin()
        {
            if (GameHasEnded) return;
            GameHasEnded = true;
            countdownTimer?.StopTimer();
            OnGameWon?.Invoke();
        }

        /// <summary>
        /// Loses the game immediately. Called by ConclusionManager once the
        /// player exhausts their attempts on the final question.
        /// </summary>
        public void TriggerLose()
        {
            if (GameHasEnded) return;
            GameHasEnded = true;
            OnGameLost?.Invoke();
        }
    }
}
