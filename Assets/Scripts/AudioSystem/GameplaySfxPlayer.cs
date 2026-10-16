using SilentGallery.ClueSystem;
using SilentGallery.WinLoseSystem;
using UnityEngine;

namespace SilentGallery.AudioSystem
{
    /// <summary>
    /// Plays sound effects in response to clue reveal and win/lose events via
    /// AudioManager, without ClueManager or GameOutcomeManager needing to know
    /// anything about audio. Attach to a GameObject in the Gallery scene and
    /// assign all fields in the Inspector.
    /// </summary>
    public class GameplaySfxPlayer : MonoBehaviour
    {
        /// <summary>The scene's ClueManager. Assign in the Inspector.</summary>
        [SerializeField]
        private ClueManager clueManager;

        /// <summary>The scene's GameOutcomeManager. Assign in the Inspector.</summary>
        [SerializeField]
        private GameOutcomeManager gameOutcomeManager;

        /// <summary>Played whenever a new clue is revealed.</summary>
        [SerializeField]
        private AudioClip clueFoundClip;

        /// <summary>Played when the player wins.</summary>
        [SerializeField]
        private AudioClip gameWonClip;

        /// <summary>Played when the player loses.</summary>
        [SerializeField]
        private AudioClip gameLostClip;

        private void Start()
        {
            if (clueManager != null)
            {
                clueManager.OnClueRevealed += HandleClueRevealed;
            }
            if (gameOutcomeManager != null)
            {
                gameOutcomeManager.OnGameWon += HandleGameWon;
                gameOutcomeManager.OnGameLost += HandleGameLost;
            }
        }

        private void OnDestroy()
        {
            if (clueManager != null)
            {
                clueManager.OnClueRevealed -= HandleClueRevealed;
            }
            if (gameOutcomeManager != null)
            {
                gameOutcomeManager.OnGameWon -= HandleGameWon;
                gameOutcomeManager.OnGameLost -= HandleGameLost;
            }
        }

        private void HandleClueRevealed(ClueData clue)
        {
            PlaySfx(clueFoundClip);
        }

        private void HandleGameWon()
        {
            PlaySfx(gameWonClip);
        }

        private void HandleGameLost()
        {
            PlaySfx(gameLostClip);
        }

        private void PlaySfx(AudioClip clip)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(clip);
            }
        }
    }
}
