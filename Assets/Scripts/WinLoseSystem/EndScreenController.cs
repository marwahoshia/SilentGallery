using SilentGallery.UISystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SilentGallery.WinLoseSystem
{
    /// <summary>
    /// Shows the Win or Lose panel based on events from <see cref="GameOutcomeManager"/>,
    /// freezes gameplay, and handles the end screen's Restart / Main Menu buttons.
    /// Attach to a UI GameObject in the gameplay scene.
    /// </summary>
    public class EndScreenController : MonoBehaviour
    {
        /// <summary>The scene's GameOutcomeManager. Assign in the Inspector.</summary>
        [SerializeField]
        private GameOutcomeManager gameOutcomeManager;

        /// <summary>The panel shown when the player wins. Should start inactive in the scene.</summary>
        [SerializeField]
        private GameObject winPanel;

        /// <summary>The panel shown when the player loses. Should start inactive in the scene.</summary>
        [SerializeField]
        private GameObject losePanel;

        private void Start()
        {
            if (gameOutcomeManager != null)
            {
                gameOutcomeManager.OnGameWon += ShowWinScreen;
                gameOutcomeManager.OnGameLost += ShowLoseScreen;
            }
        }

        private void OnDestroy()
        {
            if (gameOutcomeManager != null)
            {
                gameOutcomeManager.OnGameWon -= ShowWinScreen;
                gameOutcomeManager.OnGameLost -= ShowLoseScreen;
            }
        }

        private void ShowWinScreen()
        {
            ShowEndScreen(winPanel);
        }

        private void ShowLoseScreen()
        {
            ShowEndScreen(losePanel);
        }

        private void ShowEndScreen(GameObject panel)
        {
            if (panel != null)
            {
                panel.SetActive(true);
            }
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /// <summary>
        /// Restores normal time scale and reloads the current scene from the start.
        /// Wire this to the end screen's "Restart" button's OnClick event.
        /// </summary>
        public void RestartLevel()
        {
            Time.timeScale = 1f;
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        /// <summary>
        /// Restores normal time scale and returns to the Main Menu scene.
        /// Wire this to the end screen's "Main Menu" button's OnClick event.
        /// </summary>
        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneNames.MAIN_MENU_SCENE);
        }
    }
}
