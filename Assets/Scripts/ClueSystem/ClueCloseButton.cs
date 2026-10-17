using UnityEngine;
using UnityEngine.UI;

namespace SilentGallery.ClueSystem
{
    public class ClueCloseButton : MonoBehaviour
    {
        [SerializeField]
        private ClueDisplayUI clueDisplayUI;

        private Button closeButton;

        private void Awake()
        {
            closeButton = GetComponent<Button>();

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(ClosePanel);
            }
        }

        private void ClosePanel()
        {
            if (clueDisplayUI != null)
            {
                clueDisplayUI.CloseClueDisplay();
            }
        }
    }
}