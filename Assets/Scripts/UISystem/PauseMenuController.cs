using UnityEngine;
using UnityEngine.SceneManagement;

namespace SilentGallery.UISystem
{
    /// <summary>
    /// Drives the in-game Pause Menu: toggling it open/closed with a key press,
    /// freezing/unfreezing gameplay time, and handling its buttons (Resume,
    /// Restart, Main Menu, Quit). Attach this to a GameObject in the gameplay
    /// scene and assign the pause panel in the Inspector.
    /// </summary>
    public class PauseMenuController : MonoBehaviour
    {
        /// <summary>
        /// The root UI GameObject (e.g. a Panel) that contains the pause menu's
        /// visuals and buttons. Assign in the Inspector. Should start inactive
        /// in the scene so the game isn't paused on load.
        /// </summary>
        [SerializeField]
        private GameObject pausePanel;

        /// <summary>
        /// The key that toggles the pause menu open and closed.
        /// </summary>
        [SerializeField]
        private KeyCode pauseKey = KeyCode.Escape;

        /// <summary>
        /// True while the pause menu is open and gameplay time is frozen.
        /// </summary>
        public bool IsPaused { get; private set; }

        private void Update()
        {
            if (Input.GetKeyDown(pauseKey))
            {
                if (IsPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        /// <summary>
        /// Opens the pause panel and freezes gameplay by setting Time.timeScale to 0.
        /// </summary>
        public void PauseGame()
        {
            IsPaused = true;
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
            Time.timeScale = 0f;
        }

        /// <summary>
        /// Closes the pause panel and resumes gameplay by restoring Time.timeScale to 1.
        /// Wire this to the "Resume" button's OnClick event.
        /// </summary>
        public void ResumeGame()
        {
            IsPaused = false;
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            Time.timeScale = 1f;
        }

        /// <summary>
        /// Restores normal time scale and reloads the current scene from the start.
        /// Wire this to the "Restart" button's OnClick event.
        /// </summary>
        public void RestartLevel()
        {
            Time.timeScale = 1f;
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        /// <summary>
        /// Restores normal time scale and returns to the Main Menu scene.
        /// Wire this to the "Main Menu" button's OnClick event.
        /// </summary>
        public void GoToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneNames.MAIN_MENU_SCENE);
        }

        /// <summary>
        /// Quits the application. Has no effect when running in the Unity Editor;
        /// only works in a built (.exe) version of the game.
        /// Wire this to the "Quit" button's OnClick event.
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            Debug.Log("QuitGame called - this only quits in a real build, not in the Editor.");
#endif
            Application.Quit();
        }
    }
}
