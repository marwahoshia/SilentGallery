using UnityEngine;

namespace SilentGallery.ClueSystem
{
    /// <summary>
    /// Plays a one-shot "Reveal" animation on this object's Animator when its
    /// matching clue is revealed for the first time (not on re-inspection).
    /// Attach alongside an Animator whose controller has an Idle/Reveal state
    /// pair driven by a "Reveal" trigger parameter.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class ClueRevealAnimator : MonoBehaviour
    {
        /// <summary>Must match the ClueId of the clue this object represents.</summary>
        [SerializeField]
        private string clueId;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (ClueManager.Instance != null)
            {
                ClueManager.Instance.OnClueRevealed += HandleClueRevealed;
            }
        }

        private void OnDestroy()
        {
            if (ClueManager.Instance != null)
            {
                ClueManager.Instance.OnClueRevealed -= HandleClueRevealed;
            }
        }

        private void HandleClueRevealed(ClueData clue)
        {
            if (clue == null || clue.ClueId != clueId)
            {
                return;
            }

            // RevealClue() fires this event before marking the clue as
            // revealed, so if it's already revealed here, this call is a
            // re-inspection, not the first find - don't replay the animation.
            if (ClueManager.Instance != null && ClueManager.Instance.IsClueRevealed(clueId))
            {
                return;
            }

            animator.SetTrigger("Reveal");
        }
    }
}
