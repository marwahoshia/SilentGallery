using System;
using System.Collections.Generic;
using UnityEngine;

namespace SilentGallery.ClueSystem
{
    public class ClueManager : MonoBehaviour
    {
        private const int CLUES_REQUIRED_TO_WIN = 5;

        public static ClueManager Instance { get; private set; }

        public event Action<ClueData> OnClueRevealed;
        public event Action OnAllRequiredCluesRevealed;

        private readonly HashSet<string> revealedClueIds = new HashSet<string>();
        private bool winConditionAlreadyMet;

        public int RevealedClueCount => revealedClueIds.Count;
        public int CluesRequiredToWin => CLUES_REQUIRED_TO_WIN;

        private void Awake()
        {
            Instance = this;
        }

        public void RevealClue(ClueData clue)
        {
            if (clue == null)
            {
                return;
            }

            // دايمًا اعرض الـClue، حتى لو اللاعب شافه قبل
            OnClueRevealed?.Invoke(clue);

            // بس نحسبه ضمن التقدم أول مرة فقط
            if (!revealedClueIds.Contains(clue.ClueId))
            {
                revealedClueIds.Add(clue.ClueId);

                if (!winConditionAlreadyMet &&
                    revealedClueIds.Count >= CLUES_REQUIRED_TO_WIN)
                {
                    winConditionAlreadyMet = true;
                    OnAllRequiredCluesRevealed?.Invoke();
                }
            }
        }
    }
}