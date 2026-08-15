using UnityEngine;
using UnityEngine.SceneManagement;

namespace SilentGallery.UISystem
{
    /// <summary>
    /// Drives the buttons on the Main Menu screen: starting the game and quitting
    /// the application. Attach this to a GameObject in the Main Menu scene and wire
    /// its public methods to the corresponding Button OnClick events in the Inspector.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        /// <summary>
        /// Loads the gameplay scene defined in <see cref="SceneNames.GAMEPLAY_SCENE"/>.
        /// Wire this to the "Play" button's OnClick event.
        /// </summary>
        public void PlayGame()
        {
            SceneManager.LoadScene(SceneNames.GAMEPLAY_SCENE);
        }

        /// <summary>
        /// Quits play mode in the Unity Editor, or quits the application in a
        /// built (.exe) version of the game.
        /// Wire this to the "Quit" button's OnClick event.
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
