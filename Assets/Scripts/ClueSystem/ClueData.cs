using UnityEngine;

namespace SilentGallery.ClueSystem
{
    /// <summary>
    /// Designer-authored data for a single clue: its unique id, display title,
    /// and the text revealed to the player. Create instances via
    /// Assets &gt; Create &gt; Silent Gallery &gt; Clue Data.
    /// </summary>
    [CreateAssetMenu(fileName = "NewClue", menuName = "Silent Gallery/Clue Data")]
    public class ClueData : ScriptableObject
    {
        /// <summary>Unique identifier for this clue (used to prevent double-counting). Must be unique across all clues.</summary>
        [SerializeField]
        private string clueId;

        /// <summary>Short display title shown above the clue text, e.g. "The Torn Letter".</summary>
        [SerializeField]
        private string title;

        /// <summary>The full clue text revealed to the player when they inspect this object.</summary>
        [SerializeField]
        [TextArea(3, 10)]
        private string clueText;

        /// <summary>Unique identifier for this clue.</summary>
        public string ClueId => clueId;

        /// <summary>Short display title for this clue.</summary>
        public string Title => title;

        /// <summary>The full clue text revealed to the player.</summary>
        public string ClueText => clueText;
    }
}
