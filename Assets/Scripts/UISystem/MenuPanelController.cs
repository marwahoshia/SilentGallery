using UnityEngine;

namespace SilentGallery.UISystem
{
    /// <summary>
    /// Drives the Main Menu's secondary panels (How To Play, Settings, Credits):
    /// showing one closes any other that's currently open. Attach this to a
    /// GameObject in the Main Menu scene and assign all three panels in the
    /// Inspector.
    /// </summary>
    public class MenuPanelController : MonoBehaviour
    {
        /// <summary>The How To Play panel. Should start inactive in the scene.</summary>
        [SerializeField]
        private GameObject howToPlayPanel;

        /// <summary>The Settings panel. Should start inactive in the scene.</summary>
        [SerializeField]
        private GameObject settingsPanel;

        /// <summary>The Credits panel. Should start inactive in the scene.</summary>
        [SerializeField]
        private GameObject creditsPanel;

        /// <summary>
        /// Closes all three panels, then shows the How To Play panel.
        /// Wire this to the Main Menu's "How To Play" button's OnClick event.
        /// </summary>
        public void ShowHowToPlay()
        {
            CloseAll();
            if (howToPlayPanel != null)
            {
                howToPlayPanel.SetActive(true);
            }
        }

        /// <summary>
        /// Closes all three panels, then shows the Settings panel.
        /// Wire this to the Main Menu's "Settings" button's OnClick event.
        /// </summary>
        public void ShowSettings()
        {
            CloseAll();
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(true);
            }
        }

        /// <summary>
        /// Closes all three panels, then shows the Credits panel.
        /// Wire this to the Main Menu's "Credits" button's OnClick event.
        /// </summary>
        public void ShowCredits()
        {
            CloseAll();
            if (creditsPanel != null)
            {
                creditsPanel.SetActive(true);
            }
        }

        /// <summary>
        /// Hides all three panels. Wire this to each panel's CloseButton OnClick
        /// event, and to any "Back" button on the main menu itself.
        /// </summary>
        public void CloseAll()
        {
            if (howToPlayPanel != null)
            {
                howToPlayPanel.SetActive(false);
            }
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
            if (creditsPanel != null)
            {
                creditsPanel.SetActive(false);
            }
        }
    }
}
