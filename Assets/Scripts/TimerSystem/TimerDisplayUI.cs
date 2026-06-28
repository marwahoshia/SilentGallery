using TMPro;
using UnityEngine;

namespace SilentGallery.TimerSystem
{
    /// <summary>
    /// Formats <see cref="CountdownTimer"/>'s remaining time as mm:ss and displays
    /// it in a TextMeshPro label. Attach to a UI GameObject and assign both fields
    /// in the Inspector.
    /// </summary>
    public class TimerDisplayUI : MonoBehaviour
    {
        /// <summary>The CountdownTimer to display. Assign the scene's CountdownTimer.</summary>
        [SerializeField]
        private CountdownTimer countdownTimer;

        /// <summary>The TextMeshPro label that displays the formatted time.</summary>
        [SerializeField]
        private TextMeshProUGUI timeText;

        private void Start()
        {
            if (countdownTimer != null)
            {
                countdownTimer.OnTimeUpdated += UpdateDisplay;
                UpdateDisplay(countdownTimer.RemainingSeconds);
            }
        }

        private void OnDestroy()
        {
            if (countdownTimer != null)
            {
                countdownTimer.OnTimeUpdated -= UpdateDisplay;
            }
        }

        private void UpdateDisplay(float remainingSeconds)
        {
            if (timeText == null)
            {
                return;
            }

            int minutes = Mathf.FloorToInt(remainingSeconds / 60f);
            int seconds = Mathf.FloorToInt(remainingSeconds % 60f);
            timeText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
