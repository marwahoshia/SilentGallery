using System.Collections;
using SilentGallery.UISystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SilentGallery.LoadingSystem
{
    /// <summary>
    /// Loads the gameplay scene asynchronously in the background and drives a
    /// progress bar while it loads, activating the scene once loading completes.
    /// Attach to a GameObject in the Loading Screen scene.
    /// </summary>
    public class LoadingScreenController : MonoBehaviour
    {
        /// <summary>The slider that visually shows load progress. Assign in the Inspector.</summary>
        [SerializeField]
        private Slider progressBar;

        /// <summary>The scene to load. Defaults to the main gameplay scene.</summary>
        [SerializeField]
        private string sceneToLoad = SceneNames.GAMEPLAY_SCENE;

        private void Start()
        {
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                if (progressBar != null)
                {
                    progressBar.value = operation.progress;
                }
                yield return null;
            }

            if (progressBar != null)
            {
                progressBar.value = 1f;
            }

            yield return new WaitForSeconds(1.3f);
            operation.allowSceneActivation = true;
        }
    }
}
