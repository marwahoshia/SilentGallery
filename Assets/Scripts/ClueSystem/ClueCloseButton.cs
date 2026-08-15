using UnityEngine;
using UnityEngine.UI;

namespace SilentGallery.ClueSystem
{
    public class ClueCloseButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject cluePanel;

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
            Debug.Log("CLOSE WORKED");

            if (cluePanel != null)
            {
                cluePanel.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}