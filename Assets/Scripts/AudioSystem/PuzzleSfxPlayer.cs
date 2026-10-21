using UnityEngine;

namespace SilentGallery.AudioSystem
{
    /// <summary>
    /// Plays puzzle sound effects via AudioManager by listening to the
    /// CombinationLock events, so the lock itself does not need to know about audio.
    /// Attach to a GameObject in the Gallery scene and assign all fields.
    /// </summary>
    public class PuzzleSfxPlayer : MonoBehaviour
    {
        [SerializeField] private CombinationLock combinationLock;
        [SerializeField] private AudioClip solvedClip;
        [SerializeField] private AudioClip failedClip;

        private void Start()
        {
            if (combinationLock == null) return;
            combinationLock.OnUnlocked.AddListener(HandleSolved);
            combinationLock.OnWrongCode.AddListener(HandleFailed);
        }

        private void OnDestroy()
        {
            if (combinationLock == null) return;
            combinationLock.OnUnlocked.RemoveListener(HandleSolved);
            combinationLock.OnWrongCode.RemoveListener(HandleFailed);
        }

        private void HandleSolved() { PlaySfx(solvedClip); }

        private void HandleFailed() { PlaySfx(failedClip); }

        private void PlaySfx(AudioClip clip)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(clip);
            }
        }
    }
}