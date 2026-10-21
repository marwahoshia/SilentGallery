using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using SilentGallery.InteractionSystem;

public class CombinationLock : MonoBehaviour, IInteractable
{
    [Header("The secret code")]
    public int[] secretCode = { 3, 7, 1 };

    [Header("UI")]
    public GameObject panel;
    public TextMeshProUGUI[] digitTexts;
    public TextMeshProUGUI feedbackText;

    [Header("Player scripts to turn off while the panel is open")]
     public GameObject player;

    [Header("Called when the code is correct")]
    public UnityEvent OnUnlocked;

    
    [Header("Called when the code is wrong")]
    public UnityEvent OnWrongCode;

    private int[] currentCode = new int[3];
    private bool isUnlocked = false;

    void Start()
    {
        panel.SetActive(false);
        UpdateDigits();
    }

    // ---------- IInteractable ----------
    public void Interact(GameObject interactor)
    {
        if (isUnlocked) return;
        OpenPanel();
    }

    public string GetInteractionPrompt()
    {
        if (isUnlocked) return "Unlocked";
        return "Press E to open the lock";
    }

        public void OpenPanel()
    {
        if (!panel.activeSelf) UIState.OpenPanel();
        panel.SetActive(true);
        feedbackText.text = "";
        SetPlayerControl(false);
    }

    public void ClosePanel()
    {
        if (panel.activeSelf) UIState.ClosePanel();
        panel.SetActive(false);
        SetPlayerControl(true);
    }

    void SetPlayerControl(bool canMove)
    {
        foreach (MonoBehaviour script in player.GetComponents<MonoBehaviour>())
        {
            script.enabled = canMove;
        }

        if (canMove)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // ---------- Buttons ----------
    public void IncreaseDigit(int index)
    {
        currentCode[index] = (currentCode[index] + 1) % 10;
        UpdateDigits();
    }

    public void DecreaseDigit(int index)
    {
        currentCode[index] = (currentCode[index] + 9) % 10;
        UpdateDigits();
    }

    void UpdateDigits()
    {
        for (int i = 0; i < digitTexts.Length; i++)
        {
            digitTexts[i].text = currentCode[i].ToString();
        }
    }

    public void Submit()
    {
        for (int i = 0; i < secretCode.Length; i++)
        {
            if (currentCode[i] != secretCode[i])
            {
                feedbackText.text = "Wrong code!";
                StartCoroutine(Shake());
                OnWrongCode.Invoke();
                return;
            }
        }

        isUnlocked = true;
        ClosePanel();
        OnUnlocked.Invoke();
    }

    IEnumerator Shake()
    {
        RectTransform rect = panel.GetComponent<RectTransform>();
        Vector2 start = rect.anchoredPosition;

        for (int i = 0; i < 3; i++)
        {
            rect.anchoredPosition = start + new Vector2(10, 0);
            yield return new WaitForSeconds(0.05f);
            rect.anchoredPosition = start - new Vector2(10, 0);
            yield return new WaitForSeconds(0.05f);
        }

        rect.anchoredPosition = start;
    }
}