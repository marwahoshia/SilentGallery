using UnityEngine;
using UnityEngine.SceneManagement;

public static class UIState
{
    private static int _openPanels = 0;

    public static bool IsBlocking => _openPanels > 0;

    // _openPanels is static, so it survives scene reloads unless reset here.
    // Without this, reloading the scene while a panel is open (e.g. hitting
    // Restart from the Pause menu) would leave the counter stuck above zero
    // forever, permanently blocking player movement in the next playthrough.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        _openPanels = 0;
        SceneManager.sceneLoaded -= HandleSceneLoaded;
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _openPanels = 0;
    }

    public static void OpenPanel()
    {
        _openPanels++;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void ClosePanel()
    {
        _openPanels = Mathf.Max(0, _openPanels - 1);
        if (_openPanels == 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
