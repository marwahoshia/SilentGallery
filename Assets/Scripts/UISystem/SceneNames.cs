using UnityEngine;

namespace SilentGallery.UISystem
{
    /// <summary>
    /// Central place for scene name string constants used by menu navigation.
    /// Keeping these in one file means a scene rename only requires a single edit.
    /// </summary>
    public static class SceneNames
    {
        /// <summary>
        /// PLACEHOLDER: confirm with the team. Must exactly match the scene's
        /// file name and its entry in File &gt; Build Settings &gt; Scenes In Build.
        /// </summary>
        public const string MAIN_MENU_SCENE = "MainMenu";

        /// <summary>
        /// PLACEHOLDER: confirm with the team. Must exactly match the scene's
        /// file name and its entry in File &gt; Build Settings &gt; Scenes In Build.
        /// </summary>
        public const string GAMEPLAY_SCENE = "Gallery";
    }
}
