using UnityEngine;

public static class UIState
{
    private static int _openPanels = 0;

    public static bool IsBlocking => _openPanels > 0;

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
